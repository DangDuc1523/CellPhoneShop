using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using CellPhoneShop.Web.DTOs.Phone;

namespace CellPhoneShop.Web.Pages.Customer.Product
{
    public class ProductModel : PageModel
    {
        public List<PhoneDto> Products { get; set; } = new();
        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            var apiUrl = "http://localhost:5000/api/Phone";
            var phones = await client.GetFromJsonAsync<List<PhoneDto>>(apiUrl);
            if (phones != null)
                Products = phones.Take(8).ToList();
        }
    }
}
