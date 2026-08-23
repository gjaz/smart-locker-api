using Microsoft.AspNetCore.Mvc;
using Moq;
using SmartLocker.Api.Controllers;
using SmartLocker.Api.DTOs;
using SmartLocker.Api.Services;

namespace SmartLocker.Api.Tests.Controllers;

public class LockersControllerTests
{
    private readonly Mock<ILockerService> _mockService;

    public LockersControllerTests()
    {
        _mockService = new Mock<ILockerService>();
    }

    [Fact]
    public async Task Get_Should_Return_All_Lockers()
    {
        var lockers = new List<LockerDto>
        {
            new LockerDto
            {
                Id = 1,
                Codigo = "L-001",
                Ubicacion = "Planta 1",
                Estado = "Disponible",
                Tamano = "Mediano"
            },
            new LockerDto
            {
                Id = 2,
                Codigo = "L-002",
                Ubicacion = "Planta 2",
                Estado = "Ocupado",
                Tamano = "Grande"
            }
        };

        _mockService
        .Setup(x => x.GetAllAsync())
        .ReturnsAsync(lockers);

        var controller = new LockersController(_mockService.Object);

        var result = await controller.Get();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("L-001", result.First().Codigo);
        Assert.Equal("L-002", result.Last().Codigo);

        _mockService.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Post_Should_Create_Locker()
    {
        var dto = new CreateLockerDto
        {
            Codigo = "L-003",
            Ubicacion = "Planta 3",
            Estado = "Disponible",
            Tamano = "Pequeno"
        };

        var lockerCreado = new LockerDto
        {
            Id = 3,
            Codigo = "L-003",
            Ubicacion = "Planta 3",
            Estado = "Disponible",
            Tamano = "Pequeno"
        };

        _mockService
            .Setup(x => x.AddAsync(dto))
            .ReturnsAsync(lockerCreado);

        var controller = new LockersController(_mockService.Object);
        var result = await controller.Post(dto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var returnedLocker = Assert.IsType<LockerDto>(okResult.Value);

        Assert.Equal("L-003", returnedLocker.Codigo);
        Assert.Equal("Planta 3", returnedLocker.Ubicacion);
        Assert.Equal("Disponible", returnedLocker.Estado);
        Assert.Equal("Pequeno", returnedLocker.Tamano);

        _mockService.Verify(
            x => x.AddAsync(dto),
            Times.Once);

    }

    [Fact]
    public async Task Delete_Should_Return_NoContent_When_Locker_Exists()
    {
        var lockerId = 1;

        _mockService
            .Setup(x => x.DeleteAsync(lockerId))
            .ReturnsAsync(true);

        var controller = new LockersController(_mockService.Object);
        var result = await controller.Delete(lockerId);

        Assert.IsType<NoContentResult>(result);

        _mockService.Verify(
            x => x.DeleteAsync(lockerId),
            Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Return_NotFound_When_Locker_Does_Not_Exist()
    {
        var lockerId = 999;

        _mockService
            .Setup(x => x.DeleteAsync(lockerId))
            .ReturnsAsync(false);

        var controller = new LockersController(_mockService.Object);
        var result = await controller.Delete(lockerId);

        Assert.IsType<NotFoundResult>(result);

        _mockService.Verify(
            x => x.DeleteAsync(lockerId),
            Times.Once);
    }

    [Fact]
    public async Task Put_Should_Return_Ok_When_Locker_Exists()
    {
        // Arrange
        var lockerId = 1;

        var dto = new UpdateLockerDto
        {
            Codigo = "L-001-UPD",
            Ubicacion = "Planta Actualizada",
            Estado = "Ocupado",
            Tamano = "Grande"
        };

        var lockerActualizado = new LockerDto
        {
            Id = lockerId,
            Codigo = "L-001-UPD",
            Ubicacion = "Planta Actualizada",
            Estado = "Ocupado",
            Tamano = "Grande"
        };

        _mockService
            .Setup(x => x.UpdateAsync(lockerId, dto))
            .ReturnsAsync(lockerActualizado);

        var controller = new LockersController(_mockService.Object);

        // Act
        var result = await controller.Put(lockerId, dto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var returnedLocker =
            Assert.IsType<LockerDto>(okResult.Value);

        Assert.Equal("L-001-UPD", returnedLocker.Codigo);
        Assert.Equal("Planta Actualizada", returnedLocker.Ubicacion);
        Assert.Equal("Ocupado", returnedLocker.Estado);
        Assert.Equal("Grande", returnedLocker.Tamano);

        _mockService.Verify(
            x => x.UpdateAsync(lockerId, dto),
            Times.Once);
    }

    [Fact]
    public async Task Put_Should_Return_NotFound_When_Locker_Does_Not_Exist()
    {
        // Arrange
        var lockerId = 999;

        var dto = new UpdateLockerDto
        {
            Codigo = "L-999",
            Ubicacion = "No Existe",
            Estado = "Disponible",
            Tamano = "Pequeno"
        };

        _mockService
            .Setup(x => x.UpdateAsync(lockerId, dto))
            .ReturnsAsync((LockerDto?)null);

        var controller = new LockersController(_mockService.Object);

        // Act
        var result = await controller.Put(lockerId, dto);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        _mockService.Verify(
            x => x.UpdateAsync(lockerId, dto),
            Times.Once);
    }
}
