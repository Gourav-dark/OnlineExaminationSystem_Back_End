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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamDetailsController : ControllerBase
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;

        public ExamDetailsController(DatabaseContext context,IMapper mapper)
        {
            _dbcontext = context;
            _mapper = mapper;
        }

        // All ExamDetails list
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AllExamDetailsList()
        {
            if (_dbcontext.ExamDetails == null)
            {
                return NotFound();
            }
            return Ok(await _dbcontext.ExamDetails.Select(e=>_mapper.Map<ViewExamDetail>(e)).ToListAsync());
        }
        //Exam list by Subject Id
        [HttpGet]
        [Route("[Action]/{Sid:guid}")]
        [Authorize]
        public async Task<ActionResult> ExamDetailsListBySubject(Guid Sid)
        {
            if (_dbcontext.ExamDetails == null)
            {
                return NotFound();
            }
            var subject = await _dbcontext.Subjects.FindAsync(Sid);
            if (subject != null)
            {
                return Ok(await _dbcontext.ExamDetails.Where(e=>e.SubjectId==Sid).Select(e => _mapper.Map<ViewExamDetail>(e)).ToListAsync());
            }
            return NotFound("Invalid Subject Id");
        }

        // get Exam By Id
        [HttpGet]
        [Route("[Action]/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> ExamDetail(Guid id)
        {
            if (_dbcontext.ExamDetails == null)
            {
                return NotFound();
            }
            var examDetail = await _dbcontext.ExamDetails.FindAsync(id);

            if (examDetail == null)
            {
                return NotFound();
            }
            var viewexam = _mapper.Map<ViewExamDetail>(examDetail);
            return Ok(viewexam);
        }
        //Update Exam Detail
        [HttpPut]
        [Route("[Action]/{id:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<IActionResult> UpdateExamDetail(Guid id, AddExamDetail examDetail)
        {
            var ExamDetailexist = await _dbcontext.ExamDetails.FindAsync(id);
            if(ExamDetailexist != null)
            {
                ExamDetailexist.ExamName = examDetail.ExamName;
                ExamDetailexist.Date = DateTime.ParseExact(examDetail.Date, "dd-MM-yyyy", null);
                ExamDetailexist.Time = examDetail.Time;
                ExamDetailexist.Duration = examDetail.Duration;
                ExamDetailexist.NoOfQuestion = examDetail.NoOfQuestion;
                ExamDetailexist.TotalMark = examDetail.TotalMark;
                await _dbcontext.SaveChangesAsync();
                return Ok("Updated Successfully");
            }
            return NotFound("Invalid Exam Id");
        }

        //Add ExamDetails by Subject Id
        [HttpPost]
        [Route("[Action]/{Sid:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> AddExamDetail(Guid Sid,AddExamDetail examDetail)
        {
            var subject = await _dbcontext.Subjects.FindAsync(Sid);
            if(subject != null)
            {
                var addexamDetail = _mapper.Map<ExamDetail>(examDetail);
                addexamDetail.SubjectId = Sid;
                _dbcontext.ExamDetails.Add(addexamDetail);
                await _dbcontext.SaveChangesAsync();
                return Ok("Exam Details Entered");
            }
            return NotFound("Subject not Found");
        }
        // DELETE
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<IActionResult> DeleteExamDetail(Guid id)
        {
            if (_dbcontext.ExamDetails == null)
            {
                return NotFound();
            }
            var examDetail = await _dbcontext.ExamDetails.FindAsync(id);
            if (examDetail == null)
            {
                return BadRequest("Id Not Found");
            }

            _dbcontext.ExamDetails.Remove(examDetail);
            await _dbcontext.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
