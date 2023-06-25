using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class EnrollStudentsController : ControllerBase
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;

        public EnrollStudentsController(DatabaseContext context,IMapper mapper)
        {
            _dbcontext = context;
            _mapper=mapper;
        }

        //list of all enrolled student
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[Action]")]
        public async Task<ActionResult> AllEnrollStudents()
        {
            if (_dbcontext.EnrollStudents == null)
            {
                return NotFound("Database is Empty.");
            }
            return Ok(await _dbcontext.EnrollStudents.Select(e=>_mapper.Map<ViewEnrollStudent>(e)).ToListAsync());
        }

        // list of the all Enroll Student of that course
        [HttpGet]
        [Route("[Action]/{Cid:guid}")]
        [Authorize]
        public async Task<ActionResult> EnrollStudentsByCourse(Guid Cid)
        {
            if (_dbcontext.EnrollStudents == null)
            {
                return NotFound();
            }
            var enrollStudentslist = await _dbcontext.EnrollStudents.Where(e=>e.CourseId==Cid).Select(e => _mapper.Map<ViewEnrollStudent>(e)).ToListAsync();
            if (enrollStudentslist == null)
            {
                return NotFound("Course not Enrolled");
            }
            return Ok(enrollStudentslist) ;
        }
        // Course name by student Id
        [HttpGet]
        [Route("[Action]/{Sid:guid}")]
        [Authorize]
        public async Task<ActionResult> CourseIdByStudentId(Guid Sid)
        {
            if (_dbcontext.EnrollStudents == null)
            {
                return NotFound("Database Empty");
            }
            var enrollStudent = await _dbcontext.EnrollStudents.FirstOrDefaultAsync(e => e.StudentId == Sid);

            if (enrollStudent == null)
            {
                return NotFound("Student Id Not Found");
            }

            return Ok(enrollStudent.CourseId);
        }
        // Add Enroll Student in Database
        [HttpPost]
        [Route("[Action]")]
        [Authorize]
        public async Task<ActionResult> AddEnrollStudent(AddEnrollStudent enrollStudent)
        {
            var student = await _dbcontext.Users.FindAsync(enrollStudent.StudentId);
            var course = await _dbcontext.Courses.FindAsync(enrollStudent.CourseId);
        
            if(student != null && course != null)
            {
                var existStudent=await _dbcontext.EnrollStudents.FirstOrDefaultAsync(e=>e.StudentId==enrollStudent.StudentId);
                if(existStudent == null)
                {
                    var addEnrollStudent = _mapper.Map<EnrollStudent>(enrollStudent);
                    _dbcontext.EnrollStudents.Add(addEnrollStudent);
                    await _dbcontext.SaveChangesAsync();
                    return Ok("Student Enrolled Course");
                }
                return BadRequest("Student Already Enroll a Course");
            }
            return NotFound("Either Course Id or Student Id  Not correct");
        }

        [HttpDelete]
        [Route("[Action]/{Sid}")]
        [Authorize]
        public async Task<IActionResult> DeleteEnrollStudent(Guid Sid)
        {
            if (_dbcontext.EnrollStudents == null)
            {
                return NotFound();
            }
            var enrollStudent = await _dbcontext.EnrollStudents.FirstOrDefaultAsync(e=> e.StudentId==Sid);
            if (enrollStudent == null)
            {
                return NotFound("Student Not Exist");
            }

            _dbcontext.EnrollStudents.Remove(enrollStudent);
            await _dbcontext.SaveChangesAsync();
            return Ok("UnEnrolled Course");
        }
    }
}
