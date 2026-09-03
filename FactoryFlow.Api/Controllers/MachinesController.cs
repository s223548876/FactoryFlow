using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachinesController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MachineResponse>>> GetAll()
        {
            List<MachineResponse> machines = await _machineService.GetAllAsync();
            return Ok(machines);
            // ASP.NET Core 會自動把 List<MachineResponse> 序列化成 JSON
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<MachineResponse>> GetByCode(string code)
        {
            MachineResponse? machine = await _machineService.GetByCodeAsync(code);

            if (machine == null)
            {
                return NotFound();
            }

            return Ok(machine);
        }

        [HttpPost]
        public async Task<ActionResult<MachineResponse>> Create(CreateMachineRequest request)
        {
            try
            {
                MachineResponse machine = await _machineService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetByCode),
                    new { code = machine.Code },
                    machine
                    );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("{code}/start")]
        public async Task<ActionResult<MachineResponse>> Start(string code)
        {
            try
            {
                MachineResponse machine = await _machineService.StartAsync(code);
                return Ok(machine);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("{code}/stop")]
        public async Task<ActionResult<MachineResponse>> Stop(string code)
        {
            try
            {
                MachineResponse machine = await _machineService.StopAsync(code);
                return Ok(machine);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("{code}/maintenance")]
        public async Task<ActionResult<MachineResponse>> Maintenance(string code)
        {
            try
            {
                MachineResponse machine = await _machineService.MaintenanceAsync(code);
                return Ok(machine);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
