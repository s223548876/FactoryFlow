using FactoryFlow.Api.Dtos;

namespace FactoryFlow.Api.Services
{
    public interface IWorkOrderService
    {
        Task<WorkOrderResponse> CreateAsync(CreateWorkOrderRequest request);
        Task<List<WorkOrderResponse>> GetAllAsync();
        Task<WorkOrderResponse?> GetByIdAsync(int id);
        Task<WorkOrderResponse> StartAsync(int id);
        Task<WorkOrderResponse> CompleteAsync(int id);
        Task<WorkOrderResponse> CancelAsync(int id);
    }
}
