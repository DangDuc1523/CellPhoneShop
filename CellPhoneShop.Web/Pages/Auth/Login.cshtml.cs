using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace CellPhoneShop.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }

        public LoginModel(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _clientFactory.CreateClient("API");
                    var json = JsonSerializer.Serialize(LoginRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/api/Auth/Login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(options);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, authResponse.UserId.ToString()),
                            new Claim(ClaimTypes.Name, authResponse.FullName),
                            new Claim(ClaimTypes.Email, authResponse.Email),
                            new Claim(ClaimTypes.Role, GetRoleName(authResponse.Role)),
                            new Claim("Token", authResponse.Token)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger.LogInformation("User {Email} logged in successfully with role {Role}", authResponse.Email, authResponse.Role);

                        // Redirect based on role
                        var redirectUrl = authResponse.Role switch
                        {
                            1 => returnUrl ?? Url.Content("~/Admin"), // Admin
                            2 => returnUrl ?? Url.Content("~/Admin"), // Staff (also goes to admin)
                            3 => returnUrl ?? Url.Content("~/"), // Customer
                            _ => returnUrl ?? Url.Content("~/") // Default to customer
                        };

                        return LocalRedirect(redirectUrl);
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during login");
                    ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                    return Page();
                }
            }

            return Page();
        }

        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Admin",
                2 => "Staff",
                3 => "Customer",
                _ => "Customer"
            };
        }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string Token { get; set; }
    }
} 