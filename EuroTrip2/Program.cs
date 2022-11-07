using EuroTrip2.Contexts;
using Microsoft.EntityFrameworkCore;
using EuroTrip2.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //options.Authority = "https://localhost:5054/";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwtConfig:Issuer"],
        ValidAudience = builder.Configuration["jwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
        (builder.Configuration["jwtConfig:Key"]))
    };
});


builder.Services.AddRazorPages();
builder.Services.AddMvc();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

//manual --cors added here
app.MapControllers();
app.MapRazorPages();
app.UseCors("AllowOrigin");
//app.MapBookingEndpoints();

//app.UseStaticFiles();

app.UseHttpsRedirection();

app.Run();
