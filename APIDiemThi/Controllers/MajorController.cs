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

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(List<MajorViewDto>))]
        [ProducesResponseType(400)]
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

        [HttpGet("{majorId:int}", Name = "GetMajor")]
        [ProducesResponseType(200, Type = typeof(MajorViewDto))]
        [ProducesResponseType(400)]
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


        [HttpPatch("{MajorId:int}", Name = "UpdateMajor")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
