using System.Security.Claims;
using GradeCom.Dtos.Group;
using GradeCom.Dtos.Subject;
using GradeCom.Dtos.UserDtos;
using GradeCom.Models;
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
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpPost("group")]
    public async Task<IActionResult> CreateGroup([FromBody] GroupCreateDto dto)
    {
        await _userService.CreateGroup(dto);
        return Ok(new { message = "Group created successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpPost("groups/assign-students")]
    public async Task<IActionResult> AddStudentToGroup([FromBody] AssignStudentsToGroupDto dto)
    {
        await _userService.AddStudentToGroup(dto);
        return Ok(new { message = "Students added successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpDelete("groups/assign-students/delete")]
    public async Task<IActionResult> DeleteStudentFromGroup([FromBody] AssignStudentsToGroupDto dto)
    {
        await _userService.DeleteStudentFromGroup(dto);
        return Ok(new { message = "Students deleted successfully" });
    }
    
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpPost("subject")]
    public async Task<IActionResult> CreateSubject([FromBody] SubjectCreateDto dto)
    {
        await _userService.CreateSubject(dto);
        return Ok(new { message = "Subject created successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpPost("subject/addTeacher")]
    public async Task<IActionResult> AddTeacherToSubject([FromBody] TeacherAddDto dto)
    {
        await _userService.AddTeacherToSubject(dto);
        return Ok(new { message = "Teacher add successfully" });
    }

    
    // [HttpPut("upload-image/{id}")]
    // public async Task<IActionResult> UploadProfileImage(string id, IFormFile file)
    // {
    //     if (file == null || file.Length == 0)
    //         return BadRequest("No file uploaded");
    //     return Ok(_userService.Put(id, file));
    // }
}