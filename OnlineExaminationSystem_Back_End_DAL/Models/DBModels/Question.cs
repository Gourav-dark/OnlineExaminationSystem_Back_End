using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public enum QSet
    {
        Set_1 = 1, Set_2 = 2, Set_3 = 3, Set_4 = 4
    }
    public class Question
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public QSet QuestionSet { get; set; } = QSet.Set_1;

        [Required]
        [StringLength(1500)]
        public string QuestionTitle { get; set; }

        [Required]
        [StringLength(500)]
        public string Option_A { get; set; }

        [Required]
        [StringLength(500)]
        public string Option_B { get; set; }

        [Required]
        [StringLength(500)]
        public string Option_C { get; set; }

        [Required]
        [StringLength(500)]
        public string Option_D { get; set; }

        [Required]
        [Column(TypeName = "CHAR")]
        [MaxLength(1)]
        public char CorrectOption { get; set; }

        [Required]
        public int Mark { get; set; }
        //ForeignKey
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        //ForeignKey
        public Guid ExaminerId { get; set; }
        public User Examiner { get; set; }
    }
}
