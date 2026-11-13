using LearnToDev.Api.DTOs;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services.Interfaces;

namespace LearnToDev.Api.Services;

//The rules for housings. The controller calls this, and this calls the repository
public class HousingService : IHousingService
{
    private readonly IHousingRepository _housingRepository;
    private readonly ILogger<HousingService> _logger;

    public HousingService(IHousingRepository housingRepository, ILogger<HousingService> logger)
    {
        _housingRepository = housingRepository;
        _logger = logger;
    }

    public async Task<List<HousingDto>> GetAllAsync()
    {
        var housings = await _housingRepository.GetAllAsync();
        return housings.Select(ToDto).ToList();
    }

    public async Task<HousingDto?> GetByIdAsync(int id)
    {
        var housing = await _housingRepository.GetByIdAsync(id);
        return housing == null ? null : ToDto(housing);
    }

    public async Task<ServiceResult<HousingDto>> CreateAsync(SaveHousingDto dto)
    {
        if (await _housingRepository.OrderExistsAsync(dto.Order))
            return ServiceResult<HousingDto>.Conflict($"En annen bolig har allerede rekkefølge {dto.Order}.");

        var housing = new Housing
        {
            Name = dto.Name.Trim(),
            Order = dto.Order,
            HoursPerDay = dto.HoursPerDay,
            Rent = dto.Rent
        };

        var created = await _housingRepository.AddAsync(housing);
        _logger.LogInformation("Bolig {HousingId} opprettet.", created.Id);
        return ServiceResult<HousingDto>.Ok(ToDto(created));
    }

    public async Task<ServiceResult<HousingDto>> UpdateAsync(int id, SaveHousingDto dto)
    {
        var housing = await _housingRepository.GetByIdAsync(id);
        if (housing == null)
            return ServiceResult<HousingDto>.NotFound();

        //Players are moved to Telt (Order 1) when evicted (#43), so Telt must stay first
        if (housing.Order == 1 && dto.Order != 1)
            return ServiceResult<HousingDto>.Conflict("Telt må ha rekkefølge 1.");

        //Pass id, so the housing doesn't collide with its own Order
        if (await _housingRepository.OrderExistsAsync(dto.Order, id))
            return ServiceResult<HousingDto>.Conflict($"En annen bolig har allerede rekkefølge {dto.Order}.");

        housing.Name = dto.Name.Trim();
        housing.Order = dto.Order;
        housing.HoursPerDay = dto.HoursPerDay;
        housing.Rent = dto.Rent;

        await _housingRepository.UpdateAsync(housing);
        _logger.LogInformation("Bolig {HousingId} oppdatert.", id);
        return ServiceResult<HousingDto>.Ok(ToDto(housing));
    }

    public async Task<ServiceResult<bool>> DeleteAsync(int id)
    {
        var housing = await _housingRepository.GetByIdAsync(id);
        if (housing == null)
            return ServiceResult<bool>.NotFound();

        //Evicted players are moved to Telt (#43), so it must always exist
        if (housing.Order == 1)
            return ServiceResult<bool>.Conflict("Telt kan ikke slettes, fordi spillere flyttes dit når de blir kastet ut.");

        //Players would be left without a home
        if (await _housingRepository.HasPlayersAsync(id))
            return ServiceResult<bool>.Conflict("Boligen kan ikke slettes fordi noen spillere bor der.");

        await _housingRepository.DeleteAsync(id);
        _logger.LogInformation("Bolig {HousingId} slettet.", id);
        return ServiceResult<bool>.Ok(true);
    }

    //Turns the entity into the DTO we send out, so Players never leaves the API
    private static HousingDto ToDto(Housing housing) =>
        new(housing.Id, housing.Name, housing.Order, housing.HoursPerDay, housing.Rent);
}