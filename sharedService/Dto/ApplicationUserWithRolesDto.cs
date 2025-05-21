using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Shared.Models;

public class ApplicationUserWithRolesDto : ApplicationUserDto
{
    public List<string>? Roles { get; set; }
}
