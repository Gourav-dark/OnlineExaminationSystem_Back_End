using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExaminationSystem_Back_End_DAL.DbContexts;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        private readonly DatabaseContext _dbContext;
        public RolesController(DatabaseContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        [Route("[action]/{rolename}")]
        [AllowAnonymous]
        public async Task<IActionResult> RoleName(string rolename)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName.Contains(rolename));
            if(role != null) 
            {
                return Ok(role.Id);
            }
            return BadRequest("Role Name Not Exist.");
        }
        [HttpGet]
        [Route("[action]/{roleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> RoleId(int roleId)
        {
            var role = await _dbContext.Roles.FindAsync(roleId);
            if (role != null)
            {
                return Ok(role.RoleName);
            }
            return BadRequest("Role Id Not Exist.");
        }
    }
}
