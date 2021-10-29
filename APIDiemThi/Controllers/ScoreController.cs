
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.ScoreDto;
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
    public class ScoreController : ControllerBase
    {
        private readonly IScoreRepository _score;
        private readonly IMapper _mapper;

        public ScoreController(IScoreRepository score, IMapper mapper)
        {
            _score = score;
            _mapper = mapper;
        }

        /// <summary>
        /// Nhận danh sách điểm - Không cần role
        /// </summary>
        /// <param name="studentId"> Nhập mã sinh viên để lấy danh sách điểm của sinh viên đó </param>
        /// <param name="ownerParameters"> Nhập từ khoá để tìm kiếm tên lớp </param>
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
        [ProducesResponseType(200, Type = typeof(List<ScoreViewDto>))]
        public IActionResult GetScores([FromQuery] PageParamers ownerParameters, [FromQuery(Name = "studentId")] string studentId)
        {
            var objList = _score.GetScores(ownerParameters);
            if (studentId != null)
            {
                objList = _score.GetScoresByStudent(int.Parse(studentId), ownerParameters);
            }

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
            var objDto = new List<ScoreViewDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ScoreViewDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Xem thông tin chi tiết điểm - Không cần role
        /// </summary>
        /// <param name="studentId"> Nhập Id sin viên để xem thông tin chi tiết điểm </param>
        /// <param name="subjectId"> Nhập Id môn học để xem thông tin chi tiết điểm </param>
        /// <returns></returns>
        /// <response code="200">Trả về chi tiết lớp học</response> 
        /// <response code="404">Trả về nếu tìm không thấy</response> 
        [HttpGet("{studentId:int}/{subjectId:int}", Name = "GetScore")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ScoreViewDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetScore(int studentId, int subjectId)
        {
            var obj = _score.GetScore(studentId, subjectId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<ScoreViewDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Tạo điểm - role = Admin, Teacher
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Trả về tạo thành công</response> 
        /// <response code="404">Trả về nếu không tạo được</response> 
        /// <response code="500">Trả về nếu không tạo được</response>
        [HttpPost]
        [Authorize(Roles = "admin, teacher")]
        [ProducesResponseType(201, Type = typeof(ScoreViewDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateScore([FromBody] ScoreCreateDto scoreCreateDto)
        {
            if (scoreCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_score.ScoreExists(scoreCreateDto.StudentId, scoreCreateDto.SubjectId))
            {
                ModelState.AddModelError("", "Score Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var scoreObj = _mapper.Map<Score>(scoreCreateDto);
            if (!_score.CreateScore(scoreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {scoreObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetScore", new { studentId = scoreObj.StudentId, subjectId= scoreObj.SubjectId }, scoreObj);
        }

        /// <summary>
        /// Chỉnh sửa điểm - role = Admin, teacher
        /// </summary>
        /// <param name="studentId"> Nhập Id sinh viên để sửa thông tin điểm </param>
        /// <param name="subjectId"> Nhập Id môn học để sửa thông tin điểm </param>
        /// <param name="ScoreUpdateDto"> Nhập Id môn học để sửa thông tin điểm </param>
        /// <returns></returns>
        /// <response code="204">Trả về sửa thành công</response> 
        /// <response code="404">Trả về nếu không sửa được</response> 
        /// <response code="500">Trả về nếu không sửa được</response>
        [HttpPatch("{studentId:int}/{subjectId:int}", Name = "UpdateScore")]
        [Authorize(Roles = "admin, teacher")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateScore(int studentId, int subjectId, [FromBody] ScoreUpdateDto ScoreUpdateDto)
        {
            if (!_score.ScoreExists(studentId, subjectId))
            {
                return NotFound();
            }
            if (ScoreUpdateDto == null || studentId != ScoreUpdateDto.StudentId || subjectId != ScoreUpdateDto.SubjectId)
            {
                return BadRequest(ModelState);
            }

            var ScoreObj = _mapper.Map<Score>(ScoreUpdateDto);
            if (!_score.UpdateScore(ScoreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {ScoreObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Xoá lớp học - role = Admin, teacher
        /// </summary>
        /// <param name="studentId"> Nhập Id sinh viên để xoá thông tin điểm </param>
        /// <param name="subjectId"> Nhập Id môn học để xoá thông tin điểm </param>
        /// <returns></returns>
        /// <response code="204">Trả về xoá thành công</response> 
        /// <response code="404">Trả về nếu không xoá được</response> 
        /// <response code="404">Trả về nếu xoá bị xung đột=</response> 
        /// <response code="500">Trả về nếu không xoá được</response>
        [HttpDelete("{studentId:int}/{subjectId:int}", Name = "DeleteScore")]
        [Authorize(Roles = "admin, teacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteScore(int studentId, int subjectId)
        {
            if (!_score.ScoreExists(studentId, subjectId))
            {
                return NotFound();
            }

            var ScoreObj = _score.GetScore(studentId, subjectId);
            if (!_score.DeleteScore(ScoreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {ScoreObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
