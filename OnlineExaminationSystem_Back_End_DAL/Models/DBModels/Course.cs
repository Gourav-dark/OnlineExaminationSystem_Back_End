using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class Course
    {
        public Course()
        {
            EnrollStudents = new HashSet<EnrollStudent>();
            Subjects = new HashSet<Subject>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CourseName { get; set; }

        [StringLength(100)]
        public string ?DepartmentName { get; set; }

        //ForeignKey
        public int InstituteId { get; set; }
        public virtual InstituteDetail InstituteDetail { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<EnrollStudent> EnrollStudents { get; set; }
    }
}
