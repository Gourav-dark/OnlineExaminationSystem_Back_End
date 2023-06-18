namespace OnlineExaminationSystem_Back_End_DAL.Models.ViewModels
{
    public class ViewUser
    { 
        public Guid Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public char Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly DOB { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public int InstituteId { get; set; }
        public int RoleId { get; set; }
    }
}
