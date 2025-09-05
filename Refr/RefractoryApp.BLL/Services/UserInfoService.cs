using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Contracts;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.BLL.Services;

public class UserInfoService : IUserInfoService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserInfoService(IUnitOfWork uow, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<List<DTOUser>> GetAllUserInfoAsync()
    {
        var userInfos = await _uow.UserInfos.SelectAllAsync();
        return _mapper.Map<List<DTOUser>>(userInfos);
    }

    public async Task UpdateAsync(DTOUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (user.Id <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(user.Id));

        if (user.IsBlocked)
            throw new ArgumentException("Cannot update blocked user", nameof(user.IsBlocked));

        if (string.IsNullOrWhiteSpace(user.Position))
            throw new ArgumentException("Position is required", nameof(user.Position));

        if (string.IsNullOrWhiteSpace(user.Name))
            throw new ArgumentException("Name is required", nameof(user.Name));

        if (string.IsNullOrWhiteSpace(user.LastName))
            throw new ArgumentException("LastName is required", nameof(user.LastName));

        // Получаем существующую сущность и обновляем её поля
        var existingUserInfo = await _uow.UserInfos.SelectByIdAsync(user.Id);
        if (existingUserInfo == null)
            throw new ArgumentException("User not found", nameof(user.Id));

        existingUserInfo.Name = user.Name;
        existingUserInfo.LastName = user.LastName;
        existingUserInfo.Position = user.Position;
        
        await _uow.UserInfos.UpdateAsync(existingUserInfo);

        // Обновляем ApplicationUser (только email)
        var applicationUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (applicationUser != null)
        {
            applicationUser.Email = user.Email;
            await _userManager.UpdateAsync(applicationUser);
        }

        await _uow.SaveAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        // Удаляем связанные комментарии через прямой запрос
        var context = _uow.Context;
        var userComments = await context.Comments
            .Where(x => x.UserInfoId == userId)
            .ToListAsync();
        foreach (var comment in userComments)
        {
            await _uow.Comments.DeleteAsync(comment.Id);
        }

        // Удаляем связанные огнеупоры через прямой запрос
        var userRefractories = await context.Refractories
            .Where(x => x.UserInfoId == userId)
            .ToListAsync();
        foreach (var refractory in userRefractories)
        {
            await _uow.Refractories.DeleteAsync(refractory.Id);
        }

        // Удаляем пользователя
        await _uow.UserInfos.DeleteAsync(userId);
        await _uow.SaveAsync();
    }

    public async Task SetUserAvatarAsync(int userId, string avatar)
    {
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than 0", nameof(userId));

        if (string.IsNullOrWhiteSpace(avatar))
            throw new ArgumentException("Avatar cannot be empty", nameof(avatar));

        var userInfo = await _uow.UserInfos.SelectByIdAsync(userId);
        if (userInfo == null)
            throw new ArgumentException($"User with ID {userId} not found");

        userInfo.UserAvatar = avatar;
        await _uow.UserInfos.UpdateAsync(userInfo);
        await _uow.SaveAsync();
    }

    public async Task<bool> CheckLoginExistAsync(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return false;

        var users = await _uow.UserInfos.SelectAllAsync(x => x.Login == login);
        return users.Any();
    }

    public async Task<DTOUser?> GetUserByIdAsync(int id)
    {
        var userInfo = await _uow.UserInfos.SelectByIdAsync(id);
        return userInfo != null ? _mapper.Map<DTOUser>(userInfo) : null;
    }

    public async Task<DTOUser?> GetUserByIdWithRefractoriesAsync(int id)
    {
        var context = _uow.Context;
        var userInfo = await context.UserInfos
            .Include(u => u.Refractories)
            .ThenInclude(r => r.Comments)
            .ThenInclude(c => c.UserInfo)
            .FirstOrDefaultAsync(u => u.Id == id);
            
        return userInfo != null ? _mapper.Map<DTOUser>(userInfo) : null;
    }

    public async Task<DTOUser?> GetUserByLoginAsync(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return null;

        var users = await _uow.UserInfos.SelectAllAsync(x => x.Login == login);
        var userInfo = users.FirstOrDefault();
        return userInfo != null ? _mapper.Map<DTOUser>(userInfo) : null;
    }

    public void Dispose()
    {
        _uow?.Dispose();
    }
}