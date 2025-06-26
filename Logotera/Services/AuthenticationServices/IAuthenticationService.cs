using Logotera.Dtos.UserDtos;

namespace Logotera.Services.AuthenticationServices;

public interface IAuthenticationService
{
    Task<AuthResponseDto> Register(RegisterRequestDto registerRequestDto);
    Task<AuthResponseDto> Login(LoginRequestDto loginRequestDto);
}