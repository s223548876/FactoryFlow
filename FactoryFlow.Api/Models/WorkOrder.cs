namespace FactoryFlow.Api.Models
{
    public enum WorkOrderStatus
    {
        Open,
        InProgress,
        Completed,
        Cancelled
    }

    public class WorkOrder
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public Machine Machine { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public WorkOrderStatus Status { get; private set; } = WorkOrderStatus.Open;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; private set; }

        private WorkOrder()
        {
        }

        public WorkOrder(int machineId, string title, string description)
        {
            MachineId = machineId;
            Title = title;
            Description = description;
            Status = WorkOrderStatus.Open;
            CreatedAt = DateTime.UtcNow;
        }

        public void Start()
        {
            if (Status != WorkOrderStatus.Open)
            {
                throw new InvalidOperationException("只有 Open 狀態的 Work Order 可以開始");
            }

            Status = WorkOrderStatus.InProgress;
        }

        public void Complete()
        {
            if (Status != WorkOrderStatus.InProgress)
            {
                throw new InvalidOperationException("只有 InProgress 狀態的 Work Order 可以完成");
            }

            Status = WorkOrderStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status is WorkOrderStatus.Completed or WorkOrderStatus.Cancelled)
            {
                throw new InvalidOperationException("Completed 或 Cancelled 的 Work Order 不可再轉換狀態");
            }

            Status = WorkOrderStatus.Cancelled;
        }
    }
}
