using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineExaminationSystem_Back_End_DAL.DbContexts;
using System.Text;

//Program Start---------->

//Enble CORS For React -->Gourav
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//CORS Middleware Handles-->Gourav
//Enable Cross-Origin Requests (CORS)-->Gourav
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("http://localhost:9875")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
});

// Add services to the container
builder.Services.AddControllers();

//Connection to data base--->Gourav
builder.Services.AddDbContext<DatabaseContext>(options =>
    {
        options.UseSqlServer(builder.Configuration
            .GetConnectionString("ConnString"), b => b.MigrationsAssembly("OnlineExaminationSystem_Back_End"));
    });

//JWT Code for this project-->Gourav
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        };
    });

//Automapper---->Gourav
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.UseHttpsRedirection();
//---->Gourav
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
//above by Gourav
app.UseAuthorization();

app.MapControllers();

app.Run();
