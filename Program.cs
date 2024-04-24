using MarathonApplication.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
	options =>
{
	options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});
	options.OperationFilter<SecurityRequirementsOperationFilter>();
	
}
	);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication( x=> { 
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie( x => 
{
	x.Cookie.Name = "AccessToken";

}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience = false,
		ValidateIssuer = false,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				builder.Configuration.GetSection("JWT:Key").Value!))
	};
	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			context.Token = context.Request.Cookies["AccessToken"];
			return Task.CompletedTask;
		}
	};
});
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigins",
		builder => builder
			.AllowAnyHeader()
			.AllowAnyMethod()
			.WithOrigins("http://localhost:3000")
			.AllowCredentials());
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
