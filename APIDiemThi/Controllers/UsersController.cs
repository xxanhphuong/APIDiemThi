using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIDiemThi.Models;
using APIDiemThi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using APIDiemThi.Models.Dtos.UserDto;
using APIDiemThi.Helpers;
using Newtonsoft.Json;

namespace APIDiemThi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            var objDto = _mapper.Map<UserLoginDto>(user);
            return Ok(objDto);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }
            var user = _userRepo.Register(model.Username, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }

        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(List<UserViewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList = _userRepo.GetUsers(kw, ownerParameters);
            var metadata = new
            {
                objList.TotalCount,
                objList.PageSize,
                objList.CurrentPage,
                objList.TotalPages,
                objList.HasNext,
                objList.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var objDto = new List<UserViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<UserViewDto>(obj));
            }

            return Ok(objDto);
        }


        [HttpGet("{userId:int}", Name = "GetUser")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(UserViewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetUser(int userId)
        {
            var obj = _userRepo.GetUser(userId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<UserViewDto>(obj);
            return Ok(objDto);
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(201, Type = typeof(UserCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (userCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_userRepo.UserExists(userCreateDto.Username))
            {
                ModelState.AddModelError("", "User Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var userObj = _mapper.Map<Users>(userCreateDto);
            if (!_userRepo.CreateUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {userObj.Username}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetUser", new { userId = userObj.Id }, userObj);
        }


        [HttpPatch("{userId:int}", Name = "UpdateUser")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null || userId != userUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_userRepo.UserExists(userUpdateDto.Id))
            {
                return NotFound();
            }
            var userObj = _mapper.Map<Users>(userUpdateDto);
            if (!_userRepo.UpdateUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {userObj.Username}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId:int}", Name = "DeleteMajor")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMajor(int userId)
        {
            if (!_userRepo.UserExists(userId))
            {
                return NotFound();
            }

            var userObj = _userRepo.GetUser(userId);
            if (!_userRepo.DeleteUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {userObj.Username }");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
