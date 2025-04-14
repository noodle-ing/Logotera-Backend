using GradeCom.Dtos.UserDtos;

namespace GradeCom.Services.AuthenticationServices;

public interface IAuthenticationService
{
    Task<AuthResponseDto> Register(RegisterRequestDto registerRequestDto);
    Task<AuthResponseDto> Login(LoginRequestDto loginRequestDto);
}