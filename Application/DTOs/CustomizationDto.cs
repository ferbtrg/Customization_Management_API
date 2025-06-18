using System.ComponentModel.DataAnnotations;
using Customization_Management_API.Domain.Entities;

namespace Customization_Management_API.Application.DTOs;

public class CreateCustomizationDto
{
    public required string Name{ get; set; }
    public required string Description{ get; set; }
    public required CustomizationType Type{ get; set; }
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public required decimal Price{ get; set; }
}

public class CustomizationResponseDto
{
    public Guid Id{ get; set; }
    public string Name{ get; set; }
    public string Description{ get; set; }
    public CustomizationType Type{ get; set; }
    public decimal Price{ get; set; }
    public DateTime CreatedAt{ get; set; }
} 