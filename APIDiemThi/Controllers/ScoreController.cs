
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ScoreViewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetScores([FromQuery] PageParamers ownerParameters,[FromQuery(Name = "studentId")] string studentId)
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


        [HttpGet("{studentId:int}/{subjectId:int}", Name = "GetScore")]
        [ProducesResponseType(200, Type = typeof(ScoreViewDto))]
        [ProducesResponseType(400)]
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


        [HttpPost]
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


        [HttpPatch("{studentId:int}/{subjectId:int}", Name = "UpdateScore")]
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

        [HttpDelete("{studentId:int}/{subjectId:int}", Name = "DeleteScore")]
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
