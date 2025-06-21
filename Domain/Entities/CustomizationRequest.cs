using System;
using System.Collections.Generic;

namespace Customization_Management_API.Domain.Entities;

/// <summary>
/// Represents a client's request for customizations to their unit.
/// This entity tracks the status of the request, associated customizations,
/// and calculates the total value of all requested changes.
/// </summary>
public class CustomizationRequest
{
    public Guid Id{ get; private set; }
    public Guid UnitId{ get; private set; }
    public Unit Unit{ get; private set; }
    public ICollection<Customization> Customizations{ get; private set; }
    public decimal TotalValue{ get; private set; }
    public RequestStatus Status{ get; private set; }
    public Guid CreatedBy{ get; private set; }
    public DateTime CreatedAt{ get; private set; }

    private CustomizationRequest()
    {
        Unit                = null!;
        Customizations      = new List<Customization>();
    }

    /// <summary>
    /// Creates a new customization request for a unit
    /// </summary>
    public CustomizationRequest( Guid unitId, ICollection<Customization> customizations, Guid createdBy )
    {
        if( customizations == null || !customizations.Any() )
            throw new ArgumentException( "At least one customization must be provided", nameof( customizations ) );

        Id              = Guid.NewGuid();
        UnitId          = unitId;
        Customizations  = customizations;
        TotalValue      = customizations.Sum(c => c.Price);
        Status          = RequestStatus.UnderReview;
        CreatedBy       = createdBy;
        CreatedAt       = DateTime.UtcNow;
    }

    public void UpdateStatus( RequestStatus newStatus )
    {
        Status = newStatus;
    }
}

/// <summary>
/// Defines the possible states of a customization request
/// </summary>
public enum RequestStatus
{
    UnderReview,
    Approved,
    Rejected
} 