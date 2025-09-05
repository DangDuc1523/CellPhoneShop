using CellPhoneShop.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace CellPhoneShop.Web.Pages.Customer.Profile.EditProfile
{
    public class EditProfileModel : PageModel
    {

        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]

        public int? Id { get; set; }

        [BindProperty]
        public UpdateUserProfileDto accountNew { get; set; }

        public EditProfileModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            var client = _httpClientFactory.CreateClient("API");

            string token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrEmpty(token))
            {
                // Ch?a có token -> redirect ??n trang ??ng nh?p
                return RedirectToPage("/Auth/Login");
            }

            // Thêm Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            try
            {

                UserProfileDto result = await client.GetFromJsonAsync<UserProfileDto>($"/api/User/profile");
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
                var response = await client.PutAsJsonAsync($"/api/User/updateUserByToken", accountNew);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage();
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
