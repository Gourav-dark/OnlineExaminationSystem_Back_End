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

    public class InstituteDetailsController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        public InstituteDetailsController(DatabaseContext context,IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        //Get list of Institute Persent in database
        [HttpGet]
        [Route("[Action]")]
        [AllowAnonymous]
        public async Task<ActionResult> InstituteList()
        {
            if (_dbContext.InstituteDetails == null)
            {
                return NotFound("Institute List not there.Ask for Institue to Create Anccount Detail");
            }
            return Ok(await _dbContext.InstituteDetails.Where(i=>i.Id!=1).Select(i => _mapper.Map<ViewInstituteDetail>(i)).ToListAsync());
        }
        //Find Id by Institute Name
        [HttpGet]
        [Route("[Action]/{name}")]
        [AllowAnonymous]
        public async Task<ActionResult> SearchInstituteName(string name)
        {
            var instituteDetail = await _dbContext.InstituteDetails.FirstOrDefaultAsync(i => i.InstituteName==name);
            if (instituteDetail == null)
            {
                return BadRequest("Institute Name Not Found");
            }
            return Ok(_mapper.Map<ViewInstituteDetail>(instituteDetail));
        }
        //Add Institute Deatils
        [HttpPost]
        [Route("[Action]")]
        //[Authorize(Roles="Admin,InstituteUser")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterInstitute(AddInstituteDetail instituteDetail)
        {
            var InstituteExist =await _dbContext.InstituteDetails.FirstOrDefaultAsync(i => i.InstituteName == instituteDetail.InstituteName);
            if(InstituteExist == null)
            {
                var insDetail = _mapper.Map<InstituteDetail>(instituteDetail);
                await _dbContext.InstituteDetails.AddAsync(insDetail);
                await _dbContext.SaveChangesAsync();
                return Ok(new { massage="Registered Successfully", Id=insDetail.Id});
            }
            return BadRequest("Institute Name Already Registered");
        }

        //Update Institute Details in Database
        [HttpPut]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Admin,InstituteUser")]
        public async Task<ActionResult> UpdateInstitute(int id,AddInstituteDetail upinstituteDetail)
        {
            var Edit_instituteDetail = await _dbContext.InstituteDetails.FindAsync(id);
            if (Edit_instituteDetail != null)
            {
                Edit_instituteDetail.InstituteName = upinstituteDetail.InstituteName;
                Edit_instituteDetail.Location = upinstituteDetail.Location;
                Edit_instituteDetail.City = upinstituteDetail.City;
                Edit_instituteDetail.PostalCode = upinstituteDetail.PostalCode;
                Edit_instituteDetail.State = upinstituteDetail.State;
                Edit_instituteDetail.Country = upinstituteDetail.Country;
                await _dbContext.SaveChangesAsync();
                return Ok("Updated Successfully");
            }
            return NotFound("Institute id not valid");
        }

        //Delete Institute Details From Database
        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(Roles = "Admin,InstituteUser")]
        public async Task<ActionResult> DeleteInstituteDetails(int id)
        {
            var delete_institute = await _dbContext.InstituteDetails.FindAsync(id);
            if (delete_institute != null)
            {
                _dbContext.Remove(delete_institute);
                await _dbContext.SaveChangesAsync();
                return Ok("Deleted Institute Details");
            }
            return NotFound("Institute Id not Valid");
        }
    }
}
