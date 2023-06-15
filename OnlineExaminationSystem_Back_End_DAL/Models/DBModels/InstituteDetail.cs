using System.ComponentModel.DataAnnotations;

namespace OnlineExaminationSystem_Back_End_DAL.Models.DBModels
{
    public class InstituteDetail
    {
        public InstituteDetail()
        {
            Users = new HashSet<User>();
            Courses = new HashSet<Course>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string InstituteName { get; set; }

        [Required]
        [StringLength(75)]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
