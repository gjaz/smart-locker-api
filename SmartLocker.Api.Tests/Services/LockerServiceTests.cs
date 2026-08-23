using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.DTOs;
using SmartLocker.Api.Models;
using SmartLocker.Api.Services;

namespace SmartLocker.Api.Tests.Services;

public class LockerServiceTests
{
    [Fact]
    public async Task AddAsync_Should_Create_Locker()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        var service = new LockerService(context);

        var locker = new CreateLockerDto
        {
            Codigo = "L-100",
            Ubicacion = "Pruebas",
            Estado = "Disponible",
            Tamano = "Mediano"
        };

        // Act
        var result = await service.AddAsync(locker);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("L-100", result.Codigo);
        Assert.Single(context.Lockers);
        Assert.Equal("Pruebas", result.Ubicacion);
        Assert.Equal("Disponible", result.Estado);
        Assert.Equal("Mediano", result.Tamano);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Lockers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        context.Lockers.AddRange(
            new Locker
            {
                Codigo = "L-001",
                Ubicacion = "Planta 1",
                Estado = "Disponible",
                Tamano = "Mediano"
            },
            new Locker
            {
                Codigo = "L-002",
                Ubicacion = "Planta 2",
                Estado = "Ocupado",
                Tamano = "Grande"
            });

        await context.SaveChangesAsync();

        var service = new LockerService(context);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());

        var lockers = result.ToList();
        Assert.Equal("L-001", lockers[0].Codigo);
        Assert.Equal("Planta 1", lockers[0].Ubicacion);
        Assert.Equal("Disponible", lockers[0].Estado);
        Assert.Equal("Mediano", lockers[0].Tamano);

        Assert.Equal("L-002", lockers[1].Codigo);
        Assert.Equal("Planta 2", lockers[1].Ubicacion);
        Assert.Equal("Ocupado", lockers[1].Estado);
        Assert.Equal("Grande", lockers[1].Tamano);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_Locker_Does_Not_Exist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        var service = new LockerService(context);

        var dto = new UpdateLockerDto
        {
            Codigo = "L-999",
            Ubicacion = "Planta 9",
            Estado = "Disponible",
            Tamano = "Grande"
        };

        // Act
        var result = await service.UpdateAsync(999, dto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_Locker()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        var locker = new Locker
        {
            Codigo = "L-001",
            Ubicacion = "Planta 1",
            Estado = "Disponible",
            Tamano = "Mediano"
        };

        context.Lockers.Add(locker);

        await context.SaveChangesAsync();

        var service = new LockerService(context);

        var dto = new UpdateLockerDto
        {
            Codigo = "L-001",
            Ubicacion = "Planta 2",
            Estado = "Ocupado",
            Tamano = "Grande"
        };

        // Act
        var result = await service.UpdateAsync(locker.Id, dto);

        // Assert
        Assert.NotNull(result);

        Assert.Equal("L-001", result.Codigo);
        Assert.Equal("Planta 2", result.Ubicacion);
        Assert.Equal("Ocupado", result.Estado);
        Assert.Equal("Grande", result.Tamano);

        var lockerActualizado = await context.Lockers
            .FirstAsync(x => x.Id == locker.Id);

        Assert.Equal("Planta 2", lockerActualizado.Ubicacion);
        Assert.Equal("Ocupado", lockerActualizado.Estado);
        Assert.Equal("Grande", lockerActualizado.Tamano);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Existing_Locker()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        var locker = new Locker
        {
            Codigo = "L-001",
            Ubicacion = "Planta 1",
            Estado = "Disponible",
            Tamano = "Mediano"
        };

        context.Lockers.Add(locker);

        await context.SaveChangesAsync();

        var service = new LockerService(context);

        // Act
        var result = await service.DeleteAsync(locker.Id);

        // Assert
        Assert.True(result);

        Assert.Empty(context.Lockers);
    }

    [Fact]
    public async Task DeleteAsync_Should_Return_False_When_Locker_Does_Not_Exist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<SmartLockerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new SmartLockerDbContext(options);

        var service = new LockerService(context);

        // Act
        var result = await service.DeleteAsync(999);

        // Assert
        Assert.False(result);

        Assert.Empty(context.Lockers);
    }
}
