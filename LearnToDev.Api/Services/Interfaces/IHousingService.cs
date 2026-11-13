using LearnToDev.Api.DTOs;
using LearnToDev.Api.Services;

namespace LearnToDev.Api.Services.Interfaces;

public interface IHousingService
{
    Task<List<HousingDto>> GetAllAsync();

    Task<HousingDto?> GetByIdAsync(int id);

    Task<ServiceResult<HousingDto>> CreateAsync(SaveHousingDto dto);

    Task<ServiceResult<HousingDto>> UpdateAsync(int id, SaveHousingDto dto);

    Task<ServiceResult<bool>> DeleteAsync(int id);
}

