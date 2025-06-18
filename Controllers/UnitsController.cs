using Customization_Management_API.Application.DTOs;
using Customization_Management_API.Domain.Entities;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Customization_Management_API.Controllers;

/// <summary>
/// Controller responsible for managing real estate units.
/// This controller provides endpoints for creating and retrieving unit information.
/// All endpoints require admin role authorization.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UnitsController : ControllerBase
{
    private readonly UserDbContext _context;
    public UnitsController(UserDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new unit in the system
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UnitResponseDto>> CreateUnit( CreateUnitDto createUnitDto )
    {
        var nameIdentifier = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Guid userId;
        if( string.IsNullOrWhiteSpace(nameIdentifier) || !Guid.TryParse(nameIdentifier, out userId ) )
            return Unauthorized( "User ID not found or invalid." );

        var unit = new Unit( createUnitDto.DevelopmentName, createUnitDto.UnitNumber, createUnitDto.ClientName,
            createUnitDto.ClientCPF, userId );

        //Save to database
        _context.Units.Add(unit);
        await _context.SaveChangesAsync();

        var unitResponse                = new UnitResponseDto();
        unitResponse.Id                 = unit.Id;
        unitResponse.DevelopmentName    = unit.DevelopmentName;
        unitResponse.UnitNumber         = unit.UnitNumber;
        unitResponse.ClientName         = unit.ClientName;
        unitResponse.ClientCPF          = unit.ClientCPF;
        unitResponse.CreatedAt          = unit.CreatedAt;

        return Ok( unitResponse );
    }

    /// <summary>
    /// Retrieves all units in the system
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UnitResponseDto>>> GetUnits()
    {
        // Query and map all units to response DTOs
        var units = await _context.Units
            .Select(u => new UnitResponseDto
            {
                Id                  = u.Id,
                DevelopmentName     = u.DevelopmentName,
                UnitNumber          = u.UnitNumber,
                ClientName          = u.ClientName,
                ClientCPF           = u.ClientCPF,
                CreatedAt           = u.CreatedAt
            })
            .ToListAsync();

        return Ok( units );
    }

    /// <summary>
    /// Retrieves a specific unit by its ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UnitResponseDto>> GetUnit( Guid id )
    {
        var unit = await _context.Units.FindAsync(id);
        if( unit == null )
            return NotFound();

        var unitResponse                    = new UnitResponseDto();
        unitResponse.Id                     = unit.Id;
        unitResponse.DevelopmentName        = unit.DevelopmentName;
        unitResponse.UnitNumber             = unit.UnitNumber;
        unitResponse.ClientName             = unit.ClientName;
        unitResponse.ClientCPF              = unit.ClientCPF;
        unitResponse.CreatedAt              = unit.CreatedAt;

        return Ok( unitResponse );
    }
} 