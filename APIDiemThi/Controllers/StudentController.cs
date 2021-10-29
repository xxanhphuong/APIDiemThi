using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.StudentDto;
using APIDiemThi.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDiemThi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _student;
        private readonly IMapper _mapper;
        public StudentController(IStudentRepository student, IMapper mapper)
        {
            _student = student;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách sinh viên - Không cần role
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên sinh viên </param>
        /// <param name="ownerParameters"> Nhập từ khoá để tìm kiếm tên sinh viên </param>
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
        /// <response code="200">Trả về danh sách sinh viên</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<StudentViewDto>))]
        public IActionResult GetStudents([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList = _student.GetStudents(kw, ownerParameters);
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

            var objDto = new List<StudentViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<StudentViewDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Xem thông tin chi tiết sinh viên - Không cần role
        /// </summary>
        /// <param name="studentId"> Nhập Id sinh viên để xem thông tin chi tiết sinh viên </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết sinh viên</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response> 
        [HttpGet("{studentId:int}", Name = "GetStudent")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(StudentViewDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetStudent(int studentId)
        {
            var obj = _student.GetStudent(studentId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<StudentViewDto>(obj);
            return Ok(objDto);
        }


        //[HttpPost]
        //[ProducesResponseType(201, Type = typeof(StudentCreateDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentCreateDto)
        //{
        //    if (studentCreateDto == null)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (_student.StudentExists(studentCreateDto.StudentId))
        //    {
        //        ModelState.AddModelError("", "Student Exists");
        //        return StatusCode(404, ModelState);
        //    }

        //    /*if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }*/

        //    var studentObj = _mapper.Map<Student>(studentCreateDto);
        //    if (!await _student.CreateStudent(studentObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when saving the record {studentObj.StudentId}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return CreatedAtRoute("GetStudent", new { studentId = studentObj.StudentId }, studentObj);
        //}

        /// <summary>
        /// Chỉnh sửa sinh viên - role = Admin
        /// </summary>
        /// <param name="StudentId"> Nhập Id để sửa sinh viên </param>
        /// <param name="StudentUpdateDto"> Nhập từ khoá để tìm kiếm tên sinh viên </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{StudentId:int}", Name = "UpdateStudent")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateStudent(int StudentId, [FromBody] StudentUpdateDto StudentUpdateDto)
        {
            if (!_student.StudentExists(StudentId))
            {
                return NotFound();
            }
            if (StudentUpdateDto == null || StudentId != StudentUpdateDto.StudentId)
            {
                return BadRequest(ModelState);
            }

            var StudentObj = _mapper.Map<Student>(StudentUpdateDto);
            if (!await _student.UpdateStudent(StudentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {StudentObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá sinh viên - role = Admin
        /// </summary>
        /// <param name="StudentId"> Nhập Id để xoá sinh viên </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột=</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{StudentId:int}", Name = "DeleteStudent")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudent(int StudentId)
        {
            if (!_student.StudentExists(StudentId))
            {
                return NotFound();
            }

            var StudentObj = _student.GetStudent(StudentId);
            if (!await _student.DeleteStudent(StudentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {StudentObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
