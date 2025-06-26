using System.Security.Claims;
using Logotera.Dtos.UserDtos;
using Logotera.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services_AuthenticationServices_IAuthenticationService = Logotera.Services.AuthenticationServices.IAuthenticationService;

namespace Logotera.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly Services.AuthenticationServices.IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    
    
    public UserController( Services_AuthenticationServices_IAuthenticationService authenticationService, IUserService userService)
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
    
    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    // [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Post(UserDto userDto) =>
        Ok(await _userService.Post(userDto));



    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
            return Unauthorized();

        var userDto = await _userService.Get(email);
        return Ok(userDto);
    }

    // [Authorize]
    [HttpPut]
    // [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Put(UserDto userDto) =>
        Ok(await _userService.Put(userDto));

    [Authorize]
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> Delete(string id)
    {
        await _userService.Delete(id);

        return Ok("Объект был удален");
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UserDto>> Patch(string id,string description) =>
        Ok(await _userService.Patch(id, description));
    
    
    [Authorize]
    [Authorize(Policy = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
            return Unauthorized();
        
        var users = await _userService.GetAllUsersAsync(email);
        return Ok(users);
    }
    
}