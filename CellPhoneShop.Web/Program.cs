using Microsoft.AspNetCore.Authentication.Cookies;
using CellPhoneShop.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToFolder("/Phone");
    options.Conventions.AllowAnonymousToFolder("/Product");
});

// Configure HttpClient
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5241/");
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowWebApp",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:5241", "https://localhost:7277")
//                  .AllowAnyHeader()
//                  .AllowAnyMethod()
//                  .AllowCredentials();
//        });
//});

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "CellPhoneShop.Auth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });

//Dang Duc: them service
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSession();
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();
//DANGDUC

// Configure HttpClient for API calls
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
Console.WriteLine(apiBaseUrl);
if (string.IsNullOrEmpty(apiBaseUrl))
{
    throw new InvalidOperationException("API Base URL is not configured. Please set the 'ApiBaseUrl' value in appsettings.json");
}

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient();

// Add OData Customer Service
builder.Services.AddScoped<IODataCustomerService, ODataCustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//DANGDUC
app.UseSession();
//
app.UseRouting();
//app.UseCors("AllowWebApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.Run();