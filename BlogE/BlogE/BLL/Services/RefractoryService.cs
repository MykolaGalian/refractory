using System;
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
    public class RefractoryService : IRefractoryService
    {
        private readonly IUnitOfWork _uow;
        public RefractoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public void Dispose()
        {
            _uow?.Dispose();
        }

        public async Task<List<DTORefractory>> GetAllRefractories()
        {
            var refractories = (await _uow.Refractory.SelectAll()).OrderByDescending(x => x.DateCreate);             

            if (refractories == null)
                throw new ArgumentException("Refractory not exist");
            return AutoMapper.Mapper.Map<IEnumerable<Refractory>, List<DTORefractory>>(refractories).ToList();
        }       


        public async Task AddRefractory(DTORefractory dtoRefractory)
        {
            var refractory = AutoMapper.Mapper.Map<DTORefractory, Refractory>(dtoRefractory);
            if (refractory.RefractoryDescription != null) 
            {
                await _uow.Refractory.Insert(refractory);
            }
            else throw new ArgumentException("Wrong data");}

     

        public async Task EditRefractory(DTORefractory dtoRefractory)
        {
            var refractory = AutoMapper.Mapper.Map<DTORefractory, Refractory>(dtoRefractory);

            if (refractory.UserInfoId != 0)
            {
                await _uow.Refractory.Update(refractory);
            }
            else throw new ArgumentException("Wrong data");
        }

        public async Task DeleteRefractory(int refId)
        {
            List<Comment> refractoryComments = (await _uow.Comments.SelectAll(x => x.RefractoryId == refId)).ToList();
            if (refractoryComments != null && refractoryComments.Count > 0)
                foreach (var refComent in refractoryComments)
                {
                    await _uow.Comments.Delete(refComent.Id);
                }

            await _uow.Refractory.Delete(refId);
        }

        public async Task<DTORefractory> GetRefractoryByRefId(int refId) 
        {
            var refractory = await _uow.Refractory.SelectById(refId);
            refractory.Comments = (await _uow.Comments.SelectAll(x => x.RefractoryId == refractory.Id)).ToList(); //add list of comments

            if (refractory == null)
                throw new ArgumentException("Refractory not exist");
            return AutoMapper.Mapper.Map<Refractory, DTORefractory>(refractory);
        }
        // for get refractory Id by refractory brand
        public async Task<DTORefractory> GetRefractoryByBrand(string refBrand)
        {            
            Refractory refractory = (await _uow.Refractory.SelectAll(x => x.RefractoryBrand == refBrand)).FirstOrDefault(); 

            if (refractory == null)
                throw new ArgumentException("Refractory not exist");
            return AutoMapper.Mapper.Map<Refractory, DTORefractory>(refractory);
        }

        public async Task<IEnumerable<DTORefractory>> GetRefractoriesByUserId(int userid)
        {
            var refractories = (await _uow.Refractory.SelectAll(x => x.UserInfoId == userid)).OrderByDescending(x => x.DateCreate).ToList();
            return AutoMapper.Mapper.Map<IEnumerable<Refractory>, IEnumerable<DTORefractory>>(refractories);
        }

      
        public async Task<IEnumerable<DTORefractory>> GetRefractoriesByType(string refType)  //tag
        {
            var refractories = (await _uow.Refractory.SelectAll(x => x.RefractoryType == refType)).OrderByDescending(x => x.DateCreate).ToList();
            return AutoMapper.Mapper.Map<IEnumerable<Refractory>, IEnumerable<DTORefractory>>(refractories);
        }

        public async Task<IEnumerable<string>> GetAllRefTypes() //getting all unique types of refractories//
        {
            var refractories = (await _uow.Refractory.SelectAll());
            var refTypes = from _ref in refractories
                select new { refractoryType = _ref.RefractoryType };

           List<string> refTypeList = new List<string>();
           foreach (var r in refTypes)
           {
               refTypeList.Add(r.refractoryType);
           }

           return refTypeList.Distinct(); //only unique ref types
        }
    }
}
