using System.Xml.Linq;
using Customization_Management_API.Application.DTOs;
using Customization_Management_API.Domain.Entities;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customization_Management_API.Controllers;

/// <summary>
/// Controller responsible for managing customization options.
/// This controller provides endpoints for creating and retrieving customization options.
/// Some endpoints are public while others require admin authorization.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomizationsController : ControllerBase
{
    private readonly UserDbContext _context;
    public CustomizationsController(UserDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new customization option in the system
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CustomizationResponseDto>> CreateCustomization( CreateCustomizationDto createCustomizationDto )
    {
        //Extract and validate user ID from the JWT token
        var nameIdentifier = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Guid userId;
        if( string.IsNullOrWhiteSpace(nameIdentifier) || !Guid.TryParse( nameIdentifier, out userId ) )
            return Unauthorized( "User ID not found or invalid." );

        var customization = new Customization( createCustomizationDto.Name, createCustomizationDto.Description, createCustomizationDto.Type,
            createCustomizationDto.Price, userId );

        //Save to database
        _context.Customizations.Add( customization );
        await _context.SaveChangesAsync();

        var customResponse          = new CustomizationResponseDto();
        customResponse.Id           = customization.Id;
        customResponse.Name         = customization.Name;
        customResponse.Description  = customization.Description;
        customResponse.Type         = customization.Type;
        customResponse.Price        = customization.Price;
        customResponse.CreatedAt    = customization.CreatedAt;

        return Ok( customResponse );
    }

    /// <summary>
    /// Retrieves all customization options, optionally filtered by type
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CustomizationResponseDto>>> GetCustomizations( [FromQuery] CustomizationType? type )
    {
        //Build query with optional type filter
        var query = _context.Customizations.AsQueryable();
        if(type.HasValue)
            query = query.Where(c => c.Type == type.Value);

        //Query and map customizations to response DTOs
        var customizations = await query
            .Select(c => new CustomizationResponseDto
            {
                Id          = c.Id,
                Name        = c.Name,
                Description = c.Description,
                Type        = c.Type,
                Price       = c.Price,
                CreatedAt   = c.CreatedAt
            })
            .ToListAsync();

        return Ok( customizations );
    }

    /// <summary>
    /// Retrieves a specific customization option by its ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CustomizationResponseDto>> GetCustomization( Guid id )
    {
        //Find customization by ID
        var customization = await _context.Customizations.FindAsync(id);
        if( customization == null )
            return NotFound();

        var customResponse          = new CustomizationResponseDto();
        customResponse.Id           = customization.Id;
        customResponse.Name         = customization.Name;
        customResponse.Description  = customization.Description;
        customResponse.Type         = customization.Type;
        customResponse.Price        = customization.Price;
        customResponse.CreatedAt    = customization.CreatedAt;

        return Ok( customResponse );
    }
} 