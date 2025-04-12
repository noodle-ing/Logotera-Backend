using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GradeCom.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    
    
    public UserController(IAuthenticationService authenticationService, IUserService userService)
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
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Post(UserDto userDto) =>
        Ok(await _userService.Post(userDto));

    [Authorize]
    [HttpGet("{id:guid}")]

    public async Task<ActionResult<IEnumerable<UserDto>>> Get(string id) =>
        Ok(await _userService.Get(id));

    [Authorize]
    [HttpGet("{id:guid}&{directorId:guid}")]
    public async Task<ActionResult<UserDto>> Get(string id, string directorId) => 
        Ok(await _userService.Get(id, directorId));

    [Authorize]
    [HttpPut]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult<UserDto>> Put(UserDto userDto) =>
        Ok(await _userService.Put(userDto));

    [Authorize]
    [HttpDelete]
    [Authorize(Policy = "Admin")]
    public async Task<ActionResult> Delete(string id)
    {
        await _userService.Delete(id);

        return Ok("Объект был удален");
    }
}