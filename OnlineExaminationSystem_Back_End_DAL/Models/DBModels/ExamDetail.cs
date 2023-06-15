using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class ExamDetail
    {
        public ExamDetail()
        {
            Results = new HashSet<Result>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ExamName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(5)]//13:30 this means 01:30PM
        public string Time { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int NoOfQuestion { get; set; }

        [Required]
        public int TotalMark { get; set; }

        //ForeignKey
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public virtual ICollection<Result> Results { get; set; }
    }
}
