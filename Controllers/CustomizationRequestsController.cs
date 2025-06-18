using Customization_Management_API.Application.DTOs;
using Customization_Management_API.Domain.Entities;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customization_Management_API.Controllers;

/// <summary>
/// Controller responsible for managing customization requests from clients.
/// This controller handles the creation and management of customization requests for units,
/// including status updates and retrieval of request information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomizationRequestsController : ControllerBase
{
    private readonly UserDbContext _context;
    public CustomizationRequestsController(UserDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new customization request for a unit
    /// - 200 OK with the created request if successful
    /// - 401 Unauthorized if user ID is invalid
    /// - 404 Not Found if unit doesn't exist
    /// - 400 Bad Request if a request already exists for the unit or if customizations are invalid
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CustomizationRequestResponseDto>> CreateRequest(CreateCustomizationRequestDto createRequestDto)
    {
        var nameIdentifier = User.FindFirst( System.Security.Claims.ClaimTypes.NameIdentifier )?.Value;
        Guid userId;
        if( string.IsNullOrWhiteSpace( nameIdentifier ) || !Guid.TryParse( nameIdentifier, out userId ) )
            return Unauthorized( "User ID not found or invalid." );

        var unit = await _context.Units.FindAsync(createRequestDto.UnitId);
        if( unit == null )
            return NotFound( "Unit not found" );

        //Check for existing request
        var existingRequest = await _context.CustomizationRequests
            .AnyAsync( r => r.UnitId == createRequestDto.UnitId );

        if( existingRequest )
            return BadRequest( "A request already exists for this unit" );


        var customizations = await _context.Customizations.Where( c => createRequestDto.CustomizationIds.Contains( c.Id ) )
            .ToListAsync();

        if( customizations.Count != createRequestDto.CustomizationIds.Count )
            return BadRequest( "One or more customizations not found" );

        //Create and save the request
        var request = new CustomizationRequest( createRequestDto.UnitId, customizations, userId );
        _context.CustomizationRequests.Add( request );
        await _context.SaveChangesAsync();

        // Map entity to response DTO
        var customRequestResponse               = new CustomizationRequestResponseDto();
        customRequestResponse.Id                = request.Id;
        customRequestResponse.UnitId            = request.UnitId;
        customRequestResponse.UnitNumber        = unit.UnitNumber;
        customRequestResponse.ClientName        = unit.ClientName;
        customRequestResponse.Customizations    = customizations.Select(c => new CustomizationResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Type = c.Type,
            Price = c.Price,
            CreatedAt = c.CreatedAt
        }).ToList();
        customRequestResponse.TotalValue        = request.TotalValue;
        customRequestResponse.Status            = request.Status;
        customRequestResponse.CreatedAt         = request.CreatedAt;

        return Ok( customRequestResponse );
    }

    /// <summary>
    /// Retrieves all customization requests for a specific unit
    /// </summary>
    [HttpGet("unit/{unitId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<CustomizationRequestResponseDto>>> GetRequestsByUnit( Guid unitId )
    {
        // Query requests with related data
        var requests = await _context.CustomizationRequests
            .Include(r => r.Unit)
            .Include(r => r.Customizations)
            .Where(r => r.UnitId == unitId)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new CustomizationRequestResponseDto
            {
                Id              = r.Id,
                UnitId          = r.UnitId,
                UnitNumber      = r.Unit.UnitNumber,
                ClientName      = r.Unit.ClientName,
                Customizations  = r.Customizations.Select(c => new CustomizationResponseDto
                {
                    Id          = c.Id,
                    Name        = c.Name,
                    Description = c.Description,
                    Type        = c.Type,
                    Price       = c.Price,
                    CreatedAt   = c.CreatedAt
                }).ToList(),
                TotalValue      = r.TotalValue,
                Status          = r.Status,
                CreatedAt       = r.CreatedAt
            })
            .ToListAsync();

        return Ok( requests );
    }

    /// <summary>
    /// Updates the status of a customization request
    /// - 204 No Content if update was successful
    /// - 404 Not Found if request doesn't exist
    /// - 401 Unauthorized if user is not an admin
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateRequestStatus( Guid id, [FromBody] UpdateRequestStatusDto updateStatusDto )
    {
        // Find and update request status
        var request = await _context.CustomizationRequests.FindAsync( id );
        if( request == null )
            return NotFound();

        request.UpdateStatus( updateStatusDto.Status );
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 