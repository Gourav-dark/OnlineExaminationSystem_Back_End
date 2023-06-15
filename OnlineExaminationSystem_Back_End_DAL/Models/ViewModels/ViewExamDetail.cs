using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class ViewExamDetail
    {
        public Guid Id { get; set; }

        public string ExamName { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int Duration { get; set; }

        public int NoOfQuestion { get; set; }

        public int TotalMark { get; set; }

        public Guid SubjectId { get; set; }
    }
}
