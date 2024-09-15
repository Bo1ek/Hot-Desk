﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftwareMind.Infrastructure.Entities;

public class User : IdentityUser
{
    [PersonalData]
    [Required]
    [Column(TypeName = "text")]
    public string FirstName { get; set; }
    [PersonalData]
    [Required]
    [Column(TypeName = "text")]
    public string LastName { get; set; }
}
