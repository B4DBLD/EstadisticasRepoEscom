using EstadisticasRepoEscom.Conexion;
using EstadisticasRepoEscom.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbConfig = new DBConfig
{
    ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
};
builder.Services.AddSingleton(dbConfig);

builder.Services.AddScoped<IRepositorioEstadisticas, RepositorioEstadisticas>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.Urls.Add("http://10.0.0.4:8082");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
