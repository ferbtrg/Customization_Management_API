using System;

namespace Customization_Management_API.Domain.Entities;

/// <summary>
/// Represents a real estate unit that has been sold to a client.
/// This entity is the core of the customization management system, as all customization requests
/// must be associated with a valid unit.
/// </summary>
public class Unit
{
    public Guid Id{ get; private set; }
    public string DevelopmentName{ get; private set; }
    public string UnitNumber{ get; private set; }
    public string ClientName{ get; private set; }
    public string ClientCPF{ get; private set; }
    public Guid CreatedBy{ get; private set; }
    public DateTime CreatedAt{ get; private set; }
    public ICollection<CustomizationRequest> CustomizationRequests{ get; private set; }

    private Unit()
    {

    }

    /// <summary>
    /// Creates a new unit with the specified details
    /// </summary>
    public Unit( string developmentName, string unitNumber, string clientName, string clientCPF, Guid createdBy )
    {
        Id                      = Guid.NewGuid();
        DevelopmentName         = developmentName;
        UnitNumber              = unitNumber;
        ClientName              = clientName;
        ClientCPF               = clientCPF;
        CreatedBy               = createdBy;
        CreatedAt               = DateTime.UtcNow;
        CustomizationRequests   = new List<CustomizationRequest>();
    }
} 