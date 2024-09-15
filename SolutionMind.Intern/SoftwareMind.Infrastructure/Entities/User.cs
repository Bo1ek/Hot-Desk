using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareMind.Infrastructure.Entities;

public class User : IdentityUser 
{
    [PersonalData]
    [Column(TypeName = "text")]
    public required string FirstName { get; set; }
    [PersonalData]
    [Column(TypeName = "text")]
    public required string LastName { get; set; }
}
