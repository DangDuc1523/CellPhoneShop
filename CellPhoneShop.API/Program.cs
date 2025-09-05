using CellPhoneShop.Business.Services;
using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.Domain.Models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Microsoft.EntityFrameworkCore;

using CellPhoneShop.Business.Mapping;

using CellPhoneShop.Business.DataProtection;
using CellPhoneShop.DataAccess.UnitOfWork.Interfaces;
using CellPhoneShop.DataAccess.UnitOfWork;
using CellPhoneShop.Business.Interfaces;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Business.Services.Interfaces;
using CellPhoneShop.DataAccess.Repositories.Interfaces;
using CellPhoneShop.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
        };
    });

builder.Services.AddAuthorization();

// Cấu hình DbContext
builder.Services.AddDbContext<CellPhoneShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình AutoMapper
builder.Services.AddAutoMapper(typeof(MasterMappingProfile), 
    typeof(LocationMappingProfile),
    typeof(PhoneMappingProfile),
    typeof(ColorMappingProfile),
    typeof(BrandMappingProfile),
    typeof(UserMappingProfile));

// DI services
builder.Services.AddControllers();


// Đăng ký UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// User and Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

// Location Services
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<IWardService, WardService>();

// Order Services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IMasterService, MasterService>();

// Product Services
builder.Services.AddScoped<IPhoneService, PhoneService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<IColorImageService, ColorImageService>();
builder.Services.AddScoped<IPhoneVariantService, PhoneVariantService>();

// Attribute Services
builder.Services.AddScoped<IPhoneAttributeService, PhoneAttributeService>();
builder.Services.AddScoped<IVariantAttributeService, VariantAttributeService>();
builder.Services.AddScoped<IVariantAttributeValueService, VariantAttributeValueService>();

// User and Cart Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

// Location Repositories
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<IDistrictRepository, DistrictRepository>();
builder.Services.AddScoped<IWardRepository, WardRepository>();

// Order Repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IMasterRepository, MasterRepository>();

// Product Repositories
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IColorImageRepository, ColorImageRepository>();
builder.Services.AddScoped<IPhoneVariantRepository, PhoneVariantRepository>();

// Attribute Repositories
builder.Services.AddScoped<IPhoneAttributeRepository, PhoneAttributeRepository>();
builder.Services.AddScoped<IVariantAttributeRepository, VariantAttributeRepository>();
builder.Services.AddScoped<IVariantAttributeValueRepository, VariantAttributeValueRepository>();
builder.Services.AddScoped<IPhoneAttributeMappingRepository, PhoneAttributeMappingRepository>();



// Workers
builder.Services.AddScoped<IDataProtection, DataProtection>();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Thêm cấu hình CORS cho FE
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "CellPhoneShop API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập token theo dạng: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
