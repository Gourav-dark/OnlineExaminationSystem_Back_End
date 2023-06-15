namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class View_Result
    {
        public Guid Id { get; set; }
        public int TotalMarks { get; set; }
        public int MarksObtained { get; set; }
        public float ObtainedPercentage { get; set; }
        public string GradeObtained { get; set; }

        public Guid StudentId { get; set; }
        public Guid ExamId { get; set; }
    }
}
