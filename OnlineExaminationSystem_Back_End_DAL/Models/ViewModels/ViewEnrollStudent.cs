namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class ViewEnrollStudent
    {
        public int Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
    }
}
