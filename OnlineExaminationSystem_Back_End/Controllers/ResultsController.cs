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
    public class ResultsController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public ResultsController(DatabaseContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        // Result List
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResultsList()
        {
            if (_dbContext.Results == null)
            {
                return NotFound("Empty");
            }
            return Ok(await _dbContext.Results.Select(r=>_mapper.Map<View_Result>(r)).ToListAsync());
        }

        // Get result by Result Id
        [HttpGet]
        [Authorize]
        [Route("[Action]/{id}")]
        [Authorize]
        public async Task<ActionResult> ResultById(Guid id)
        {
            if (_dbContext.Results == null)
            {
                return NotFound("Empty");
            }
            var result = await _dbContext.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound("Id Not Found");
            }
            var viewresult = _mapper.Map<View_Result>(result);
            return Ok(viewresult);
        }
        //get Result By Student Id and Exam Id
        //[HttpGet]
        //[Authorize]
        //[Route("[Action]")]
        //[Authorize]
        //public async Task<ActionResult> ResultByStudentIdandExamId(Guid sid,Guid eid)
        //{
        //    if (_dbContext.Results == null)
        //    {
        //        return NotFound("Empty Database");
        //    }
        //    var resultlist = await _dbContext.Results.FirstOrDefaultAsync(r => r.StudentId == sid && r.ExamId==eid);
        //    if(resultlist != null)
        //    {
        //        return Ok(resultlist);
        //    }
        //    return NotFound("Invalid Student Id or Exam Id");
        //}
        //get Result By Student Id
        [HttpGet]
        [Authorize]
        [Route("[Action]/{Sid:guid}")]
        [Authorize]
        public async Task<ActionResult> ResultByStudentId(Guid Sid)
        {
            if (_dbContext.Results == null)
            {
                return NotFound("Empty Database");
            }
            var resultlist = await _dbContext.Results.Where(r => r.StudentId == Sid).Select(r => _mapper.Map<View_Result>(r)).ToListAsync();
            if (resultlist != null)
            {
                return Ok(resultlist);
            }
            return NotFound("Invalid Student Id");
        }
        //get Result By Exam Id
        [HttpGet]
        [Authorize]
        [Route("[Action]/{Eid:guid}")]
        [Authorize]
        public async Task<ActionResult> ResultByExamId(Guid Eid)
        {
            if (_dbContext.Results == null)
            {
                return NotFound("Empty Database");
            }
            var resultlist = await _dbContext.Results.Where(r => r.ExamId == Eid).Select(r => _mapper.Map<View_Result>(r)).ToListAsync();
            if (resultlist != null)
            {
                return Ok(resultlist);
            }
            return NotFound("Invalid Exam Id");
        }
        //Add Result in database this will system generated
        [HttpPost]
        [Authorize]
        [Route("[Action]")]
        public async Task<ActionResult> AddResult(Guid sId,Guid eId,AddResult result)
        {
            var addResult = _mapper.Map<Result>(result);
            addResult.StudentId = sId;
            addResult.ExamId = eId;
            await _dbContext.Results.AddAsync(addResult);
            _dbContext.SaveChanges();
            return Ok(addResult.Id);
        }

        // DELETE Result from Database this no use
        [HttpDelete]
        [Authorize(Roles ="Admin,InstituteUser,Examiner")]
        [Route("[Action]/{id}")]
        public async Task<ActionResult> DeleteResult(Guid id)
        {
            if (_dbContext.Results == null)
            {
                return NoContent();
            }
            var result = await _dbContext.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound("Id Not Found");
            }

            _dbContext.Results.Remove(result);
            await _dbContext.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
