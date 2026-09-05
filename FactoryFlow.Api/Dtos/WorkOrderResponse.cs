using FactoryFlow.Api.Models;

namespace FactoryFlow.Api.Dtos
{
    public class WorkOrderResponse
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string MachineCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public WorkOrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
