using FactoryFlow.Api.Models;

namespace FactoryFlow.Api.Dtos
{
    public class MachineResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public MachineStatus Status { get; set; }
    }
}
