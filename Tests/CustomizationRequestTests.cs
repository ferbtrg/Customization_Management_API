using Customization_Management_API.Domain.Entities;
using Xunit;

namespace Customization_Management_API.Tests;

public class CustomizationRequestTests
{
    [Fact]
    public void CreateCustomizationRequest_WithValidData_ShouldCreateSuccessfully()
    {
        var unitId          = Guid.NewGuid();
        var userId          = Guid.NewGuid();
        var customizations  = new List<Customization>
        {
            new Customization( "Test Customization 1", "Description 1", CustomizationType.Finishing, 1000m, userId ),
            new Customization( "Test Customization 2", "Description 2", CustomizationType.Structural, 2000m, userId )
        };

        var request = new CustomizationRequest(unitId, customizations, userId);
        Assert.Equal( unitId, request.UnitId );
        Assert.Equal( 2, request.Customizations.Count );
        Assert.Equal( 3000m, request.TotalValue );
        Assert.Equal( RequestStatus.UnderReview, request.Status );
        Assert.Equal( userId, request.CreatedBy );
    }

    [Fact]
    public void CreateCustomizationRequest_WithEmptyCustomizations_ShouldThrowException()
    {
        var unitId          = Guid.NewGuid();
        var userId          = Guid.NewGuid();
        var customizations  = new List<Customization>();

        Assert.Throws<ArgumentException>(() => new CustomizationRequest( unitId, customizations, userId ));
    }

    [Fact]
    public void UpdateStatus_ShouldChangeStatus()
    {
        var unitId              = Guid.NewGuid();
        var userId              = Guid.NewGuid();
        var customizations      = new List<Customization>
        {
            new Customization( "Test Customization", "Description", CustomizationType.Finishing, 1000m, userId )
        };
        var request = new CustomizationRequest( unitId, customizations, userId );
        request.UpdateStatus( RequestStatus.Approved );
        Assert.Equal( RequestStatus.Approved, request.Status );
    }
} 