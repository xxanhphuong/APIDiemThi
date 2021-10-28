using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using APIDiemThi.Repository.IRepository;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.MajorDto;
using Microsoft.AspNetCore.Authorization;
using APIDiemThi.Helpers;
using Newtonsoft.Json;

namespace APIDiemThi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly IMajorRepository _major;
        private readonly IMapper _mapper;

        public MajorController(IMajorRepository major, IMapper mapper)
        {
            _major = major;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách ngành học - Không cần role
        /// </summary>
        /// <param name="kw"> Nhập từ khoá để tìm kiếm tên ngành </param>
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
        /// <response code="200">Trả về danh sách ngành</response>            
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MajorViewDto>))]
        public IActionResult GetMajors([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "kw")] string kw)
        {
            var objList = _major.GetMajors(kw, ownerParameters);
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



            var objDto = new List<MajorViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<MajorViewDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Xem thông tin chi tiết ngành học - Không cần role
        /// </summary>
        /// <param name="majorId"> Nhập Id để xem thông tin chi tiết ngành học </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết ngành</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response>       
        [HttpGet("{majorId:int}", Name = "GetMajor")]
        [ProducesResponseType(200, Type = typeof(MajorViewDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetMajor(int majorId)
        {
            var obj = _major.GetMajor(majorId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<MajorViewDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Tạo ngành học - role = Admin
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Trả về chi tiết ngành vừa tạo</response> 
        /// <response code="404">Trả về nếu không tạo được</response> 
        /// <response code="500">Trả về nếu không tạo được</response>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MajorCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateMajor([FromBody] MajorCreateDto majorCreateDto)
        {
            if (majorCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_major.MajorExists(majorCreateDto.Name))
            {
                ModelState.AddModelError("", "Major Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var majorObj = _mapper.Map<Major>(majorCreateDto);
            if (!_major.CreateMajor(majorObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {majorObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetMajor", new { majorId = majorObj.Id }, majorObj);
        }

        /// <summary>
        /// Chỉnh sửa ngành học - role = Admin
        /// </summary>
        /// <param name="MajorId"> Nhập Id để sửa lớp học </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{MajorId:int}", Name = "UpdateMajor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMajor(int MajorId, [FromBody] MajorUpdateDto MajorUpdateDto)
        {
            if (!_major.MajorExists(MajorId))
            {
                return NotFound();
            }
            if (MajorUpdateDto == null || MajorId != MajorUpdateDto.Id)
            {
                return BadRequest(ModelState);
            }

            var MajorObj = _mapper.Map<Major>(MajorUpdateDto);
            if (!_major.UpdateMajor(MajorObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {MajorObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //[HttpDelete("{majorId:int}", Name = "DeleteMajor")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult DeleteMajor(int majorId)
        //{
        //    if (!_major.MajorExists(majorId))
        //    {
        //        return NotFound();
        //    }

        //    var MajorObj = _major.GetMajor(majorId);
        //    if (!_major.DeleteMajor(MajorObj))
        //    {
        //        ModelState.AddModelError("", $"Something went wrong when deleting the record {MajorObj.Name}");
        //        return StatusCode(500, ModelState);
        //    }

        //    return NoContent();
        //}
    }
}
