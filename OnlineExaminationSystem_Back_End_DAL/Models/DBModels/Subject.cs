using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class Subject
    {
        public Subject()
        {
            ExamDetails = new HashSet<ExamDetail>();
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string SubjectCode { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; }

        //connectiong to course table ---FK
        //ForeignKey
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }


        public virtual ICollection<ExamDetail> ExamDetails { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

    }
}
