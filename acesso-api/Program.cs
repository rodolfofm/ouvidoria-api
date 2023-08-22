using acesso.IoC;
using acesso.Service.Autenticacao;
using acesso_api.MappingConfig;
using AutenticacaoLib.Core;
using AutenticacaoLib.Jwt;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddCors();

builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
    .WriteTo
    .Console()
    .ReadFrom.Configuration(hostContext.Configuration);
});
builder.Services.AddInfrastructure(configuration);

builder.Services.AddMemoryCache();

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddTransient<ISegurancaService, SegurancaService>();


builder.Services.AddAutoMapperConfiguration();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();


app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("authorization", "content-disposition", "content-length", "content-type", "accept", "origin")
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Iniciando Acesso API!");

    app.Run();

    Log.Information("Parado Acesso API");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

