using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellPhoneShop.Business.DTOs
{

	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class LoginResponse
	{
		public string Token { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
	}


}
