﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;
using DAL.Models;

namespace BLL.Services
{
    class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        public CommentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Dispose()
        {
            _uow?.Dispose();
        }

        public async Task AddComment(DTOComment dtoComment)
        {
            var comment = AutoMapper.Mapper.Map<DTOComment, Comment>(dtoComment);
            if (dtoComment.UserInfoId > 0 && dtoComment.CommentBody.Length > 0)
            {
                await _uow.Comments.Insert(comment);
            }
            else throw new ArgumentException("Wrong data");
        }

      

        public async Task DeleteComment(int dtoCommentId)
        {
            await _uow.Comments.Delete(dtoCommentId);
        }

        public async Task<IEnumerable<DTOComment>> GetCommentsForRefractory(int Id)
        {
            var comments = (await _uow.Comments.SelectAll(x => x.RefractoryId == Id)).OrderByDescending(x => x.DateCreation).ToList();
            
            return AutoMapper.Mapper.Map<IEnumerable<Comment>, IEnumerable<DTOComment>>(comments);
        }

        public async Task<DTOComment> GetCommentById(int comId)
        {
            return AutoMapper.Mapper.Map<Comment, DTOComment>(await _uow.Comments.SelectById(comId));
        }
    }
}
