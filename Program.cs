using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using trivia_backend.Context;
using trivia_backend.Services;

var builder = WebApplication.CreateBuilder(args);


// REMEMBER TO CREATE AND CONNECT THE WEB APP


// Add services to the container.
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<QuizServices>();
builder.Services.AddSingleton<BlobServices>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));




var secretKey = builder.Configuration["Jwt:Key"] ?? "superSecretKey@345";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // THis is setting our authentication to know what to expect and check to see if our token is valid
        // These options are defining what is valid in our token as well, and should correlate to the options that we set upon generating our token
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // THis is a list of all the places a token should be allowed to get generated from
        ValidIssuers = new[]
        {
            // remember to change these links to the actual webaddress of your API
            "trivia-api-g3d7dwczhma0hzdt.westus-01.azurewebsites.net"
        },
        // This is a list of all the places a token should be allowed to get used
        ValidAudiences = new[]
        {
            "trivia-api-g3d7dwczhma0hzdt.westus-01.azurewebsites.net"
        },
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) // Secret key
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
