using System.ComponentModel.DataAnnotations;

namespace FactoryFlow.Api.Dtos
{
    public class CreateMachineRequest
    {
        [Required]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
