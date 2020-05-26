﻿using AutoMapper;
using MammalAPI.DTO;
using MammalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace MammalAPI.Controllers
{
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class HabitatController : ControllerBase
    {

        private readonly IHabitatRepository _habitatRepository;
        private readonly IMapper _mapper;

        public HabitatController(IHabitatRepository habitatRepository, IMapper mapper)
        {
            _habitatRepository = habitatRepository;
            _mapper = mapper;
        }
        // /api/v1.0/habitat/?habitatName=pacific%20ocean  To get habitat by name
        [HttpGet]
        public async Task<IActionResult> GetHabitatByName([FromQuery]string habitatName)
        {
            try
            {
                return Ok(await _habitatRepository.GetHabitatByName(habitatName));
            }
            catch (TimeoutException e)
            {
                return this.StatusCode(StatusCodes.Status408RequestTimeout, $"Request timeout: {e.Message}");
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status404NotFound, $"Something went wrong: {e.Message}");
            }
        }
        ///api/v1.0/habitat/1    To get one habitat by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabitatById(int id)
        {
            try
            {
                var result =await _habitatRepository.GetHabitatById(id);
                var mappedResult = _mapper.Map<HabitatDTO>(result);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }
        }

        ///api/v1.0/habitat/all             To get all habitats
        [HttpGet("all")]
        public async Task<ActionResult<HabitatDTO[]>> GetAllHabitats()
        {
            try
            {
                if (_habitatRepository==null)
                {
                    return NotFound();
                }
                var result= await _habitatRepository.GetAllHabitats();
                var mappedResult = _mapper.Map<HabitatDTO[]>(result);
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database Failure: {e.Message}");
            }
        }
    }
}
