using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Attendance.ViewModel
{
    public class StudentCreateViewModel
    {
        public string Name { get; set; }
        public int? Age { get; set; }
        [RegularExpression(@"[a-zA-Z0-9_]+@[a-zA-Z]+.[a-zA-Z]{2,4}")]
        [Remote("CheckEmailExist", "Student", AdditionalFields = "Name")]
        public string Email { get; set; }
        public int? DeptNum { get; set; }
        public int? HrId { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        [NotMapped]
        public string CPassword { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Hrs { get; set; }
    }

}
