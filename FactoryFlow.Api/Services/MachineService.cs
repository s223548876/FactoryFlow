using FactoryFlow.Api.Data;
using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryFlow.Api.Services
{
    public class MachineService : IMachineService
    {
        private readonly AppDbContext _dbContext;

        public MachineService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MachineResponse>> GetAllAsync()
        {
            return await _dbContext.Machines
                .AsNoTracking()
                .OrderBy(x => x.Code)
                .Select(x => ToResponse(x))
                .ToListAsync();
        }

        public async Task<MachineResponse?> GetByCodeAsync(string code)
        {
            Machine? machine = await _dbContext.Machines
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);

            return machine == null ? null : ToResponse(machine);
        }

        public async Task<MachineResponse> CreateAsync(CreateMachineRequest request)
        {
            // 1. 檢查 code 有沒有重複
            bool exists = await _dbContext.Machines
                .AnyAsync(x => x.Code == request.Code);

            // 2. 重複就丟 Exception
            if (exists)
            {
                throw new InvalidOperationException("Machine code 已存在");
            }

            // 3. 建立 Machine
            Machine machine = new Machine(request.Code, request.Name);

            // 4. 加進 DbContext，並存進資料庫
            _dbContext.Machines.Add(machine);
            await _dbContext.SaveChangesAsync();

            // 5. 回傳 Machine
            return ToResponse(machine);
        }

        public async Task<MachineResponse> StartAsync(string code)
        {
            // 1. 找到 Machine
            Machine? machine = await FindTrackedMachineAsync(code);
            if (machine == null)
            {
                throw new KeyNotFoundException("找不到指定的 Machine");
            }

            // 2. 呼叫 Start()
            machine.Start();

            // 3. 儲存狀態變更並回傳 Machine
            await _dbContext.SaveChangesAsync();
            return ToResponse(machine);
        }

        public async Task<MachineResponse> StopAsync(string code)
        {
            // 找到 Machine
            Machine? machine = await FindTrackedMachineAsync(code);
            if (machine == null)
            {
                throw new KeyNotFoundException("找不到指定的 Machine");
            }

            machine.Stop();
            await _dbContext.SaveChangesAsync();
            return ToResponse(machine);
        }

        public async Task<MachineResponse> MaintenanceAsync(string code)
        {
            // 找到 Machine
            Machine? machine = await FindTrackedMachineAsync(code);
            if (machine == null)
            {
                throw new KeyNotFoundException("找不到指定的 Machine");
            }

            machine.SendToMaintenance();
            await _dbContext.SaveChangesAsync();
            return ToResponse(machine);
        }

        private async Task<Machine?> FindTrackedMachineAsync(string code)
        {
            return await _dbContext.Machines
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        private static MachineResponse ToResponse(Machine machine)
        {
            return new MachineResponse
            {
                Id = machine.Id,
                Code = machine.Code,
                Name = machine.Name,
                Status = machine.Status
            };
        }
    }
}
