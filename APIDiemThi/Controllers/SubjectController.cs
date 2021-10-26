
using APIDiemThi.Helpers;
using APIDiemThi.Models;
using APIDiemThi.Models.Dtos.SubjectDto;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subject;
        private readonly IMapper _mapper;

        public SubjectController(ISubjectRepository subject, IMapper mapper)
        {
            _subject = subject;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<SubjectViewDto>))]
        [ProducesResponseType(400)]
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

        [HttpGet("{subjectId:int}", Name = "GetSubject")]
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


        [HttpPost]
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


        [HttpPatch("{SubjectId:int}", Name = "UpdateSubject")]
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

        [HttpDelete("{SubjectId:int}", Name = "DeleteSubject")]
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
