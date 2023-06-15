using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int TotalMarks { get; set; }
        [Required]
        public int MarksObtained { get; set; }
        [Required]
        public float ObtainedPercentage { get; set; }
        [Required]
        [StringLength(3)]
        public string GradeObtained { get; set; }

        //ForeignKey
        public Guid StudentId { get; set; }
        public User Student { get; set; }
        //ForeignKey
        public Guid ExamId { get; set; }
        public ExamDetail ExamDetail { get; set; }

    }
}
