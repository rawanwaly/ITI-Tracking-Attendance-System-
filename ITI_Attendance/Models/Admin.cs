using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ITI_Attendance.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
        [RegularExpression(@"[a-zA-Z0-9_]+@[a-zA-Z]+.[a-zA-Z]{2,4}")]
        public string Email { get; set; }

    }
}
