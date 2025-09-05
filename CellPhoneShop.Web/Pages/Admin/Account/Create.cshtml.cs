using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Domain.Models;
using CellPhoneShop.Web.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CellPhoneShop.Web.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;

        [BindProperty]
        public UserProfileDto Account { get; set; }

        public CreateModel(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("API");

            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Auth/Login");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/User/createUser", Account);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ModelState.AddModelError(string.Empty, "Error creating account.");




            return Page();
        }
    }
}