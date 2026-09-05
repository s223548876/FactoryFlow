namespace FactoryFlow.Api.Models
{
    public enum MachineStatus
    {
        Idle,           // 0
        Running,        // 1
        Maintenance,    // 2
        Offline         // 3
    }

    public class Machine
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MachineStatus Status { get; private set; } = MachineStatus.Idle;
        public List<WorkOrder> WorkOrders { get; set; } = new();

        private Machine()
        {
        }

        public Machine(string code, string name)
        {
            Name = name;
            Code = code;
            Status = MachineStatus.Idle;
        }

        // 只有閒置中的機台才能啟動
        public void Start()
        {
            if (Status != MachineStatus.Idle)
            {
                throw new InvalidOperationException("只有 Idle 狀態的機台才能啟動");
            }
            Status = MachineStatus.Running;
        }

        // 機台必須運作中，才能停止
        public void Stop()
        {
            if (Status != MachineStatus.Running)
            {
                throw new InvalidOperationException("只有 Running 的機台才能 Stop");
            }
            Status = MachineStatus.Idle;
        }

        // 只有 Idle 狀態的機台才能進入維護
        public void SendToMaintenance()
        {
            if (Status != MachineStatus.Idle)
            {
                throw new InvalidOperationException("只有 Idle 狀態的機台才能進入維護");
            }
            Status = MachineStatus.Maintenance;
        }
    }
}
