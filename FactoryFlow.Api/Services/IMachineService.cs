using FactoryFlow.Api.Dtos;

namespace FactoryFlow.Api.Services
{
    public interface IMachineService
    {
        Task<List<MachineResponse>> GetAllAsync();
        Task<MachineResponse?> GetByCodeAsync(string code);
        Task<MachineResponse> CreateAsync(CreateMachineRequest request);
        Task<MachineResponse> StartAsync(string code);
        Task<MachineResponse> StopAsync(string code);
        Task<MachineResponse> MaintenanceAsync(string code);
    }
}
