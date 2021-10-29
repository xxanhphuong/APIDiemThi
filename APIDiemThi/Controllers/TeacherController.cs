
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.TeacherDto;
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
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacher;
        private readonly IMapper _mapper;
        public TeacherController(ITeacherRepository teacher, IMapper mapper)
        {
            _teacher = teacher;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách giảng viên - Không cần role
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên giảng viên </param>
        /// <param name="ownerParameters"> Nhập từ khoá để tìm kiếm tên giảng viên </param>
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
        /// <response code="200">Trả về danh sách giảng viên</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<TeacherViewDto>))]
        public IActionResult GetTeachers([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList = _teacher.GetTeachers(kw, ownerParameters);
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

            var objDto = new List<TeacherViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TeacherViewDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Xem thông tin chi tiết giảng viên - Không cần role
        /// </summary>
        /// <param name="teacherId"> Nhập Id sinh viên để xem thông tin chi tiết giảng viên </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết giảng viên</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response>
        [HttpGet("{teacherId:int}", Name = "GetTeacher")]
        [ProducesResponseType(200, Type = typeof(TeacherViewDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTeacher(int teacherId)
        {
            var obj = _teacher.GetTeacher(teacherId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TeacherViewDto>(obj);
            return Ok(objDto);
        }


        //[HttpPost]
        //[ProducesResponseType(201, Type = typeof(TeacherCreateDto))]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesDefaultResponseType]
        //public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreateDto teacherCreateDto)
        //{
        //    if (teacherCreateDto == null)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (_teacher.TeacherExists(teacherCreateDto.TeacherId))
        //    {
        //        ModelState.AddModelError("", "Teacher Exists");
        //        return StatusCode(404, ModelState);
        //    }

        //    /*if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }*/

        //    var teacherObj = _mapper.Map<Teacher>(teacherCreateDto);
        //    if (!await _teacher.CreateTeacher(teacherObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when saving the record {teacherObj.TeacherId}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return CreatedAtRoute("GetTeacher", new { teacherId = teacherObj.TeacherId }, teacherObj);
        //}

        /// <summary>
        /// Chỉnh sửa giảng viên - role = Admin
        /// </summary>
        /// <param name="TeacherId"> Nhập Id để sửa giảng viên </param>
        /// <param name="TeacherUpdateDto"> Nhập từ khoá để tìm kiếm tên giảng viên </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{TeacherId:int}", Name = "UpdateTeacher")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTeacher(int TeacherId, [FromBody] TeacherUpdateDto TeacherUpdateDto)
        {
            if (!_teacher.TeacherExists(TeacherId))
            {
                return NotFound();
            }
            if (TeacherUpdateDto == null || TeacherId != TeacherUpdateDto.TeacherId)
            {
                return BadRequest(ModelState);
            }

            var TeacherObj = _mapper.Map<Teacher>(TeacherUpdateDto);
            if (!await _teacher.UpdateTeacher(TeacherObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TeacherObj.TeacherId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá giảng viên - role = Admin
        /// </summary>
        /// <param name="TeacherId"> Nhập Id để xoá giảng viên </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{TeacherId:int}", Name = "DeleteTeacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTeacher(int TeacherId)
        {
            if (!_teacher.TeacherExists(TeacherId))
            {
                return NotFound();
            }

            var TeacherObj = _teacher.GetTeacher(TeacherId);
            if (!await _teacher.DeleteTeacher(TeacherObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {TeacherObj.TeacherId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
