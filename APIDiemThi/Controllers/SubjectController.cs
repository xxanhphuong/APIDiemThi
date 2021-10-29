
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.SubjectDto;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subject;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectRepository subject, IMapper mapper)
        {
            _subject = subject;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách môn học - Không cần role
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên môn học </param>
        /// <param name="ownerParameters"> Nhập từ khoá để tìm kiếm tên môn học </param>
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
        /// <response code="200">Trả về danh sách môn học</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<SubjectViewDto>))]
        public IActionResult GetSubjects([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList = _subject.GetSubjects(kw, ownerParameters);
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
            var objDto = new List<SubjectViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<SubjectViewDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Xem thông tin chi tiết môn học - Không cần role
        /// </summary>
        /// <param name="subjectId"> Nhập Id để xem thông tin chi tiết môn học </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết môn học</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response> 
        [HttpGet("{subjectId:int}", Name = "GetSubject")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(SubjectViewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetSubject(int subjectId)
        {
            var obj = _subject.GetSubject(subjectId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<SubjectViewDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Tạo môn học - role = Admin
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Trả về tạo môn thành công</response> 
        /// <response code="404">Trả về nếu không tạo được</response> 
        /// <response code="500">Trả về nếu không tạo được</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(201, Type = typeof(SubjectCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateSubject([FromBody] SubjectCreateDto subjectCreateDto)
        {
            if (subjectCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_subject.SubjectExists(subjectCreateDto.Name))
            {
                ModelState.AddModelError("", "Subject Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var subjectObj = _mapper.Map<Subject>(subjectCreateDto);
            if (!_subject.CreateSubject(subjectObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {subjectObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetSubject", new { subjectId = subjectObj.Id }, subjectObj);
        }

        /// <summary>
        /// Chỉnh sửa môn học - role = Admin
        /// </summary>
        /// <param name="SubjectId"> Nhập Id để sửa môn học </param>
        /// <param name="SubjectUpdateDto"> Nhập Id để sửa môn học </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{SubjectId:int}", Name = "UpdateSubject")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateSubject(int SubjectId, [FromBody] SubjectUpdateDto SubjectUpdateDto)
        {
            if (!_subject.SubjectExists(SubjectId))
            {
                return NotFound();
            }
            if (SubjectUpdateDto == null || SubjectId != SubjectUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var SubjectObj = _mapper.Map<Subject>(SubjectUpdateDto);
            if (!_subject.UpdateSubject(SubjectObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {SubjectObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá môn học - role = Admin
        /// </summary>
        /// <param name="SubjectId"> Nhập Id để xoá môn học </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột=</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{SubjectId:int}", Name = "DeleteSubject")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSubject(int SubjectId)
        {
            if (!_subject.SubjectExists(SubjectId))
            {
                return NotFound();
            }

            var SubjectObj = _subject.GetSubject(SubjectId);
            if (!_subject.DeleteSubject(SubjectObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {SubjectObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
