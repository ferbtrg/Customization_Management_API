using System;

namespace Customization_Management_API.Domain.Entities;

/// <summary>
/// Represents a customization option that can be applied to a unit.
/// This entity defines the available modifications that clients can request for their units,
/// including details like type, description, and price.
/// </summary>
public class Customization
{
    public Guid Id{ get; private set; }

    /// <summary>
    /// Name of the customization option (must be unique)
    /// </summary>
    public string Name{ get; private set; }

    /// <summary>
    /// Detailed description of what the customization entails
    /// </summary>
    public string Description{ get; private set; }

    /// <summary>
    /// Category of the customization (e.g., Finishing, Structural)
    /// </summary>
    public CustomizationType Type{ get; private set; }
    public decimal Price{ get; private set; }

    /// <summary>
    /// ID of the admin who created this customization option
    /// </summary>
    public Guid CreatedBy{ get; private set; }
    public DateTime CreatedAt{ get; private set; }
    public ICollection<CustomizationRequest> CustomizationRequests{ get; private set; }

    private Customization()
    {
    }

    /// <summary>
    /// Creates a new customization option with the specified details
    /// </summary>
    public Customization(string name, string description, CustomizationType type, decimal price, Guid createdBy)
    {
        Id                      = Guid.NewGuid();
        Name                    = name;
        Description             = description;
        Type                    = type;
        Price                   = price;
        CreatedBy               = createdBy;
        CreatedAt               = DateTime.UtcNow;
        CustomizationRequests   = new List<CustomizationRequest>();
    }
}

/// <summary>
/// Defines the possible categories of customization options available in the system
/// </summary>
public enum CustomizationType
{
    /// <summary>
    /// Modifications to surface finishes, materials, and aesthetics
    /// </summary>
    Finishing,

    /// <summary>
    /// Changes to the building's structure or layout
    /// </summary>
    Structural,

    /// <summary>
    /// Modifications to electrical systems and installations
    /// </summary>
    Electrical,

    /// <summary>
    /// Changes to plumbing systems and installations
    /// </summary>
    Plumbing,

    /// <summary>
    /// Any other type of customization not covered by the above categories
    /// </summary>
    Other
} 