using FactoryFlow.Api.Data;
using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryFlow.Api.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly AppDbContext _dbContext;

        public WorkOrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkOrderResponse> CreateAsync(CreateWorkOrderRequest request)
        {
            bool machineExists = await _dbContext.Machines
                .AnyAsync(x => x.Id == request.MachineId);

            if (!machineExists)
            {
                throw new KeyNotFoundException("找不到指定的 Machine");
            }

            WorkOrder workOrder = new(request.MachineId, request.Title, request.Description);

            _dbContext.WorkOrders.Add(workOrder);
            await _dbContext.SaveChangesAsync();

            return await GetRequiredResponseAsync(workOrder.Id);
        }

        public async Task<List<WorkOrderResponse>> GetAllAsync()
        {
            return await _dbContext.WorkOrders
                .AsNoTracking()
                .Include(x => x.Machine)
                .OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.Id)
                .Select(x => ToResponse(x))
                .ToListAsync();
        }

        public async Task<WorkOrderResponse?> GetByIdAsync(int id)
        {
            WorkOrder? workOrder = await _dbContext.WorkOrders
                .AsNoTracking()
                .Include(x => x.Machine)
                .FirstOrDefaultAsync(x => x.Id == id);

            return workOrder == null ? null : ToResponse(workOrder);
        }

        public async Task<WorkOrderResponse> StartAsync(int id)
        {
            WorkOrder workOrder = await GetTrackedWorkOrderAsync(id);

            workOrder.Start();
            await _dbContext.SaveChangesAsync();

            return ToResponse(workOrder);
        }

        public async Task<WorkOrderResponse> CompleteAsync(int id)
        {
            WorkOrder workOrder = await GetTrackedWorkOrderAsync(id);

            workOrder.Complete();
            await _dbContext.SaveChangesAsync();

            return ToResponse(workOrder);
        }

        public async Task<WorkOrderResponse> CancelAsync(int id)
        {
            WorkOrder workOrder = await GetTrackedWorkOrderAsync(id);

            workOrder.Cancel();
            await _dbContext.SaveChangesAsync();

            return ToResponse(workOrder);
        }

        private async Task<WorkOrder> GetTrackedWorkOrderAsync(int id)
        {
            WorkOrder? workOrder = await _dbContext.WorkOrders
                .Include(x => x.Machine)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (workOrder == null)
            {
                throw new KeyNotFoundException("找不到指定的 Work Order");
            }

            return workOrder;
        }

        private async Task<WorkOrderResponse> GetRequiredResponseAsync(int id)
        {
            WorkOrderResponse? response = await GetByIdAsync(id);

            if (response == null)
            {
                throw new KeyNotFoundException("找不到指定的 Work Order");
            }

            return response;
        }

        private static WorkOrderResponse ToResponse(WorkOrder workOrder)
        {
            return new WorkOrderResponse
            {
                Id = workOrder.Id,
                MachineId = workOrder.MachineId,
                MachineCode = workOrder.Machine.Code,
                Title = workOrder.Title,
                Description = workOrder.Description,
                Status = workOrder.Status,
                CreatedAt = workOrder.CreatedAt,
                CompletedAt = workOrder.CompletedAt
            };
        }
    }
}
