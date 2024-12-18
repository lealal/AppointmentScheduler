using AppointmentScheduler.Configuration;
using AppointmentScheduler.Data;
using AppointmentScheduler.Logging;
using AppointmentScheduler.Managers;
using AppointmentScheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is undefined");
}

if (jwtSettings == null)
{
    throw new InvalidOperationException("Jwt settings are undefined");
}

builder.Logging.AddProvider(new SqlLoggerProvider(connectionString));

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience
        };

        options.RequireHttpsMetadata = false;
    });

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<TestService>();
builder.Services.AddScoped<TestManager>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AppointmentManager>();

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppointmentScheduler API v1");
        c.RoutePrefix = string.Empty; // Set Swagger as the default route
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
