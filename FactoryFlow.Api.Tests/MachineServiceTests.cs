using FactoryFlow.Api.Data;
using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Models;
using FactoryFlow.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace FactoryFlow.Api.Tests;

public class MachineServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsMachineWithIdleStatus()
    {
        await using AppDbContext dbContext = CreateDbContext();
        MachineService service = new(dbContext);

        MachineResponse created = await service.CreateAsync(new CreateMachineRequest
        {
            Code = "MC-100",
            Name = "Cutter"
        });

        Machine? saved = await dbContext.Machines.SingleOrDefaultAsync(x => x.Code == "MC-100");

        Assert.NotNull(saved);
        Assert.Equal("MC-100", created.Code);
        Assert.Equal("Cutter", created.Name);
        Assert.Equal(MachineStatus.Idle, created.Status);
        Assert.Equal("Cutter", saved.Name);
        Assert.Equal(MachineStatus.Idle, saved.Status);
    }

    [Fact]
    public async Task CreateAsync_WhenCodeAlreadyExists_ThrowsInvalidOperationException()
    {
        await using AppDbContext dbContext = CreateDbContext();
        MachineService service = new(dbContext);

        await service.CreateAsync(new CreateMachineRequest
        {
            Code = "MC-100",
            Name = "Cutter"
        });

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateAsync(new CreateMachineRequest
            {
                Code = "MC-100",
                Name = "Duplicate"
            }));
    }

    private static AppDbContext CreateDbContext()
    {
        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
