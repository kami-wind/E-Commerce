using BusinessObjects;
using BusinessObjects.Entites;
using DataAccess.Irepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.Options;
using WebApi.ExtensionMethods;
using WebAPI;
using WebAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Odata
builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Expand().Filter().OrderBy().SetMaxTop(100).Count()
           .AddRouteComponents("odata", GetEdmModel());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define and register CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>();

// Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<PaymentRepository>();

builder.Services.AddScoped<VnPayService>();
builder.Services.AddAppOptions<VnpayOptions>(builder.Configuration);

builder.Services.AddScoped<IAuthorizationHandler, AdminEmailRequirementHandler>();

// Identity

builder.Services.AddIdentity<User, Role>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// Authentication
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminEmailPolicy", policy =>
//        policy.Requirements.Add(new AdminEmailRequirement()));
//});

// JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Odata
app.UseODataBatching();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication(); // This should come before UseAuthorization()
app.UseAuthorization();


app.MapControllers();

app.Run();

IEdmModel GetEdmModel()
{
    var modeluilder = new ODataConventionModelBuilder();
    modeluilder.EntitySet<Product>("Products");
    modeluilder.EntitySet<Category>("Categories");
    modeluilder.EntitySet<Cart>("Carts");
    modeluilder.EntitySet<CartItem>("CartItems");


    return modeluilder.GetEdmModel();
}
