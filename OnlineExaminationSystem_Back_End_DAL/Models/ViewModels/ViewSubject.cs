using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class ViewSubject
    {
        public Guid Id { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public Guid CourseId { get; set; }
    }
}
