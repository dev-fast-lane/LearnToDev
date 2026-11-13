using LearnToDev.Api.DTOs;
using LearnToDev.Api.Models;
using LearnToDev.Api.Repositories.Interfaces;
using LearnToDev.Api.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace LearnToDev.Tests.Services;

public class HousingServiceTests
{
    private const int TeltId = 1;
    private const int HybelId = 3;

    private readonly Mock<IHousingRepository> _housingRepository = new();
    private readonly HousingService _service;

    // Two housings the fake repository knows about. Anything not set up answers false or null
    public HousingServiceTests()
    {
        _housingRepository.Setup(r => r.GetByIdAsync(TeltId))
            .ReturnsAsync(new Housing { Id = TeltId, Name = "Telt", Order = 1, HoursPerDay = 12, Rent = 0 });
        _housingRepository.Setup(r => r.GetByIdAsync(HybelId))
            .ReturnsAsync(new Housing { Id = HybelId, Name = "Hybel", Order = 3, HoursPerDay = 16, Rent = 80 });

        _service = new HousingService(_housingRepository.Object, NullLogger<HousingService>.Instance);
    }

    [Fact]
    public async Task CreateAsync_FreeOrder_ReturnsOkWithTrimmedName()
    {
        // Arrange: the fake database gives the new housing id 10, like a real one would
        _housingRepository.Setup(r => r.AddAsync(It.IsAny<Housing>()))
            .ReturnsAsync((Housing h) => { h.Id = 10; return h; });

        // Act
        var result = await _service.CreateAsync(new SaveHousingDto { Name = " Villa ", Order = 6, HoursPerDay = 21, Rent = 500 });

        // Assert
        Assert.Equal(ServiceStatus.Success, result.Status);
        Assert.Equal(10, result.Value!.Id);
        Assert.Equal("Villa", result.Value.Name);
    }

    [Fact]
    public async Task CreateAsync_OrderTaken_ReturnsConflictAndDoesNotSave()
    {
        // Arrange: another housing already has Order 3
        _housingRepository.Setup(r => r.OrderExistsAsync(3, null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(new SaveHousingDto { Name = "Kopi", Order = 3, HoursPerDay = 16, Rent = 80 });

        // Assert
        Assert.Equal(ServiceStatus.Conflict, result.Status);
        _housingRepository.Verify(r => r.AddAsync(It.IsAny<Housing>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_UnknownId_ReturnsNotFound()
    {
        // Arrange: nothing, id 99 is not set up, so the fake repository answers null

        // Act
        var result = await _service.UpdateAsync(99, new SaveHousingDto { Name = "Ukjent", Order = 9, HoursPerDay = 10, Rent = 10 });

        // Assert
        Assert.Equal(ServiceStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_SameOrder_ReturnsOkAndSaves()
    {
        // Act: Hybel keeps Order 3 and only gets a new rent
        var result = await _service.UpdateAsync(HybelId, new SaveHousingDto { Name = "Hybel", Order = 3, HoursPerDay = 16, Rent = 90 });

        // Assert: the Order check skipped Hybel itself, and the change was saved
        Assert.Equal(ServiceStatus.Success, result.Status);
        Assert.Equal(90, result.Value!.Rent);
        _housingRepository.Verify(r => r.OrderExistsAsync(3, HybelId), Times.Once);
        _housingRepository.Verify(r => r.UpdateAsync(It.IsAny<Housing>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_MoveTeltFromOrderOne_ReturnsConflict()
    {
        // Act
        var result = await _service.UpdateAsync(TeltId, new SaveHousingDto { Name = "Telt", Order = 5, HoursPerDay = 12, Rent = 0 });

        // Assert
        Assert.Equal(ServiceStatus.Conflict, result.Status);
        _housingRepository.Verify(r => r.UpdateAsync(It.IsAny<Housing>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Telt_ReturnsConflictAndDoesNotDelete()
    {
        // Act
        var result = await _service.DeleteAsync(TeltId);

        // Assert
        Assert.Equal(ServiceStatus.Conflict, result.Status);
        _housingRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_HousingWithPlayers_ReturnsConflictAndDoesNotDelete()
    {
        // Arrange: someone lives in Hybel
        _housingRepository.Setup(r => r.HasPlayersAsync(HybelId)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(HybelId);

        // Assert
        Assert.Equal(ServiceStatus.Conflict, result.Status);
        _housingRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_EmptyHousing_ReturnsOkAndDeletes()
    {
        // Act: nobody lives in Hybel (not set up, so HasPlayersAsync answers false)
        var result = await _service.DeleteAsync(HybelId);

        // Assert
        Assert.Equal(ServiceStatus.Success, result.Status);
        _housingRepository.Verify(r => r.DeleteAsync(HybelId), Times.Once);
    }
}