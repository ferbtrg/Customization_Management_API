using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Customization_Management_API.Application.DTOs;

public class CreateUnitDto
{
    public required string DevelopmentName{ get; set; }
    public required string UnitNumber{ get; set; }
    public required string ClientName{ get; set; }

    private string _clientCpf = string.Empty;

    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF must contain 11 digits.")]
    public required string ClientCPF
    {
        get =>  _clientCpf;
        set =>  _clientCpf = new string( value.Where( char.IsDigit ).ToArray() );
    }
}

public class UnitResponseDto
{
    public Guid Id{ get; set; }
    public string DevelopmentName{ get; set; }  = string.Empty;
    public string UnitNumber{ get; set; }       = string.Empty;
    public string ClientName{ get; set; }       = string.Empty;
    public string ClientCPF{ get; set; }        = string.Empty;
    public DateTime CreatedAt{ get; set; }
} 