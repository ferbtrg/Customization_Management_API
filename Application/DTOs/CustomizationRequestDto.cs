using System.ComponentModel.DataAnnotations;
using Customization_Management_API.Domain.Entities;

namespace Customization_Management_API.Application.DTOs;

public class CreateCustomizationRequestDto
{
    public required Guid UnitId{ get; set; }
    [MinLength(1, ErrorMessage = "At least one customization must be selected")]
    public required ICollection<Guid> CustomizationIds{ get; set; }
}

public class CustomizationRequestResponseDto
{
    public Guid Id{ get; set; }
    public Guid UnitId{ get; set; }
    public string UnitNumber{ get; set; } = string.Empty;
    public string ClientName{ get; set; } = string.Empty;
    public ICollection<CustomizationResponseDto> Customizations{ get; set; } = new List<CustomizationResponseDto>();
    public decimal TotalValue{ get; set; }
    public RequestStatus Status{ get; set; }
    public DateTime CreatedAt{ get; set; }
}

public class UpdateRequestStatusDto
{
    public required RequestStatus Status{ get; set; }
} 