using System.ComponentModel.DataAnnotations;
namespace AuthenticationApi.Application.DTOs


{
    public record AppUserDTO(
    int Id,
    string Name,
     string TelephoneNumber,
     string Address,
    [ EmailAddress] string Email,
    [Required] string Password,
    string Role
);
}

