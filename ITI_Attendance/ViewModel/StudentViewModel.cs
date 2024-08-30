using ITI_Attendance.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITI_Attendance.ViewModel
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<SelectListItem> Departments { get; set; }
        public IEnumerable<SelectListItem> Hrs { get; set; }
    }

}
