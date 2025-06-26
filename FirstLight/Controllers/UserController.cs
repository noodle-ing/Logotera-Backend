using System.Security.Claims;
using FirstLight.Dtos.Group;
using FirstLight.Dtos.ModuleDto;
using FirstLight.Dtos.Subject;
using FirstLight.Dtos.UserDtos;
using FirstLight.Models;
using FirstLight.Services.UserServices;
using FirstLight.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationServices_IAuthenticationService = FirstLight.Services.AuthenticationServices.IAuthenticationService;
using IAuthenticationService = FirstLight.Services.AuthenticationServices.IAuthenticationService;

namespace FirstLight.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController : ControllerBase
{
    private readonly Services.AuthenticationServices.IAuthenticationService _authenticationService;
    private readonly IUserService _userService;
    
    
    public UserController( AuthenticationServices_IAuthenticationService authenticationService, IUserService userService)
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
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpPost("subject/addGroup")]
    public async Task<IActionResult> AddGroupToSubject([FromBody] GroupAddToSubjectDto toSubjectDto)
    {
        await _userService.AddGroupToSubject(toSubjectDto);
        return Ok(new { message = "Group add successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpDelete("subject/deleteTeacher")]
    public async Task<IActionResult> DeleteTeacherFromSubject([FromBody] TeacherDeleteFromSubjectDto dto)
    {
        await _userService.DeleteTeacherFromSubject(dto);
        return Ok(new { message = "Teacher deleted successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpDelete("subject/deleteGroup")]
    public async Task<IActionResult> DeleteTeacherFromSubject([FromBody] GroupAddToSubjectDto toSubjectDto)
    {
        await _userService.DeleteGroupFromSubject(toSubjectDto);
        return Ok(new { message = "Group deleted successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpDelete("subject/delete{subjectId}")]
    public async Task<IActionResult> DeleteSubject(int subjectId)
    {
        await _userService.DeleteSubject(subjectId);
        return Ok(new { message = "subject deleted successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpDelete("group/delete")]
    public async Task<IActionResult> DeleteGroup(int groupId)
    {
        await _userService.DeleteGroup(groupId);
        return Ok(new { message = "Group deleted successfully" });
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpGet("subject/list")]
    public async Task<IActionResult> AllSubjects()
    {
        var allSubjects = await _userService.GetAllSubjects();
        return Ok(allSubjects);
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpGet("subject/list/teachers")]
    public async Task<IActionResult> AllTeachers()
    {
        var allTeachers = await _userService.GetAllTeachers();
        return Ok(allTeachers);
    }
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpGet("list/groups")]
    public async Task<IActionResult> AllGroups()
    {
        var allGroups = await _userService.GetAllGroups();
        return Ok(allGroups);
    }
    
    
    // [Authorize]
    // [Authorize(Policy = "Admin")]
    [HttpGet("group/list/students")]
    public async Task<IActionResult> AllStudents()
    {
        var allStudents = await _userService.GetAllStudent();
        return Ok(allStudents);
    }
    
    // [Authorize]
    // [Authorize(Policy = "Teacher")]

    [HttpGet("teacher/subjectList")]
    public async Task<IActionResult> GetAllTeacherSubject()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
            return Unauthorized();

        List<Subject> subjects = await _userService.GetAllTeacherSubject(email);
        return Ok(subjects);
    }
    
    // [Authorize]
    // [Authorize(Policy = "Teacher")]
    [HttpGet("teacher/subject")]
    public async Task<IActionResult> GetSubjectTeacher(int subjectId)
    {
        
        SubjectShowDto subject = await _userService.GetSubjectForTeacher(subjectId);
        return Ok(subject);
    }

    [HttpPost("teacher/upload-syllabus")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UploadSyllabus([FromQuery] int subjectId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");
        await _userService.AddSyllabus(subjectId, file);
        return Ok(new { message = "Syllabus uploaded successfully." });
    }
    
    [HttpGet("user/download-syllabus")]
    [Authorize(Roles = "Teacher,Student")]
    public async Task<IActionResult> DownloadSyllabus([FromQuery] int subjectId)
    {
        try
        {
            var fileInfo = await _userService.GetSyllabus(subjectId);
            return PhysicalFile(fileInfo.FilePath, fileInfo.ContentType, fileInfo.FileName);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost("teacher/createmodule")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateModule([FromBody] CreateModuleDto module)
    {
        await _userService.CreateModule(module);
        return Ok(new { message = "Module created successfully" });
    }
    
    [HttpDelete("teacher/deletemodule")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteModule(int moduleId)
    {
        await _userService.DeleteModule(moduleId);
        return Ok(new { message = "Module deleted successfully" });
    }
    
    [HttpPut("teacher/editmodule")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> EditModule([FromBody] ModuleViewDto module)
    {
        await _userService.EditModule(module);
        return Ok(new { message = "Module edited successfully" });
    }


    [HttpPost("teacher/addFileForModule")]
    public async Task<IActionResult> UploadMaterial([FromQuery] int moduleId, string type, IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("File is empty.");
        
        await _userService.UploadMaterialFile(moduleId, type, file);
        return Ok("File uploaded");
    }
    
    
    [HttpGet("teacher/downloadMaterial")]
    [Authorize(Roles = "Teacher,Student")]
    public async Task<IActionResult> GetMaterial([FromQuery] int fileId, string fileType)
    {
        try
        {
            var fileInfo = await _userService.GetMaterial(fileId, fileType);
            return PhysicalFile(fileInfo.FilePath, fileInfo.ContentType, fileInfo.FileName);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("teacher/deleteMaterial")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> DeleteMaterial([FromQuery] int fileId, string fileType)
    {
        await _userService.DeleteMaterial(fileId, fileType);
        return Ok(new { message = "Material deleted successfully" });
    }



}