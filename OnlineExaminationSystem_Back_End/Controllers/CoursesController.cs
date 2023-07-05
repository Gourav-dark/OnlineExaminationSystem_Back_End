using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExaminationSystem_Back_End_DAL.Data;
using OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels;
using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;
using OnlineExaminationSystem_Back_End_DAL.Models.ViewModels;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        public CoursesController(DatabaseContext context,IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        //list of course in database
        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> AllCourseList()
        {
            if (_dbContext.Courses == null)
            {
                return NotFound("Database is Empty");
            }
            return Ok(await _dbContext.Courses.Select(c => _mapper.Map<ViewCourse>(c)).ToListAsync());
        }
        //course by the id
        [HttpGet("[action]/{id}")]
        [Authorize]
        public async Task<ActionResult> FindCourse(Guid id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            if (course != null)
            {
                var viewcourse = _mapper.Map<ViewCourse>(course);
                return Ok(viewcourse);
            }
            return BadRequest("Id not Found");
        }
        //course list persent in that institute
        [HttpGet("[action]/{Iid}")]
        [AllowAnonymous]
        public async Task<ActionResult> InstituteCourses(int Iid)
        {
            var courselist=await _dbContext.Courses.Where(c=>c.InstituteId==Iid).ToListAsync();
            if(courselist.Count > 0)
            {
                var viewlist = courselist.Select(c => _mapper.Map<ViewCourse>(c));
                return Ok(viewlist);
            }
            return BadRequest("Institute does not have any courses.");
        }
        //Add course in database
        [HttpPost]
        [Route("[Action]/{Iid}")]
        [Authorize(Roles = "Admin,InstituteUser")]
        public async Task<ActionResult> AddCourse(int Iid,AddCourse course)
        {
            var existcourse = await _dbContext.Courses.FirstOrDefaultAsync(c => c.InstituteId == Iid && c.CourseName.Contains(course.CourseName) && c.DepartmentName.Contains(course.DepartmentName));
            if(existcourse == null)
            {
                var addcourse =_mapper.Map<Course>(course);
                addcourse.InstituteId = Iid;
                await _dbContext.AddRangeAsync(addcourse);
                await _dbContext.SaveChangesAsync();
                return Ok("Course Registered");
            }
            return BadRequest("Course Already Available");
        }

        //Update Course Details
        [HttpPut("[action]/{id:guid}")]
        [Authorize(Roles = "Admin,InstituteUser")]
        public async Task<ActionResult> Update(Guid id,AddCourse course)
        {
            var upcourse = await _dbContext.Courses.FindAsync(id);
            if(upcourse != null)
            {
                upcourse.CourseName = course.CourseName;
                upcourse.DepartmentName = course.DepartmentName;
                await _dbContext.SaveChangesAsync();
                return Ok("Updated Successfully");
            }
            return NotFound("Id is Invalid");
        }

        // DELETE Course in database
        [HttpDelete("[action]/{id:guid}")]
        [Authorize(Roles = "Admin,InstituteUser")]
        public async Task<ActionResult> Delete([FromRoute]Guid id)
        {
            var existcourse = await _dbContext.Courses.FindAsync(id);
            if(existcourse != null)
            {
                _dbContext.Courses.Remove(existcourse);
                await _dbContext.SaveChangesAsync();
                return Ok("Course Deleted in Database");
            }
            return NotFound("Id not Found");
        }
    }
}
