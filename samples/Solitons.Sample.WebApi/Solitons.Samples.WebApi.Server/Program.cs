using Solitons.Samples.WebApi.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var databaseApi = new SampleDatabaseWebApi();
builder.Services.AddSingleton(databaseApi);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Map("/{*rest}", async (SampleDatabaseWebApi api, HttpContext context) =>
{
    await api.ProcessAsync(context);
});


app.Run();


