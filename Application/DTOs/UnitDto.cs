using System.ComponentModel.DataAnnotations;

namespace Customization_Management_API.Application.DTOs;

public class CreateUnitDto
{
    public required string DevelopmentName{ get; set; }
    public required string UnitNumber{ get; set; }
    public required string ClientName{ get; set; }
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF must contain exactly 11 digits")]
    public required string ClientCPF{ get; set; }
}

public class UnitResponseDto
{
    public Guid Id{ get; set; }
    public string DevelopmentName{ get; set; }
    public string UnitNumber{ get; set; }
    public string ClientName{ get; set; }
    public string ClientCPF{ get; set; }
    public DateTime CreatedAt{ get; set; }
} 