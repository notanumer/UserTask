using Core;
using Microsoft.EntityFrameworkCore;
using Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Logic.Interfaces;
using Logic;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"]))
        };
    });

builder.Services.AddControllers(opt => opt.Conventions.Add(new NamespaceRoutingConvention()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper();
var settings = new ConnectionSetting(builder.Configuration);
builder.Services.AddTransient(_ => settings);
builder.Services.AddDbContext<Context>(opt => opt.UseSqlServer(settings.ConnectionString));
builder.Services.AddScoped<IUserRepository, AccountManager>();
builder.Services.AddScoped<IJwtGenerator, AccountManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
