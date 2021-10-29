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
using APIDiemThi.Models.Dtos.TeacherDto;

namespace APIDiemThi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ITeacherRepository _teacher;
        private readonly IStudentRepository _student;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepo, IMapper mapper, ITeacherRepository teacher, IStudentRepository student)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _teacher = teacher;
            _student = student;
        }

        /// <summary>
        /// Đăng nhập user - Không cần role
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
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

        //[AllowAnonymous]
        //[HttpPost("register")]
        //public IActionResult Register([FromBody] AuthenticationModel model)
        //{
        //    bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username);
        //    if (!ifUserNameUnique)
        //    {
        //        return BadRequest(new { message = "Username already exists" });
        //    }
        //    var user = _userRepo.Register(model.Username, model.Password);

        //    if (user == null)
        //    {
        //        return BadRequest(new { message = "Error while registering" });
        //    }

        //    return Ok();
        //}

        /// <summary>
        /// Nhận danh sách người dùng - role = admin
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên người dùng </param>
        /// <param name="ownerParameters"> Nhập từ khoá để tìm kiếm tên người dùng </param>
        /// <remarks>
        /// Chú thích:
        ///
        ///     
        ///     {
        ///        "PageNumber": "Số trang cần xem",
        ///        "PageSize": "Số lượt đối tượng trong 1 trang"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Trả về danh sách người dùng</response>
        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200, Type = typeof(List<UserViewDto>))]
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

        /// <summary>
        /// Xem thông tin chi tiết người dùng - cần có 1 trong 3 phân quyền
        /// </summary>
        /// <param name="userId"> Nhập Id sinh viên để xem thông tin chi tiết người dùng </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết người dùng</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response>
        [HttpGet("{userId:int}", Name = "GetUser")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(UserViewDto))]
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

        /// <summary>
        /// Tạo người dùng - role = Admin, khi tạo thì role chỉ được trong 3 loại(admin, teacher, student)
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Trả về tạo thành công</response> 
        /// <response code="404">Trả về nếu không tạo được</response> 
        /// <response code="500">Trả về nếu không tạo được</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(201, Type = typeof(UserCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
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
            if (!await _userRepo.CreateUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {userObj.Username}");
                return StatusCode(500, ModelState);
            }

            if (userCreateDto.Role == "teacher")
            {
                int idUser = await _userRepo.GetIdUser(userCreateDto.Username);
                Teacher tea = new Teacher();
                tea.TeacherId = idUser;
                if (!await _teacher.CreateTeacher(tea))
                {
                    ModelState.AddModelError("", $"Something went wrong when creat Teacher");
                    return StatusCode(500, ModelState);
                }
            }

            if (userCreateDto.Role == "student")
            {
                int idUser = await _userRepo.GetIdUser(userCreateDto.Username);
                Student stu = new Student();
                stu.StudentId = idUser;
                if (!await _student.CreateStudent(stu))
                {
                    ModelState.AddModelError("", $"Something went wrong when creat Student");
                    return StatusCode(500, ModelState);
                }
            }

            return CreatedAtRoute("GetUser", new { userId = userObj.Id }, userObj);
        }

        /// <summary>
        /// Chỉnh sửa user - role = Admin
        /// </summary>
        /// <param name="userId"> Nhập Id để sửa user </param>
        /// <param name="userUpdateDto"> Nhập từ khoá để tìm kiếm user </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{userId:int}", Name = "UpdateUser")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserUpdateDto userUpdateDto)
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
            if (!await _userRepo.UpdateUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {userObj.Username}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá user - role = Admin
        /// </summary>
        /// <param name="userId"> Nhập Id để xoá user </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{userId:int}", Name = "DeleteUser")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMajor(int userId)
        {
            if (!_userRepo.UserExists(userId))
            {
                return NotFound();
            }

            var userObj = _userRepo.GetUser(userId);
            if (!await _userRepo.DeleteUser(userObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {userObj.Username }");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
