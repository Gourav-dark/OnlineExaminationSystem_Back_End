using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;

namespace OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels
{
    public class AddQuestion
    {
        public QSet QuestionSet { get; set; } = QSet.Set_1;

        public string QuestionTitle { get; set; }

        public string Option_A { get; set; }

        public string Option_B { get; set; }

        public string Option_C { get; set; }

        public string Option_D { get; set; }

        public char CorrectOption { get; set; }

        public int Mark { get; set; }
    }
}