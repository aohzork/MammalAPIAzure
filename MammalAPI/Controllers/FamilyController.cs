﻿using MammalAPI.DTO;
using MammalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using AutoMapper;
using MammalAPI.Models;

namespace MammalAPI.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyRepository _familyRepository;
        private readonly IMapper _mapper;
        public FamilyController(IFamilyRepository familyRepository, IMapper mapper)
        {
            _familyRepository = familyRepository;
            this._mapper = mapper;
        }

        ///api/v1.0/family       Get all families
        [HttpGet]
        public async Task<IActionResult> GetAllFamilies([FromQuery]bool includeMammals = false)
        {
            try
            {
                var results = await _familyRepository.GetAllFamilies(includeMammals);
                var mappedResult = _mapper.Map<FamilyDTO[]>(results);
                return Ok(mappedResult);
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }
        }

        ///api/v1.0/family/1   Get family by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFamilyById(int id)
        {
            try
            {
                var result = await _familyRepository.GetFamilyById(id);
                var mappedResult = _mapper.Map<FamilyDTO>(result);
                return Ok(mappedResult);
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");

            }
        }

        ///api/v1.0/family/Phocidae      Get family by name
        [HttpGet("{name}")]
        public async Task<ActionResult> GetFamilyByName(string name, [FromQuery] bool includeMammals = false)
        {
            try
            {
                var result = await _familyRepository.GetFamilyByName(name, includeMammals);
                var mappedResult = _mapper.Map<FamilyDTO>(result);
                return Ok(mappedResult);
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FamilyDTO>> PostFamily(FamilyDTO familyDTO)
        {
            try
            {
                var mappedEntity = _mapper.Map<Family>(familyDTO);
                _familyRepository.Add(mappedEntity);
                if (await _familyRepository.Save())
                {
                    return Created($"/api/v1.0/family/id{familyDTO.FamilyID}", _mapper.Map<FamilyDTO>(mappedEntity));
                }
            }

            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"database failure {e.Message}");
            }

            return BadRequest();
        }

        ///api/v1.0/family/##       Put a family by id
        [HttpPut("{familyId}")]
        public async Task<ActionResult<FamilyDTO>> PutFamily (int familyId, FamilyDTO familyDTO)
        {
            try
            {
                var oldFamily = await _familyRepository.GetFamilyById(familyId);
                if (oldFamily == null)
                {
                    return NotFound($"Family with ID: {familyId} could not be found.");
                }

                var newFamily = _mapper.Map(familyDTO, oldFamily);
                _familyRepository.Update(newFamily);
                if (await _familyRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }

            return BadRequest();
        }


        [HttpDelete("{familyId}")]
        public async Task<ActionResult<FamilyDTO>> DeleteFamily (int familyId)
        {
            try
            {
                var familyToDelete = await _familyRepository.GetFamilyById(familyId);
                if (familyToDelete == null)
                {
                    return NotFound($"Family with ID: {familyId} could not be found.");
                }

                _familyRepository.Delete(familyToDelete);

                if (await _familyRepository.Save())
                {
                    return NoContent();
                }
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }

            return BadRequest();
        }
    }
}
