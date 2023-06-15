namespace OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels
{
    public class AddExamDetail
    {
        public string ExamName { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int Duration { get; set; }

        public int NoOfQuestion { get; set; }

        public int TotalMark { get; set; }
    }
}
