using EuroTrip2.Contexts;
using Microsoft.EntityFrameworkCore;
using EuroTrip2.Controllers;
using EuroTrip2.BussinessLayer;

var builder = WebApplication.CreateBuilder(args);
//To enable cors from here
builder.Services.AddCors(options =>
{

    options.AddPolicy(

    name: "AllowOrigin",

    builder => {

        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

    });

});
// cors ends here

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<FlightDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookingInterface,BookingRepository>();
builder.Services.AddScoped<IGeneralInterface,GeneralRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//manual --cors added here

app.UseCors("AllowOrigin");
//app.MapBookingEndpoints();

//app.UseStaticFiles();


app.Run();
