using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsModel = await _regionRepository.GetAllAsync();

            var regionResponse = _mapper.Map<List<RegionDTO>>(regionsModel);

            return Ok(regionResponse);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionResponse = _mapper.Map<RegionDTO>(region);

            return Ok(regionResponse);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionModel = _mapper.Map<Region>(addRegionRequestDTO);

            await _regionRepository.CreateAsync(regionModel);

            var regionResponse = _mapper.Map<RegionDTO>(regionModel);

            return CreatedAtAction(nameof(GetById), new { id = regionResponse.Id }, regionResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            var regionModel = _mapper.Map<Region>(updateRegionRequestDTO);
            regionModel = await _regionRepository.UpdateAsync(id, regionModel);
            if (regionModel == null)
            {
                return NotFound();
            }
            var regionResponse = _mapper.Map<RegionDTO>(regionModel);

            return Ok(regionResponse);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionModel = await _regionRepository.DeleteAsync(id);
            if (regionModel == null)
            {
                return NotFound();
            }

            return Content(regionModel.Name + " Deleted Successfully");
        }
    }
}
