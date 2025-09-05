using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RefractoryApp.Api.Models.Refractory;
using RefractoryApp.Api.Services;
using RefractoryApp.BLL.Contracts;
using RefractoryApp.BLL.DTO;
using RefractoryApp.DAL.Identity;

namespace RefractoryApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RefractoryController : ControllerBase
{
    private readonly IBLLUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;

    public RefractoryController(
        IBLLUnitOfWork uow,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        IWebHostEnvironment env)
    {
        _uow = uow;
        _userManager = userManager;
        _mapper = mapper;
        _env = env;
    }

    [HttpGet("getallrefractories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllRefractories()
    {
        var refractories = await _uow.RefractoryService.GetAllRefractoriesAsync();
        var filteredRefractories = refractories.Where(x => !x.IsBlocked).ToList();
        
        if (!filteredRefractories.Any())
            return NotFound();

        var refractoryViewModels = _mapper.Map<List<RefractoryViewModel>>(filteredRefractories);
        return Ok(refractoryViewModels);
    }

    [HttpGet("{refId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRefractoryByRefId(int refId)
    {
        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        var refractoryViewModel = _mapper.Map<RefractoryViewModel>(refractory);
        return Ok(refractoryViewModel);
    }

    [HttpPost("getrefractoriesbytype")]
    [AllowAnonymous]
    
    public async Task<IActionResult> GetRefractoriesByType([FromForm] string type)
    {
        var refractories = await _uow.RefractoryService.GetRefractoriesByTypeAsync(type);
        if (refractories == null || !refractories.Any())
            return NotFound();

        var refractoryViewModels = _mapper.Map<List<RefractoryViewModel>>(refractories);
        return Ok(refractoryViewModels);
    }

    [HttpGet("getallrefractoriesmoder")]
    public async Task<IActionResult> GetAllRefractoriesModer()
    {
        var refractories = await _uow.RefractoryService.GetAllRefractoriesAsync();
        if (!refractories.Any())
            return NotFound();

        var refractoryViewModels = _mapper.Map<List<RefractoryViewModel>>(refractories);
        return Ok(refractoryViewModels);
    }

    [HttpGet("getalltypes")]
    [AllowAnonymous]
    
    public async Task<IActionResult> GetAllTypes()
    {
        var types = await _uow.RefractoryService.GetAllRefTypesAsync();
        if (types == null || !types.Any())
            return NotFound();

        return Ok(types);
    }

    [HttpGet("image/get")]
    [AllowAnonymous]
    
    public IActionResult GetImageForRef(string imageName, string userLogin)
    {
        var imagePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images", userLogin, "refractories", imageName);
        
        if (!System.IO.File.Exists(imagePath))
            return NotFound();

        var imageBytes = System.IO.File.ReadAllBytes(imagePath);
        return File(imageBytes, "application/octet-stream");
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddRefractory(
        IFormFile refPicture,
        [FromForm] string type,
        [FromForm] string brand,
        [FromForm] string description,
        [FromForm] float density,
        [FromForm] float maxWorkTemperature,
        [FromForm] float lime,
        [FromForm] float alumina,
        [FromForm] float silica,
        [FromForm] float magnesia,
        [FromForm] float carbon,
        [FromForm] float price)
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();

        var author = await _uow.UserInfoService.GetUserByIdAsync(userId);
        if (author == null)
            return Unauthorized();

        if (author.IsBlocked)
            return BadRequest("Your account blocked.");

        if (refPicture == null || refPicture.Length < 2)
            return BadRequest("No image");

        var filename = await AddImageAsync(refPicture);

        var newRefractory = new DTORefractory
        {
            RefractoryDescription = description ?? "",
            RefractoryType = type,
            RefractoryBrand = brand,
            DateCreate = DateTime.Now,
            UserInfoId = author.Id,
            RefractoryPicture = filename,
            Density = density,
            MaxWorkTemperature = maxWorkTemperature,
            Lime = lime,
            Alumina = alumina,
            Silica = silica,
            Magnesia = magnesia,
            Carbon = carbon,
            Price = price
        };

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _uow.RefractoryService.AddRefractoryAsync(newRefractory);
        return Ok("Refractory added");
    }

    [HttpPut("{refId}")]
    public async Task<IActionResult> EditRefractory(int refId, [FromBody] RefractoryEditViewModel uRef)
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();

        var author = await _uow.UserInfoService.GetUserByIdAsync(userId);
        if (author == null)
            return Unauthorized();

        if (author.IsBlocked)
            return BadRequest("Your account blocked.");

        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        if (refractory.UserInfo?.Id != author.Id)
            return BadRequest("It is not your refractory.");

        if (refractory.IsBlocked)
            return BadRequest("Refractory blocked.");

        // Создаем правильный DTO для обновления
        var updatedRefractory = _mapper.Map<RefractoryEditViewModel, DTORefractory>(uRef);
        updatedRefractory.Id = refId;
        updatedRefractory.UserInfoId = refractory.UserInfoId;
        updatedRefractory.DateCreate = refractory.DateCreate;
        updatedRefractory.RefractoryPicture = refractory.RefractoryPicture;
        updatedRefractory.IsBlocked = refractory.IsBlocked;

        await _uow.RefractoryService.EditRefractoryAsync(updatedRefractory);
        return Ok("Refractory edited");
    }

    [HttpPost("getrefbybrand")]
    
    public async Task<IActionResult> GetRefByBrand([FromBody] RefractoryEditViewModel refractory)
    {
        if (string.IsNullOrEmpty(refractory.RefractoryBrand))
            return NotFound();

        var result = await _uow.RefractoryService.GetRefractoryByBrandAsync(refractory.RefractoryBrand);
        if (result == null)
            return NotFound();

        var refractoryViewModel = _mapper.Map<RefractoryViewModel>(result);
        return Ok(refractoryViewModel);
    }

    [HttpDelete("{refId}")]
    public async Task<IActionResult> DeleteRefractory(int refId)
    {
        var currentUserId = _userManager.GetUserId(User);
        if (currentUserId == null || !int.TryParse(currentUserId, out var userId))
            return Unauthorized();

        var author = await _uow.UserInfoService.GetUserByIdAsync(userId);
        if (author == null)
            return Unauthorized();

        if (author.IsBlocked)
            return BadRequest("Your account blocked.");

        var refractory = await _uow.RefractoryService.GetRefractoryByRefIdAsync(refId);
        if (refractory == null)
            return NotFound();

        if (refractory.UserInfo?.Id != author.Id)
            return BadRequest("It is not your refractory.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _uow.RefractoryService.DeleteRefractoryAsync(refId);
        return Ok("Refractory removed");
    }

    [HttpPost("calcRefRow")]
    [AllowAnonymous]
    
    public IActionResult CalcRefRow([FromBody] RefractoryCalculationModel refractory)
    {
        var result = CalcService.CalcRefManual(refractory);
        return Ok(result);
    }

    private async Task<string> AddImageAsync(IFormFile image)
    {
        var userLogin = _userManager.GetUserName(User);
        if (userLogin == null)
            throw new UnauthorizedAccessException("User not found");
        var imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

        var userRefractoriesDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "Images", userLogin, "refractories");
        if (!Directory.Exists(userRefractoriesDir))
            Directory.CreateDirectory(userRefractoriesDir);

        var filePath = Path.Combine(userRefractoriesDir, imageName);
        
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return imageName;
    }
}