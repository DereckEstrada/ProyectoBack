using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoSoftware.Back.BE.Models;
using ProyectoSoftware.Back.BE.Utilitarian;
using ProyectoSoftware.Back.BL.Interfaces;
using ProyectoSoftware.Back.BL.Services;
using ProyectoSoftware.Back.DAL.Interfaces;
using ProyectoSoftware.Back.DAL.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
         "MyAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddDbContext<ProyectoSoftwareDbContext>((options) => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<ISexRepository, SexRepository>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IPersonServices, PersonServices>();
builder.Services.AddScoped<IRolServices, RolServices>();
builder.Services.AddScoped<ISexServices, SexServices>();
builder.Services.AddScoped<IEmailServices, EmailServices>();

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}

    ).AddJwtBearer(options =>
    {
        options.MapInboundClaims = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["keyJWT"] ?? throw new Exception("Sin Key JWT"))),
            ClockSkew = TimeSpan.Zero
        };

    });

builder.Services.AddAuthorizationBuilder()
.AddPolicy("Admin", policy =>
{
    policy.RequireRole("ADMIN");
});
builder.Services.Configure<EmailCredential>(builder.Configuration.GetSection(EmailCredential.Credential));

var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
