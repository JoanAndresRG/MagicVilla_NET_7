using AutoMapper;
using Entities.DTOs;
using Entities.Entiti;
using LogicDomain.Repository.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_API_2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVillaService_Domain _service;
        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger, IVillaService_Domain service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }


        [HttpGet("GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villas = await _service.GetAllVillas();
                IEnumerable<VillaDTO> villaDTOs = _mapper.Map<IEnumerable<VillaDTO>>(villas);
                return Ok(villaDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

        [HttpGet("GetVilla/{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest(id);
                }
                Villa? villa = await _service.GetVilla(id);
                if (villa == null)
                {
                    return NotFound();
                }
                VillaDTO villaDTO = _mapper.Map<VillaDTO>(villa);
                return Ok(villaDTO);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("CreateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVilla([FromBody] VillaDTO villaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Villa villa = _mapper.Map<Villa>(villaDto);
                await _service.AddVilla(villa);
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

        [HttpDelete("DeleteVilla/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest(id);
                }
                await _service.DeleteVilla(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("UpdateVilla/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    return BadRequest(id);
                }
                Villa villa = _mapper.Map<Villa>(villaDto);
                villa.Id = id;
                await _service.UpdateVilla(villa);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPatch("UpdatePartial/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpadtePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> villaDto)
        {
            try
            {
                if (id == 0 || !ModelState.IsValid)
                {
                    return BadRequest(id);
                }
                await _service.UpdatePartialVilla(id, villaDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
