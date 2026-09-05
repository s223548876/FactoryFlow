using System.ComponentModel.DataAnnotations;

namespace FactoryFlow.Api.Dtos
{
    public class CreateWorkOrderRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MachineId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
