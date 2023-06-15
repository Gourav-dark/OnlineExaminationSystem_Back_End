using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
