using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Service;

var builder = WebApplication.CreateBuilder(args);

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
