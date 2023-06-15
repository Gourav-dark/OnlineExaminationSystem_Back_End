using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class ViewCourse
    {
        public Guid Id { get; set; }

        public string CourseName { get; set; }

        public string DepartmentName { get; set; }

        public int InstituteId { get; set; }
    }
}
