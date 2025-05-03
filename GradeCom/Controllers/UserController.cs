using GradeCom.Dtos.UserDtos;
using GradeCom.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = GradeCom.Services.AuthenticationServices.IAuthenticationService;

namespace GradeCom.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly Services.AuthenticationServices.IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    
    
    public UserController( IAuthenticationService authenticationService, IUserService userService)
    {
        _authenticationService = authenticationService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request) =>
        Ok(await _authenticationService.Login(request));
    
    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request) =>
        Ok(await _authenticationService.Register(request));
    
    // [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    // [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Post(UserDto userDto) =>
        Ok(await _userService.Post(userDto));

    // [Authorize]
    // [HttpGet("{id:guid}")]
    //
    // public async Task<ActionResult<IEnumerable<UserDto>>> Get(string id) =>
    //     Ok(await _userService.Get(id));

    // [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> Get(string id) => 
        Ok(await _userService.Get(id));

    // [Authorize]
    [HttpPut]
    // [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Put(UserDto userDto) =>
        Ok(await _userService.Put(userDto));

    // [Authorize]
    [HttpDelete("{id:guid}")]
    // [Authorize(Policy = "Admin")]
    public async Task<ActionResult> Delete(string id)
    {
        await _userService.Delete(id);

        return Ok("Объект был удален");
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UserDto>> Patch(string id,string description) =>
        Ok(await _userService.Patch(id, description));
    
    
    // [HttpPut("upload-image/{id}")]
    // public async Task<IActionResult> UploadProfileImage(string id, IFormFile file)
    // {
    //     if (file == null || file.Length == 0)
    //         return BadRequest("No file uploaded");
    //     return Ok(_userService.Put(id, file));
    // }
}