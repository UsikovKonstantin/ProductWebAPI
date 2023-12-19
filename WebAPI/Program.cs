using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Adding database context.
builder.Services.AddDbContext<ProductContext>(options =>
{
	options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
	options.EnableSensitiveDataLogging(true);
});

// Adding data filler class.
builder.Services.AddTransient<SeedData>();

// Adding controllers
builder.Services.AddControllers().AddNewtonsoftJson();

// Configuring JSON parser.
builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts => {
	opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});

// Configuring swagger.
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
	// Swagger will use the documentation from controller files.
	string? xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	string? xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Filling the database.
if (app.Environment.IsDevelopment())
{
	using (var scope = app.Services.CreateScope())
	{
		var scopedContext = scope.ServiceProvider.GetRequiredService<SeedData>();
		scopedContext.SeedDatabase();
	}
}

// Mapping controllers.
app.MapControllers();

// Using swagger.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
});

app.Run();