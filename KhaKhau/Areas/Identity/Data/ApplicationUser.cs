using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace KhaKhau.Areas.Identity.Data;
public class ApplicationUser: IdentityUser
{
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string FirstName { get; set; } 
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string LastName { get; set; }  
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string PhoneNumber { get; set; }  
    [PersonalData]
    [Column(TypeName ="nvarchar(100)")]
    public string Address { get; set; }

}
