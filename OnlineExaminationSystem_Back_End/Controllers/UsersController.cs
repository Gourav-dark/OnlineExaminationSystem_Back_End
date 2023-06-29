using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineExaminationSystem_Back_End_DAL.Data;
using OnlineExaminationSystem_Back_End_DAL.Models.DBModels;
using OnlineExaminationSystem_Back_End_DAL.Models.ViewModels;
using OnlineExaminationSystem_Back_End_DAL.Models.AddOrUpdateModels;
using OnlineExaminationSystem_Back_End_DAL.Contains.Functions;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace OnlineExaminationSystem_Back_End_DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public UsersController(DatabaseContext context,IConfiguration config, IMapper mapper)
        {
            _dbContext = context;
            _config = config;
            _mapper = mapper;
        }
        //Get list of user form database
        [HttpGet]
        [Authorize(Roles ="Admin")]
        [Route("[Action]")]
        public async Task<ActionResult>  Userlist()
        {
            if (_dbContext.Users == null)
            {
                return NotFound("Database Is Empty");
            }
            return Ok(await _dbContext.Users.Select(user => _mapper.Map<ViewUser>(user)).ToListAsync());
        }
        //Get list of user form database Using Institute ID
        [HttpGet]
        [Authorize(Roles = "Admin,InstituteUser,Examiner")]
        [Route("[Action]/{Iid}")]
        public async Task<ActionResult> UserlistByInstituteId(int Iid)
        {
            if (_dbContext.Users == null)
            {
                return NotFound("Database Is Empty");
            }
            if(await _dbContext.InstituteDetails.FindAsync(Iid)!=null)
            {
                return Ok(await _dbContext.Users.Where(u=>u.InstituteId==Iid).Select(user => _mapper.Map<ViewUser>(user)).ToListAsync());
            }
            return BadRequest("Id Not Found");
        }
        //Get Details of user by Id
        [HttpGet]
        [Authorize]
        [Route("[action]/{id:guid}")]
        public async Task<ActionResult> UserFind(  Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                var viewuser = _mapper.Map<ViewUser>(user);
                return Ok(viewuser);
            }
            return NotFound("User is Invalid");
        }
        //Add User this is for normal who have Institute Details is already there in database
        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<ActionResult> SignUp(int roleId,int instituteId, SignUpUser adduser)
        {
            var Existuser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adduser.Email);
            if (Existuser == null)
            {
                var role = await _dbContext.Roles.FindAsync(roleId);
                var institute = await _dbContext.InstituteDetails.FindAsync(instituteId);
                if (role !=null && institute !=null)
                {
                    if(adduser.Password==adduser.ConfirmPassword)
                    {
                        var user = _mapper.Map<User>(adduser);
                        user.RoleId= roleId;
                        user.InstituteId=instituteId;
                        if(role.RoleName.Contains("Admin") || role.RoleName.Contains("InstituteUser"))
                        {
                            user.Status = true;
                        }
                        await _dbContext.Users.AddAsync(user);
                        await _dbContext.SaveChangesAsync();
                        return Ok(new { Message = "Registered Successfully", userId = user.Id });
                    }
                    else
                    {
                        return BadRequest("Password and Confirm Password Not Match!");
                    }
                }
                else
                {
                    return NotFound("Role Not Exist and Institute");
                }
            }
            return BadRequest("Email Id Already Exist Try Another Email Id");
        }
        
        //Update users in Database
        [HttpPut]
        [Authorize]
        [Route("[action]/{id:guid}")]
        public async Task<ActionResult> Update(Guid id,SignUpUser upuser)
        {
            var Existuser = await _dbContext.Users.FindAsync(id);
            if (Existuser != null)
            {
                if (CommanFunctions.VerifyPassword(upuser.Password,Existuser.Password))
                {
                    Existuser.Fname = upuser.Fname;
                    Existuser.Lname = upuser.Lname;
                    Existuser.Gender = upuser.Gender;
                    Existuser.DOB = DateTime.ParseExact(upuser.DOB,"yyyy-MM-dd",null);
                    Existuser.Email = upuser.Email;
                    Existuser.PhoneNumber = upuser.PhoneNumber;
                    Existuser.Password = CommanFunctions.EncriptPassword(upuser.Password);
                    await _dbContext.SaveChangesAsync();
                    return Ok("Updated Successfully");
                }
                return BadRequest("Current Password Not Correct!");
            }
            return NotFound("Invalid User Id");
        }

        //Delete user From Database
        [HttpDelete]
        [Authorize]
        [Route("[Action]/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var delete_User = await _dbContext.Users.FindAsync(id);
            if (delete_User != null)
            {
                _dbContext.Remove(delete_User);
                await _dbContext.SaveChangesAsync();
                return Ok("Account is Deleted");
            }
            return NotFound("User_Id Not Found");
        }
        //Update Profile Image
        [HttpPut]
        [Route("[action]/{Id:guid}")]
        [Authorize]
        public async Task<ActionResult> UpdateProfilePic(Guid Id,[FromBody] string ImageUrl)
        {
            var user = await _dbContext.Users.FindAsync(Id);
            if (user != null)
            {
                user.ImageUrl = ImageUrl;
                await _dbContext.SaveChangesAsync();
                return Ok("Profile Pic Update Updated");
            }
            return NotFound("User Id Not Correct");
        }

        //UpdatePassword
        //[HttpPut]
        //[Route("[action]/{Id}")]
        //[Authorize]
        //public async Task<ActionResult> ForgetPassword(Guid Id, UpdatePassword PrivateDetail)
        //{
        //    var user=await _dbContext.Users.FindAsync(Id);
        //    if(user != null)
        //    {
        //        if (CommanFunctions.VerifyPassword(PrivateDetail.CurrentPassword, user.Password))
        //        {
        //            user.Password = CommanFunctions.EncriptPassword(PrivateDetail.NewPassword);
        //            await _dbContext.SaveChangesAsync();
        //            return Ok("Password Updated");
        //        }
        //        return BadRequest("Current Password Not Match");
        //    }
        //    return NotFound("User Id Not Correct");
        //}

        //Get user By Email id -----Login------------------------
        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login loginuser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == loginuser.Email);
            if (user != null)
            {
                if (CommanFunctions.VerifyPassword(loginuser.Password, user.Password))
                {
                    var role = await _dbContext.Roles.FindAsync(user.RoleId);
                    string jwt = JwtTokenCreate(user.Email, role.RoleName,user.Id.ToString(),user.InstituteId);
                    return Ok(new { Message = "Login Successfully", Token = jwt });
                }
                return BadRequest(new { Message="Incorrect Password" });
            }
            return NotFound(new { Message = "Email Id is Not Found" });
        }
        //JWT with Role Base Token Generate--------------------
        private string JwtTokenCreate(string email,string role,string userId,int Iid)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                    //new Claim(ClaimTypes.Email,email),
                    new Claim("Email",email),
                    //new  Claim("Image",image),
                    new Claim("Role",role),
                    new Claim("UserId",userId),
                    new Claim("Iid",Iid.ToString()),
                    new Claim(ClaimTypes.Role,role),
            };
            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(180),
                signingCredentials: credential
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        //this Only for Student and Examiner
        [HttpPut]
        [Route("[Action]/{id:guid}")]
        [Authorize(Roles ="Admin,InstituteUser")]
        public async Task<ActionResult> ActivatedStatus(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound("User Id Not Valid");
            }
            user.Status = true;
            await _dbContext.SaveChangesAsync();
            var viewuser=_mapper.Map<ViewUser>(user);
            return Ok(viewuser);
        }
    }
}
