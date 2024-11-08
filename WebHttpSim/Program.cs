var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

int reply = 0; // Cooperate

app.MapPost("/NextIteration", () => reply);

app.MapPost("/Reset", () => { reply = 0; });

app.MapPost("/SetResult", (ItResult result) =>
{
    if (result.opponent == 1)
    {
        reply = 1; // Defect
    }
});

app.Run();

record ItResult(int player, int opponent);