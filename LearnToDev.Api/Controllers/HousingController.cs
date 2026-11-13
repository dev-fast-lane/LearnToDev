using LearnToDev.Api.DTOs;
using LearnToDev.Api.Models;
using LearnToDev.Api.Services;
using LearnToDev.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnToDev.Api.Controllers;

//Admin screens for housings (#26). Only admins get in
[ApiController]
[Route("api/housings")]
[Authorize(Roles = Roles.Admin)]
public class HousingsController : ControllerBase
{
    private readonly IHousingService _housingService;

    public HousingsController(IHousingService housingService)
    {
        _housingService = housingService;
    }

    //GET api/housings
    [HttpGet]
    public async Task<ActionResult<List<HousingDto>>> GetAll()
    {
        return Ok(await _housingService.GetAllAsync());
    }

    //GET api/housings/3
    [HttpGet("{id:int}")]
    public async Task<ActionResult<HousingDto>> GetById(int id)
    {
        var housing = await _housingService.GetByIdAsync(id);
        if (housing == null)
            return NotFound();

        return Ok(housing);
    }

    //POST api/housings
    [HttpPost]
    public async Task<ActionResult<HousingDto>> Create(SaveHousingDto dto)
    {
        var result = await _housingService.CreateAsync(dto);
        if (result.Status == ServiceStatus.Conflict)
            return Problem(detail: result.Message, statusCode: StatusCodes.Status409Conflict);

        //201 Created, with a link to the new housing in the Location header
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    //PUT api/housings/3
    [HttpPut("{id:int}")]
    public async Task<ActionResult<HousingDto>> Update(int id, SaveHousingDto dto)
    {
        var result = await _housingService.UpdateAsync(id, dto);
        if (result.Status == ServiceStatus.NotFound)
            return NotFound();
        if (result.Status == ServiceStatus.Conflict)
            return Problem(detail: result.Message, statusCode: StatusCodes.Status409Conflict);

        return Ok(result.Value);
    }

    //DELETE api/housings/3
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _housingService.DeleteAsync(id);
        if (result.Status == ServiceStatus.NotFound)
            return NotFound();
        if (result.Status == ServiceStatus.Conflict)
            return Problem(detail: result.Message, statusCode: StatusCodes.Status409Conflict);

        //204 No Content: deleted, nothing to send back
        return NoContent();
    }
}