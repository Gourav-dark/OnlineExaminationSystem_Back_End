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
using OnlineExaminationSystem_Back_End_DAL.Models.ViewModels;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly DatabaseContext _dbcontext;
        private readonly IMapper _mapper;

        public QuestionsController(DatabaseContext context,IMapper mapper)
        {
            _dbcontext = context;
            _mapper = mapper;
        }

        // List of All Questioin
        [HttpGet]
        [Route("[Action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AllQuestions()
        {
            if (_dbcontext.Questions == null)
            {
                return NotFound();
            }
            return Ok(await _dbcontext.Questions.Select(q=>_mapper.Map<ViewQuestion>(q)).ToListAsync());
            
        }

        // Question By Question Id
        [HttpGet]
        [Route("[Action]/{id:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> FindQuestion(Guid id)
        {
            if (_dbcontext.Questions == null)
            {
                return NotFound();
            }
            var question = await _dbcontext.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound("Invalid Question Id");
            }
            var viewquestion = _mapper.Map<ViewQuestion>(question);
            return Ok(viewquestion);
        }
        //list of all Question in that subject using SubjectID
        [HttpGet]
        [Route("[Action]/{Sid:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> QuestionsListBySubject(Guid Sid)
        {
            if (_dbcontext.Questions == null)
            {
                return NotFound();
            }
            var questionList=await _dbcontext.Questions.Where(q=>q.SubjectId==Sid).Select(q => _mapper.Map<ViewQuestion>(q)).ToListAsync();
            if(questionList !=null)
            {
                return Ok(questionList);
            }
            return NotFound("No Question in this Subject Code");
        }
        //list of all Question in that Create by the Examiner
        [HttpGet]
        [Route("[Action]/{Eid:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> QuestionsListByExaminer(Guid Eid)
        {
            if (_dbcontext.Questions == null)
            {
                return NotFound();
            }
            var questionList = await _dbcontext.Questions.Where(q => q.ExaminerId == Eid).Select(q => _mapper.Map<ViewQuestion>(q)).ToListAsync();
            if (questionList != null)
            {
                return Ok(questionList);
            }
            return NotFound("No Question Create By this Examiner");
        }
        //Update Question
        [HttpPut]
        [Route("[Action]/{id:guid}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<IActionResult> UpdateQuestion(Guid id, AddQuestion question)
        {
            var questionexist = await _dbcontext.Questions.FindAsync(id);
            if(questionexist != null)
            {
                questionexist.QuestionSet = question.QuestionSet;
                questionexist.QuestionTitle = question.QuestionTitle;
                questionexist.Option_A = question.Option_A;
                questionexist.Option_B = question.Option_B;
                questionexist.Option_C = question.Option_C;
                questionexist.Option_D = question.Option_D;
                questionexist.CorrectOption = question.CorrectOption;
                questionexist.Mark = question.Mark;
                await _dbcontext.SaveChangesAsync();
                return Ok("Updated Successfully");
            }
            return NotFound("Invalid Question Id");
        }

        // Add Question in Database
        [HttpPost]
        [Route("[Action]")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<ActionResult> AddQuestion(Guid Sid,Guid Eid, AddQuestion question)
        {
            var addquestion = _mapper.Map<Question>(question);
            addquestion.SubjectId = Sid;
            addquestion.ExaminerId = Eid;
            _dbcontext.Questions.Add(addquestion);
            await _dbcontext.SaveChangesAsync();
            return Ok("Question Create in Database");
        }
        // DELETE 
        [HttpDelete]
        [Route("[Action]/{id}")]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            if (_dbcontext.Questions == null)
            {
                return NotFound("Empty database");
            }
            var question = await _dbcontext.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound("Question Id Not Found");
            }
            _dbcontext.Questions.Remove(question);
            await _dbcontext.SaveChangesAsync();
            return Ok("Deleted");
        }
    }
}
