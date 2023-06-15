using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExaminationSystem_Back_End_DAL.DbContexts;
using OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels;
using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;

        public SubjectsController(DatabaseContext context,IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        //all subject by list
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> AllSubjectlist()
        {
            if (_dbContext.Subjects == null)
            {
                return NotFound();
            }
            return Ok(await _dbContext.Subjects.Select(s=>_mapper.Map<ViewSubject>(s)).ToListAsync());
        }
        //all subject of that course
        [HttpGet]
        [Authorize]
        [Route("[Action]/{Cid:guid}")]
        public async Task<ActionResult> subjectlistbycourse(Guid CId)
        {
            if (_dbContext.Subjects == null)
            {
                return NotFound("not Found");
            }
            var Csubjectlt=await _dbContext.Subjects.Where(s=>s.CourseId==CId).Select(s => _mapper.Map<ViewSubject>(s)).ToListAsync();
            if (Csubjectlt == null )
            {
                return NotFound("No Subject in this Course");
            }
            return Ok(Csubjectlt);
        }

        // find subject by id
        [HttpGet]
        [Route("[Action]/{id:guid}")]
        [Authorize]
        public async Task<ActionResult> FindSubject(Guid id)
        {
            if (_dbContext.Subjects == null)
            {
                return NotFound();
            }
            var subject = await _dbContext.Subjects.FindAsync(id);

            if (subject != null)
            {
                var viewsubject = _mapper.Map<ViewSubject>(subject);
                return Ok(viewsubject);
            }
            return NotFound();
        }

        // Updata Subject Details
        [HttpPut]
        [Route("[Action]/{id:guid}")]
        [Authorize(Roles ="Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> Update(Guid id, AddSubject subject)
        {
            var existsubject = await _dbContext.Subjects.FindAsync(id);
            if (existsubject != null)
            {
                existsubject.SubjectName = subject.SubjectName;
                existsubject.SubjectCode = subject.SubjectCode;
                await _dbContext.SaveChangesAsync();
                return Ok("Subject Details Updated");
            }
            return NotFound("Id is Invalid");
        }

        //add Subject using Course Id
        [HttpPost]
        [Route("[Action]/{Cid:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> AddSubject(Guid Cid,AddSubject subject)
        {
            var existsubject = await _dbContext.Subjects.FirstOrDefaultAsync(s=> s.CourseId==Cid && s.SubjectCode==subject.SubjectCode);
            if(existsubject == null)
            {
                var addsubject = _mapper.Map<Subject>(subject);
                addsubject.CourseId = Cid;
                _dbContext.Subjects.Add(addsubject);
                await _dbContext.SaveChangesAsync();
                return Ok("Subject Added");
            }
            return BadRequest("Subject Already there");
        }

        // DELETE subject
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound("Id Not Found");
            }
            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}
