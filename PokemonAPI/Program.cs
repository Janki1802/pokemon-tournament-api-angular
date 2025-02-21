
using PokemonAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PokemonService>();
builder.Services.AddLogging();
builder.Services.AddHttpClient();

var app = builder.Build();

// Use the CORS
app.UseCors(x => x.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()      // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();
