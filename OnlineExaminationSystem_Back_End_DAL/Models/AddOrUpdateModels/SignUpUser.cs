namespace OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels
{
    public class SignUpUser
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public char Gender { get; set; }
        public string ImageUrl { get; set; } = "./ProfileImages/user.png";
        public string DOB { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
