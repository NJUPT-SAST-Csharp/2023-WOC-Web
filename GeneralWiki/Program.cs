using System.Text.Json.Serialization;
using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// JWT配置
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("这里是一个非常安全的密钥"));//JWT密钥
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = securityKey,
    ValidateIssuer = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });
var connectString = builder.Configuration.GetConnectionString("WikiContext");
builder.Services.AddControllers().AddJsonOptions(o=>
    o.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<WikiContext>(o =>
    o.UseNpgsql(connectString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<WikiContext>();

builder.Services.AddScoped<IUserDataProvider,UserDataProvider>();
builder.Services.AddScoped<IEntryDataProvider, EntryDataProvider>();
builder.Services.AddScoped<IPictureProvider,PictureProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();