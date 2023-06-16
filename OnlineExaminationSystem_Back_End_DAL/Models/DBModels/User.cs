using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class User
    {
        public User()
        {
            Questions= new HashSet<Question>();
            Results=new HashSet<Result>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Fname { get; set; }

        [Required]
        [StringLength(50)]
        public string Lname { get; set; }

        [Required]
        [Column(TypeName = "CHAR")]
        [MaxLength(1)]
        public char Gender { get; set; }

        //Image of Users
        public string ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Status { get; set; }=false;

        //Connecting to Institute Details Tables
        public int InstituteId { get; set; }
        public virtual InstituteDetail InstituteDetail { get; set; }

        //Connecting to Institute Details Tables
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        //Create Forgrn key in Child Table 
        public virtual EnrollStudent EnrollStudent { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
