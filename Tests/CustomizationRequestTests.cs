using System;
using System.Collections.Generic;
using System.Linq;
using Customization_Management_API.Domain.Entities;
using Xunit;

namespace Customization_Management_API.Tests;

public class CustomizationRequestTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties_Correctly_When_Given_Valid_Data()
    {
        var unitId              = Guid.NewGuid();
        var createdBy           = Guid.NewGuid();
        var customizations      = new List<Customization>
        {
            new Customization( "Hardwood Floor", "Oak finish", CustomizationType.Finishing, 1500.00m, Guid.NewGuid() ),
            new Customization( "Extra Power Outlet", "220V outlet", CustomizationType.Electrical, 250.50m, Guid.NewGuid() )
        };


        var expectedTotalValue = customizations.Sum(c => c.Price);
        var request            = new CustomizationRequest( unitId, customizations, createdBy );

        Assert.NotEqual( Guid.Empty, request.Id );
        Assert.Equal( unitId, request.UnitId );
        Assert.Equal( customizations.Count, request.Customizations.Count );
        Assert.Equal( expectedTotalValue, request.TotalValue );
        Assert.Equal( RequestStatus.UnderReview, request.Status );
        Assert.Equal( createdBy, request.CreatedBy );
        Assert.True( request.CreatedAt <= DateTime.UtcNow && request.CreatedAt > DateTime.UtcNow.AddMinutes(-1) );
    }

    [Fact]
    public void Constructor_Should_Throw_ArgumentException_When_Customizations_List_Is_Null()
    {
        var unitId                          = Guid.NewGuid();
        var createdBy                       = Guid.NewGuid();
        List<Customization> customizations  = null;

        var exception = Assert.Throws<ArgumentException>(() => new CustomizationRequest(unitId, customizations, createdBy));
        Assert.Equal("customizations", exception.ParamName);
    }

    [Fact]
    public void Constructor_Should_Throw_ArgumentException_When_Customizations_List_Is_Empty()
    {
        var unitId          = Guid.NewGuid();
        var createdBy       = Guid.NewGuid();
        var customizations  = new List<Customization>();

        var exception       = Assert.Throws<ArgumentException>(() => new CustomizationRequest(unitId, customizations, createdBy));
        Assert.Equal("customizations", exception.ParamName);
    }
} 