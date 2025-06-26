using FirstLight.Dtos.UserDtos;

namespace FirstLight.Services.AuthenticationServices;

public interface IAuthenticationService
{
    Task<AuthResponseDto> Register(RegisterRequestDto registerRequestDto);
    Task<AuthResponseDto> Login(LoginRequestDto loginRequestDto);
}