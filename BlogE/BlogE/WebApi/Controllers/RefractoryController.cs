using System;

using System.Web.Http;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Contracts;
using Microsoft.AspNet.Identity;
using WebApi.Models.Post;
using WebApi.Models.User;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Text;

namespace WebApi.Controllers
{
   
    [Authorize]
    [RoutePrefix("api/refractory")]
    public class RefractoryController : ApiController
    {
        private readonly IBLLUnitOfWork _uow;

        public RefractoryController(IBLLUnitOfWork uow)
        {
            _uow = uow;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("getallrefractories")]   
        public async Task<IHttpActionResult> GetAllRefractories()
        {

            var refractories = (await _uow.RefractoryService.GetAllRefractories()).ToList();
            refractories.RemoveAll(x => x.IsBlocked == true);      // remove all Blocked refractories
            if (refractories == null)
                NotFound();  //code 404         
        

          IEnumerable <RefractoryViewModel> Refractories = AutoMapper.Mapper.Map<IEnumerable<DTORefractory>, List<RefractoryViewModel>>(refractories);            
          return Ok(Refractories);
        }

        [HttpGet]
        [Route("{refId}")]
        public async Task<IHttpActionResult> GetRefractoryByRefId(int refId)
        {

            var refractory = (await _uow.RefractoryService.GetRefractoryByRefId(refId));
            if (refractory == null)
                NotFound();  //code 404         
            RefractoryViewModel Refaractory = AutoMapper.Mapper.Map<DTORefractory, RefractoryViewModel>(refractory);
            return Ok(Refaractory);
        }
     

        [HttpPost]
        [AllowAnonymous]
        [Route("getrefractoriesbytype")]
        public async Task<IHttpActionResult> GetRefractoriesByType()
        {
            var httpRequest = HttpContext.Current.Request;
            string type = httpRequest.Params["Type"]; //Teg

            var refractories = (await _uow.RefractoryService.GetRefractoriesByType(type));
            if (refractories == null)
                NotFound();  //code 404         
            IEnumerable<RefractoryViewModel> Refractories = AutoMapper.Mapper.Map<IEnumerable<DTORefractory>, List<RefractoryViewModel>>(refractories);
            return Ok(Refractories);
        }

        [HttpGet]
        [Route("getallrefractoriesmoder")]  
        public async Task<IHttpActionResult> GetAllRefractoriesModer()
        {

            var refractories = (await _uow.RefractoryService.GetAllRefractories()).ToList();
            if (refractories == null)
                NotFound();  //code 404         

            IEnumerable<RefractoryViewModel> Refractories = AutoMapper.Mapper.Map<IEnumerable<DTORefractory>, List<RefractoryViewModel>>(refractories);
            return Ok(Refractories);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getalltypes")] 
        public async Task<IHttpActionResult> GetAllTypes()
        {
            var types = await _uow.RefractoryService.GetAllRefTypes();
            if (types == null)
                NotFound();  //code 404 
            
            return Ok(types);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("image/get")]
        public IHttpActionResult GetImageForRefractory([FromUri]string imageName, [FromUri] string userLogin)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK); //made HttpResponseMessage 

            var path = "~/Images/" + userLogin + "/refractories/" + imageName; //made new path to local server storage for images for post
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/" + userLogin + "/refractories/")))
                return null;

            path = HttpContext.Current.Server.MapPath(path); // add path
            var contents = File.ReadAllBytes(path);  // convert required file into bytes -read image from server storage

            var ms = new MemoryStream(contents); // create an instance of MemoryStream by passing the bytes
            result.Content = new StreamContent(ms); // add the MemoryStream object to the HttpResponseMessage Content

            //file type which we trusted - we have to use application/octet-stream
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            var response = ResponseMessage(result);
            return response;

            //www.c-sharpcorner.com/article/sending-files-from-web-api/
        }
        
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddRefractory()
        {
            var authtor = await _uow.UserInfoService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            var httpRequest = HttpContext.Current.Request;  //get request object
            HttpPostedFile refFile = httpRequest.Files["RefPicture"];  //Gets the collection of files uploaded by the client, in multipart MIME format.
            if (refFile == null || refFile.ContentLength < 2)
                return BadRequest("No image");

            string filename = AddImage(refFile);  //add image for post     


            DTORefractory newRefractory = new DTORefractory
            {
                RefractoryDescription = "", //descriptionRef Body
                RefractoryType = httpRequest.Params["Type"],
                RefractoryBrand = httpRequest.Params["Brand"],
                DateCreate = DateTime.Now,
                UserInfoId = authtor.Id,
                RefractoryPicture = filename,
                Density = float.Parse(httpRequest.Params["Density"]),
                MaxWorkTemperature = float.Parse(httpRequest.Params["MaxWorkTemperature"]),                
                Lime = float.Parse(httpRequest.Params["Lime"]),
                Alumina = float.Parse(httpRequest.Params["Alumina"]),
                Silica = float.Parse(httpRequest.Params["Silica"]),
                Magnesia = float.Parse(httpRequest.Params["Magnesia"]),
                Carbon = float.Parse(httpRequest.Params["Carbon"]),
                Price = float.Parse(httpRequest.Params["Price"])
            };

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            await _uow.RefractoryService.AddRefractory(newRefractory);
            return Content(HttpStatusCode.Created, "Refractory added");
        }

        [HttpPut]
        [Route("{refId}")]
        public async Task<IHttpActionResult> EditRefractory([FromUri]int refId, [FromBody] RefractoryEditViewModel updateRef)
        {
            var authtor = await _uow.UserInfoService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            DTORefractory refractory = await _uow.RefractoryService.GetRefractoryByRefId(refId);
            if (refractory == null)
                return NotFound();

            if (refractory.UserInfo.Id != authtor.Id)
                return BadRequest("It is not your refractory.");

            if (refractory.IsBlocked)
                return BadRequest("Refractory blocked.");

            refractory.RefractoryDescription = updateRef.RefractoryDescription;
            refractory.RefractoryType = updateRef.RefractoryType;
            refractory.LastEdit = DateTime.Now;

            refractory.Density = updateRef.Density;
            refractory.MaxWorkTemperature = updateRef.MaxWorkTemperature;            
            refractory.Lime = updateRef.Lime;
            refractory.Alumina = updateRef.Alumina;
            refractory.Silica = updateRef.Silica;
            refractory.Magnesia = updateRef.Magnesia;
            refractory.Carbon = updateRef.Carbon;
            refractory.Price = updateRef.Price;

            refractory.UserInfo = null;
            refractory.Comments = null;

            await _uow.RefractoryService.EditRefractory(refractory);
            return Ok("Refractory edited");
        }

        // for geting post id by refractory brand
        [HttpPost]
        [AllowAnonymous]
        [Route("getrefbybrand")] 
        public async Task<IHttpActionResult> GetRefByBrand([FromBody] RefractoryEditViewModel Refractory)
        {
            if (Refractory.RefractoryBrand == null)
                NotFound();  //code 404    

            var refractory = (await _uow.RefractoryService.GetRefractoryByBrand(Refractory.RefractoryBrand));
            if (refractory == null)
                NotFound();  //code 404         
            RefractoryViewModel refractory_ = AutoMapper.Mapper.Map<DTORefractory, RefractoryViewModel>(refractory);
            return Ok(refractory_);
        }

        [HttpDelete]
        [Route("{refId}")]
        public async Task<IHttpActionResult> DeleteRefractory([FromUri]int refId)  //optional
        {
            var authtor = await _uow.UserInfoService.GetUserById(User.Identity.GetUserId<int>());
            if (authtor == null)
                return this.Unauthorized();

            if (authtor.IsBlocked)
                return BadRequest("Your account blocked.");

            DTORefractory refractory = await _uow.RefractoryService.GetRefractoryByRefId(refId);
            if (refractory == null)
                return NotFound();

            if (refractory.UserInfo.Id != authtor.Id)
                return BadRequest("It is not your refractory.");

            if (!this.ModelState.IsValid)
                return BadRequest(this.ModelState);

            await _uow.RefractoryService.DeleteRefractory(refId);
            return Ok("Refractory removed");
        }

        private string AddImage(HttpPostedFile image)
        {
            string userLogin = User.Identity.GetUserName();
            
            string imageName = new string(Path.GetFileNameWithoutExtension(image.FileName).Take(5).ToArray()); //made image name 
            imageName = imageName + Path.GetExtension(image.FileName);

            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Images/" + userLogin + "/refractories"))) //made directory  for user ref image on webapi/Images/user/refractories
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Images/" + userLogin + "/refractories"));

            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + userLogin + "/refractories/" + imageName); // made new path to local server storage
            image.SaveAs(filePath);

            return imageName;

        }

    }
}
