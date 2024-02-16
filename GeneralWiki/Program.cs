using System.Text.Json.Serialization;
using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectString = builder.Configuration.GetConnectionString("WikiContext");
builder.Services.AddControllers().AddJsonOptions(o=>
    o.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddDbContext<WikiContext>(o =>
    o.UseNpgsql(connectString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<WikiContext>();

builder.Services.AddScoped<IUserDataProvider, UserDataProvider>();
builder.Services.AddScoped<IEntryDataProvider, EntryDataProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
