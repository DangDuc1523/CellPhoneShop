using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Web.DTOs.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CellPhoneShop.Web.Pages.Accounts
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]

        public int? Id { get; set; }

        [BindProperty]
        public UpdateUserProfileDto accountNew { get; set; }

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            var client = _httpClientFactory.CreateClient("API");

            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                // Ch?a c� token -> redirect ??n trang ??ng nh?p
                return RedirectToPage("/Auth/Login");
            }

            // Th�m Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            try
            {

                UserProfileDto result = await client.GetFromJsonAsync<UserProfileDto>($"/api/User/profilebyid/{id}");
                Id = result.UserId;
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    accountNew = new UpdateUserProfileDto();
                    accountNew.FullName = result.FullName;
                    accountNew.Phone = result.Phone;
                    accountNew.AddressDetail = result.AddressDetail;
                    accountNew.DistrictId = (int)result.DistrictId;
                    accountNew.ProvinceId = (int)result.ProvinceId;
                    accountNew.WardId = (int)result.WardId;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error fetching accounts: {ex.Message}");
            }
            return Page();
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

            try
            {
                var response = await client.PutAsJsonAsync($"/api/User/updateUserById/{Id}", accountNew);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Error updating account.");
            }
            return Page();
        }
    }
}