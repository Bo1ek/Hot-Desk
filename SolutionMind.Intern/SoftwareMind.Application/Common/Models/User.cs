using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareMind.Application.Common.Models;

public class User : IdentityUser
{
    [PersonalData]
    [Required]
    [Column(TypeName = "text")]
    public string FirstName { get; set; } = string.Empty;
    [PersonalData]
    [Required]
    [Column(TypeName = "text")]
    public string LastName { get; set; } = string.Empty;
}
