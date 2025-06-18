using System.ComponentModel.DataAnnotations;
using Customization_Management_API.Domain.Entities;

namespace Customization_Management_API.Application.DTOs;

public class CreateCustomizationRequestDto
{
    public required Guid UnitId{ get; set; }
    [MinLength(1, ErrorMessage = "At least one customization must be selected")]
    public required List<Guid> CustomizationIds{ get; set; }
}

public class CustomizationRequestResponseDto
{
    public Guid Id{ get; set; }
    public Guid UnitId{ get; set; }
    public string UnitNumber{ get; set; }
    public string ClientName{ get; set; }
    public List<CustomizationResponseDto> Customizations{ get; set; }
    public decimal TotalValue{ get; set; }
    public RequestStatus Status{ get; set; }
    public DateTime CreatedAt{ get; set; }
}

public class UpdateRequestStatusDto
{
    public required RequestStatus Status{ get; set; }
} 