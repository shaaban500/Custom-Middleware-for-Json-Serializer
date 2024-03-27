using testAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddControllersWithViews()
//                .AddJsonOptions(options =>
//                {
//                    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
//                });
builder.Services.AddScoped<JsonMiddleware>();


var app = builder.Build();
app.UseMiddleware<JsonMiddleware>();

//app.UseMiddleware<DateTimeMiddleware>();
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
