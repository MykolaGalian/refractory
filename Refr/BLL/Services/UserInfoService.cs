using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;
using DAL.Identity;
using DAL.Models;



namespace BLL.Services
{
    public  class UserInfoService : IUserInfoService
    {
        private readonly IUnitOfWork _uow;
        public UserInfoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<DTOUser>> GetAllUserInfo()
        {
            List<DTOUser> users = AutoMapper.Mapper.Map<IEnumerable<UserInfo>, IEnumerable<DTOUser>>(await _uow.UserInfo.SelectAll()).ToList();
            return users;
        }
       
        public async Task Update(DTOUser user)
        {
            if(user != null && user.Id > 0 && user.IsBlocked == false && user.Position.Length > 1 && user.Name.Length > 1 &&
              user.LastName.Length > 1)
            {
               
                var userInfo = AutoMapper.Mapper.Map<DTOUser, UserInfo>(user);
                await _uow.UserInfo.Update(userInfo);

                var userIdent = await _uow.UserManager.FindByIdAsync(userInfo.Id);
                userIdent.Email = user.Email;  //change only email 
                await _uow.UserManager.UpdateAsync(userIdent);
                await _uow.Save();

            }
            else throw new ArgumentException("Wrong user data");
        }

        public async Task Delete(int userId)  // optional
        {
            // delete comments
            List<Comment> userComments = new List<Comment>();
            userComments.AddRange(await _uow.Comments.SelectAll(x => x.UserInfoId == userId));
            foreach (var i in userComments)
            {
                await _uow.Comments.Delete(i.Id);
            }

            //delete refractories
            List<Refractory> userRefractories = new List<Refractory>();
            userRefractories.AddRange(await _uow.Refractory.SelectAll(x => x.UserInfoId == userId));
            foreach (var i in userRefractories)
            {
                await _uow.Refractory.Delete(i.Id);
            }

            await _uow.UserInfo.Delete(userId);

        }

        public async Task SetUserAvatar(int userId, string avatar)
        {
            (await _uow.UserInfo.SelectById(userId)).UserAvatar = avatar;
            await _uow.Save();
        }

        public async Task<bool> CheckLoginExist(string login)
        {
            
            return (await _uow.UserInfo.SelectAll(x => x.Login == login)).Any();
        }

        public async Task<DTOUser> GetUserById(int id)
        {
            return AutoMapper.Mapper.Map<UserInfo, DTOUser>(await _uow.UserInfo.SelectById(id));
        }

        public async Task<DTOUser> GetUserByLogin(string login) //login = userName
        {
            return AutoMapper.Mapper.Map<UserInfo, DTOUser>((await _uow.UserInfo.SelectAll(x => x.Login == login)).FirstOrDefault());
        }
       

        public void Dispose()
        {
            _uow?.Dispose();
        }
    }
}
