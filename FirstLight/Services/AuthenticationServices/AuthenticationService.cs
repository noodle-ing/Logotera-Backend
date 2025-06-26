using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirstLight.Dtos.UserDtos;
using FirstLight.Models;
using FirstLight.Utilits;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FirstLight.Services.AuthenticationServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly StudentAuthenticationService _studentService;
    private readonly TeacherAuthenticationService _teacherService;

    public AuthenticationService(UserManager<User> userManager, IConfiguration configuration, StudentAuthenticationService studentService, TeacherAuthenticationService teacherService)
    {
        _userManager = userManager;
        _configuration = configuration;
        _studentService = studentService;
        _teacherService = teacherService;
    }

    public async Task<AuthResponseDto> Register(RegisterRequestDto requestDto)
    {
        var userByEmail = await _userManager.FindByEmailAsync(requestDto.Email);

        if (userByEmail is not null)
            throw new ArgumentException(
                $"Пользователь с {requestDto.Email} уже существует.");
        
        User user = new()
        {
            Email = requestDto.Email,
            Surname = requestDto.Surname,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = requestDto.Name
        };

        var result = await _userManager.CreateAsync(user, requestDto.Password);

        if(!result.Succeeded)
            throw new ArgumentException(
                $"Невозможно зарегистрировать пользователя {requestDto.Email}," +
                $"ошибка: {GetErrorsText(result.Errors)}");

        result = await _userManager.AddToRoleAsync(user, requestDto.Role);

        if (!result.Succeeded)
            throw new ArgumentException($"Невозможно добавить пользователя {user} в роль Admin" +
                                        $"ошибка: {GetErrorsText(result.Errors)}");
        if (requestDto.Role == "Student")
            await _studentService.CreateStudentProfileAsync(user);
        else if (requestDto.Role == "Teacher")
            await _teacherService.CreateTeacherProfileAsync(user);
        return await Login(new LoginRequestDto { Email = requestDto.Email, 
            Password = requestDto.Password });
    }

    public async Task<AuthResponseDto> Login(LoginRequestDto request)
    {
        var user = await _userManager
            .FindByEmailAsync(request.Email);
    
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw new ArgumentException(
                $"Невозможно аутентифицировать пользователя {request.Email}");
        
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
    
        var usersRoles = await _userManager.GetRolesAsync(user);
    
        authClaims.AddRange(usersRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        var token = GetToken(authClaims);

        return new AuthResponseDto { token = new JwtSecurityTokenHandler()
            .WriteToken(token), UserDto = UserMapper.UserUserDto(user), UserRole = usersRoles.FirstOrDefault()};
    }

    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
    
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey,
                SecurityAlgorithms.HmacSha256));
    
        return token;
    }
    
    public static string GetErrorsText(IEnumerable<IdentityError> errors)
        => string.Join(", ", errors.Select(error => error.Description).ToArray());
}