using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.StudentDto;
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
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _student;
        private readonly IMapper _mapper;
        public StudentController(IStudentRepository student, IMapper mapper)
        {
            _student = student;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<StudentViewDto>))]
        [ProducesResponseType(400)]
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

        [HttpGet("{studentId:int}", Name = "GetStudent")]
        [ProducesResponseType(200, Type = typeof(StudentViewDto))]
        [ProducesResponseType(400)]
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


        [HttpPost]
        [ProducesResponseType(201, Type = typeof(StudentCreateDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateStudent([FromBody] StudentCreateDto studentCreateDto)
        {
            if (studentCreateDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_student.StudentExists(studentCreateDto.StudentId))
            {
                ModelState.AddModelError("", "Student Exists");
                return StatusCode(404, ModelState);
            }

            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/

            var studentObj = _mapper.Map<Student>(studentCreateDto);
            if (!_student.CreateStudent(studentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {studentObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetStudent", new { studentId = studentObj.StudentId }, studentObj);
        }


        [HttpPatch("{StudentId:int}", Name = "UpdateStudent")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateStudent(int StudentId, [FromBody] StudentUpdateDto StudentUpdateDto)
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
            if (!_student.UpdateStudent(StudentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {StudentObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{StudentId:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStudent(int StudentId)
        {
            if (!_student.StudentExists(StudentId))
            {
                return NotFound();
            }

            var StudentObj = _student.GetStudent(StudentId);
            if (!_student.DeleteStudent(StudentObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {StudentObj.StudentId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
