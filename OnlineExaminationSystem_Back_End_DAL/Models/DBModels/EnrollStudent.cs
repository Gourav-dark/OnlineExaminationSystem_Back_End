using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class EnrollStudent
    {
        [Key]
        public int Id { get; set; }
        //ForeignKey
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        //ForeignKey
        public Guid StudentId { get; set; }
        public User Student { get; set; }
    }
}
