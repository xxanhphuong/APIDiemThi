
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.TeacherDto;
using APIDiemThi.Repository.IRepository;
using AutoMapper;
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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TeacherViewDto>))]
        [ProducesResponseType(400)]
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

        [HttpGet("{teacherId:int}", Name = "GetTeacher")]
        [ProducesResponseType(200, Type = typeof(TeacherViewDto))]
        [ProducesResponseType(400)]
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


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TeacherCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTeacher([FromBody] TeacherCreateDto teacherCreateDto)
        {
            if (teacherCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_teacher.TeacherExists(teacherCreateDto.TeacherId))
            {
                ModelState.AddModelError("", "Teacher Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var teacherObj = _mapper.Map<Teacher>(teacherCreateDto);
            if (!_teacher.CreateTeacher(teacherObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {teacherObj.TeacherId}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTeacher", new { teacherId = teacherObj.TeacherId }, teacherObj);
        }


        [HttpPatch("{TeacherId:int}", Name = "UpdateTeacher")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTeacher(int TeacherId, [FromBody] TeacherUpdateDto TeacherUpdateDto)
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
            if (!_teacher.UpdateTeacher(TeacherObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TeacherObj.TeacherId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{TeacherId:int}", Name = "DeleteTeacher")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTeacher(int TeacherId)
        {
            if (!_teacher.TeacherExists(TeacherId))
            {
                return NotFound();
            }

            var TeacherObj = _teacher.GetTeacher(TeacherId);
            if (!_teacher.DeleteTeacher(TeacherObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {TeacherObj.TeacherId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
