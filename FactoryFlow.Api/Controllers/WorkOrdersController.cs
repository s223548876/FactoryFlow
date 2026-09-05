using FactoryFlow.Api.Dtos;
using FactoryFlow.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactoryFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrdersController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpPost]
        public async Task<ActionResult<WorkOrderResponse>> Create(CreateWorkOrderRequest request)
        {
            WorkOrderResponse workOrder = await _workOrderService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = workOrder.Id },
                workOrder);
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkOrderResponse>>> GetAll()
        {
            List<WorkOrderResponse> workOrders = await _workOrderService.GetAllAsync();
            return Ok(workOrders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WorkOrderResponse>> GetById(int id)
        {
            WorkOrderResponse? workOrder = await _workOrderService.GetByIdAsync(id);

            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
        }

        [HttpPost("{id:int}/start")]
        public async Task<ActionResult<WorkOrderResponse>> Start(int id)
        {
            WorkOrderResponse workOrder = await _workOrderService.StartAsync(id);
            return Ok(workOrder);
        }

        [HttpPost("{id:int}/complete")]
        public async Task<ActionResult<WorkOrderResponse>> Complete(int id)
        {
            WorkOrderResponse workOrder = await _workOrderService.CompleteAsync(id);
            return Ok(workOrder);
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<ActionResult<WorkOrderResponse>> Cancel(int id)
        {
            WorkOrderResponse workOrder = await _workOrderService.CancelAsync(id);
            return Ok(workOrder);
        }
    }
}
