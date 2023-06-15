using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;

namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class ViewQuestion
    {
        public Guid Id { get; set; }

        public QSet QuestionSet { get; set; }

        public string QuestionTitle { get; set; }

        public string Option_A { get; set; }

        public string Option_B { get; set; }

        public string Option_C { get; set; }

        public string Option_D { get; set; }

        public char CorrectOption { get; set; }

        public int Mark { get; set; }

        public Guid SubjectId { get; set; }

        public Guid ExaminerId { get; set; }
    }
}
