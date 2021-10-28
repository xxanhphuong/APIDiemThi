using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using APIDiemThi.Repository.IRepository;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.ClassesDto;
using Microsoft.AspNetCore.Authorization;
using APIDiemThi.Helpers;
using Newtonsoft.Json;

namespace APIDiemThi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesRepository _Classes;
        private readonly IMapper _mapper;

        public ClassesController(IClassesRepository Classes, IMapper mapper)
        {
            _Classes = Classes;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách lớp - Không cần role
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên lớp </param>
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
        /// <response code="200">Trả về danh sách lớp</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<ClassesViewDto>))]
        public IActionResult GetClassess([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList =  _Classes.GetClassess(kw, ownerParameters);
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

            var objDto = new List<ClassesViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ClassesViewDto>(obj));
            }

            return Ok(objDto);
        }


        /// <summary>
        /// Xem thông tin chi tiết lớp học - Không cần role
        /// </summary>
        /// <param name="ClassesId"> Nhập Id để xem thông tin chi tiết lớp học </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết lớp học</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response> 
        [HttpGet("{ClassesId:int}", Name = "GetClasses")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ClassesViewDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetClasses(int ClassesId)
        {
            var obj =  _Classes.GetClasses(ClassesId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ClassesViewDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Tạo lớp học - role = Admin
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Trả về tạo thành công</response> 
        /// <response code="404">Trả về nếu không tạo được</response> 
        /// <response code="500">Trả về nếu không tạo được</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(201, Type = typeof(ClassesCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateClasses([FromBody] ClassesCreateDto ClassesCreateDto)
        {
            if (ClassesCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_Classes.ClassesExists(ClassesCreateDto.Name))
            {
                ModelState.AddModelError("", "Classes Exists");
                return StatusCode(404, ModelState);
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var ClassesObj = _mapper.Map<Classes>(ClassesCreateDto);
            if (!_Classes.CreateClasses(ClassesObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {ClassesObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetClasses", new { ClassesId = ClassesObj.Id }, ClassesObj);
        }

        /// <summary>
        /// Chỉnh sửa lớp học - role = Admin
        /// </summary>
        /// <param name="ClassesId"> Nhập Id để sửa lớp học </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{ClassesId:int}", Name = "UpdateClasses")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateClasses(int ClassesId, [FromBody] ClassesUpdateDto ClassesUpdateDto)
        {
            if (!_Classes.ClassesExists(ClassesId))
            {
                return NotFound();
            }
            if (ClassesUpdateDto == null || ClassesId != ClassesUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var ClassesObj = _mapper.Map<Classes>(ClassesUpdateDto);
            if (!_Classes.UpdateClasses(ClassesObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {ClassesObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá lớp học - role = Admin
        /// </summary>
        /// <param name="ClassesId"> Nhập Id để xoá lớp học </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột=</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{ClassesId:int}", Name = "DeleteClasses")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteClasses(int ClassesId)
        {
            if (! _Classes.ClassesExists(ClassesId))
            {
                return NotFound();
            }

            var ClassesObj = _Classes.GetClasses(ClassesId);
            if (! _Classes.DeleteClasses(ClassesObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {ClassesObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
