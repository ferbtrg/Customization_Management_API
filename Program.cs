using Customization_Management_API.Application.Services;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Customization_Management_API.Filters;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Customization Management API", Version = "v1" });

    //Add filters to document answers to 401 and 403.
    c.OperationFilter<AuthResponsesOperationFilter>();

    //Define the JWT security configuration for Swagger.
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name                = "Authorization",
        Type                = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme              = "Bearer",
        BearerFormat        = "JWT",
        In                  = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description         = "Enter JWT in the format: Bearer {your_token}"
    });

    // This enforces the security definition globally. It tells Swagger that every endpoint
    // in this API requires the 'Bearer' security.
    // As a result, a lock icon will be displayed next to every operation, and the
    // JWT token from the 'Authorize' button will be sent in the 'Authorization' header.
    c.AddSecurityRequirement( new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type        = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id          = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<UserDbContext>( options =>
    options.UseSqlite( builder.Configuration.GetConnectionString("UserDatabase") ) );

//Check if connection with SQLite works.
try
{
    var connectionString = builder.Configuration.GetConnectionString("UserDatabase");
    if( string.IsNullOrEmpty( connectionString ) )
        throw new InvalidOperationException("Connection string 'UserDatabase' not found in appsettings.json");

    using( var connection = new SqliteConnection( connectionString ) )
    {
        connection.Open();
        Console.WriteLine( "SQLite connection validated successfully." );
        Console.WriteLine( string.Format( "Database file: {0}", Path.GetFullPath("CustomizationManagement.db") ) );
    }
}
catch (Exception ex)
{
    Console.WriteLine(string.Format("Error validating SQLite connection: {0}", ex.Message));
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer              = true,
            ValidIssuer                 = builder.Configuration["Jwt:Issuer"],
            ValidateAudience            = true,
            ValidAudience               = builder.Configuration["Jwt:Audience"],
            ValidateLifetime            = true,
            IssuerSigningKey            = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey    = true
        };
    });

builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
