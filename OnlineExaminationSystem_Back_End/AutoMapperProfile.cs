using AutoMapper;
using OnlineExaminationSystem_Back_End_DAL.Contains.Functions;
using OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels;
using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;
using OnlineExaminationSystem_Back_End_DAL.Models.ViewModels;

namespace OnlineExaminationSystem_Back_End_DAL
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            //1.Institute Detail Mapper
            CreateMap<InstituteDetail,ViewInstituteDetail>();
            CreateMap<AddInstituteDetail, InstituteDetail>();

            //2.User Mapper
            CreateMap<User, ViewUser>()
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DOB)));
            CreateMap<SignUpUser, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => CommanFunctions.EncriptPassword(src.Password)))
                .ForMember(dest => dest.DOB, opt => opt.MapFrom(src =>DateTime.ParseExact(src.DOB, "dd-MM-yyyy", null)));

            //3.Course
            CreateMap<Course,ViewCourse>();
            CreateMap<AddCourse,Course>();

            //4.Enroll Student
            CreateMap<EnrollStudent,ViewEnrollStudent>();
            CreateMap<AddEnrollStudent,EnrollStudent>();

            //5.Subject
            CreateMap<Subject,ViewSubject>();
            CreateMap<AddSubject,Subject>();

            //6.Question
            CreateMap<Question,ViewQuestion>();
            CreateMap<AddQuestion,Question>();

            //7.Exam Details
            CreateMap<ExamDetail, ViewExamDetail>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString()));
            CreateMap<AddExamDetail,ExamDetail>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.ParseExact(src.Date, "dd-MM-yyyy", null)));

            //8.Result
            CreateMap<Result,View_Result>();
            CreateMap<AddResult,Result>();
        }
    }
}
