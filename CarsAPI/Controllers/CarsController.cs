using AutoMapper;
using CarsAPI.Repository.Irepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarsAPI.Models;
using CarsAPI.Models.Dtos;

namespace CarsAPI.Controllers
{
    [Route("api/Cars")]
    [ApiController]
    [ApiExplorerSettings( GroupName = "APICars")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public class CarsController : Controller
    {
        private readonly ICarsRepository _repo;
        private readonly IMapper _mapper;

        public CarsController(IMapper mapper,ICarsRepository repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CarsDtos>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetCars()
        {
            List<Cars> list = await _repo.GetCars();

            List<CarsDtos> listDto = new List<CarsDtos>();

            foreach (Cars item in list)
            {
                listDto.Add(_mapper.Map<CarsDtos>(item));
            }

            return Ok(listDto);
        }
        [HttpGet("{IdCars:int}", Name = "GetCarById")]
        [ProducesResponseType(200, Type = typeof(List<CarsDtos>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetCarById(int IdCars)
        {
            Cars item = await _repo.GetCarById(IdCars);

            if (item == null)
            {
                return NotFound();
            }

            CarsDtos carsDto = _mapper.Map<CarsDtos>(item);

            return Ok(carsDto);
        }
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<CarsDtos>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]

        public async Task<ActionResult> CreateCars([FromBody] CarsDtos carsDto)
        {
            if (carsDto == null)
            {
                return BadRequest(ModelState);
            }

            bool value = await _repo.ExistCars(carsDto.Name);

            if (value)
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            Cars item = _mapper.Map<Cars>(carsDto);

            value = await _repo.CreateCars(item);

            if (!value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCarById", new { IdCars = item.Id }, item);
        }
        [HttpPatch("{IdCars:int:int}", Name = "UpdateCars")]
        [ProducesResponseType(204, Type = typeof(List<CarsDtos>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateCars([FromBody] CarsDtos carsDto, int IdCars)
        {
            if (carsDto.Id != IdCars)
            {
                ModelState.AddModelError("", "Los id son diferente.");
                return StatusCode(400, ModelState);
            }
            if (carsDto == null)
            {
                ModelState.AddModelError("", "El formulario esta vacio");
                return StatusCode(400, ModelState);
            }
            bool Value = await _repo.ExistCars(IdCars);

            if (!Value)
            {
                ModelState.AddModelError("", "La categoria que desea actualizar, no existe en la base de datos.");
                return StatusCode(404, ModelState);
            }

            Cars cars = _mapper.Map<Cars>(carsDto);
            Value = await _repo.UpdateCars(cars);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCarById", new { IdCars = cars.Id }, cars);
        }

        [HttpDelete("{IdCars:int}", Name = "DeleteCars")]
        [ProducesResponseType(200, Type = typeof(List<CarsDtos>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteCars(int IdCars)
        {
            bool Value = await _repo.ExistCars(IdCars);

            if (!Value)
            {
                ModelState.AddModelError("", "La categoria que deseas eliminar no existe.");
                return StatusCode(404, ModelState);
            }

            Cars cars = await _repo.GetCarById(IdCars);

            Value = await _repo.DeleteCars(cars);

            if (!Value)
            {
                ModelState.AddModelError("", "Ha pasado algo con la base de datos, por favor comuniquese con servicio tecnico.");
                return StatusCode(500, ModelState);
            }

            return Ok($"Se ha eliminado correctamente la categoria {cars.Name} de la base de datos.");
        }


    }
}
