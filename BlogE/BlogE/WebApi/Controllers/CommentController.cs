﻿using BLL.Contracts;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Identity;
using WebApi.Models;
using WebApi.Models.Comment;
using System.Net;

namespace WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/comments")]
    public class CommentController : ApiController
    {
        private readonly IBLLUnitOfWork _uow;

        public CommentController(IBLLUnitOfWork uow)
        {
            _uow = uow;
        }

       [HttpPost]
       [Route("{postId}")]
        public async Task<IHttpActionResult> AddComment([FromUri]int postId)
        {
            var httpRequest = HttpContext.Current.Request;
            string comment = httpRequest.Params["Coment"];

            DTOPost post = await _uow.PostService.GetPostByPosId(postId);
            if (post == null)
                return NotFound();            

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
                return this.Unauthorized();

            if (comment.Length < 1)
                ModelState.AddModelError("Body", "Please write comment.");

            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            DTOUser user = await _uow.UserInfoService.GetUserById(User.Identity.GetUserId<int>());
            DTOComment dtoComment = new DTOComment { DateCreation = DateTime.Now, PostId = postId, UserInfoId = user.Id, CommentBody = comment };

            await _uow.CommentService.AddComment(dtoComment);
            return Content(HttpStatusCode.Created, "Comment added.");
        }


        [HttpGet]
        [Route("{postId}")]
        public async Task<IHttpActionResult> GetComments([FromUri]int postId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            DTOPost post = await _uow.PostService.GetPostByPosId(postId);
            if (post == null)
                return NotFound();

            if (post.IsBlocked)
                return BadRequest("This post blocked");

            var userId = User.Identity.GetUserId();
            if (userId == null)
                return this.Unauthorized();

            List<CommentViewModel> comments = AutoMapper.Mapper.Map<IEnumerable<DTOComment>, List<CommentViewModel>>(
            await _uow.CommentService.GetCommentsToPost(postId));

            if (comments != null)
                return Ok(comments);
            else return NotFound();
        }

        [HttpDelete]
        [Route("{commentId}")]
        public async Task<IHttpActionResult> Delete([FromUri] int commentId)
        {
            if (!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);

            if (User.Identity.GetUserName() == null)
                return this.Unauthorized();

            var userId = User.Identity.GetUserId<int>();

            var comment = await _uow.CommentService.GetCommentByComId(commentId);
            if (comment == null)
                return NotFound();

            DTOPost post = await _uow.PostService.GetPostByPosId(comment.PostId);
            if (post == null)
                return NotFound();

            if (comment.UserInfoId != userId)
                return BadRequest("It is not your comment");

            await _uow.CommentService.DeleteComment(commentId);
            return Ok("Comment deleted");
        }
    }
}