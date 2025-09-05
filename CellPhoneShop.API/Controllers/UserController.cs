using CellPhoneShop.Business.DTOs;
using CellPhoneShop.Business.DTOs.Auth;
using CellPhoneShop.Business.Services;
using CellPhoneShop.DataAccess.Respositories;
using CellPhoneShop.DataAccess.Respositories.Interfaces;
using CellPhoneShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserRepository userRepository, IUserService userService)
	{
		_userService = userService;
	}

	[HttpGet("profile")]
	[Authorize]
	public async Task<IActionResult> GetProfile()
	{
		// Lấy UserId từ token (claims)
		var userIdClaim = User.FindFirst("userId");
		if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
		{
			return Unauthorized();
		}

		// Lấy thông tin user từ database
		var user = await _userService.GetByIdAsync(userId);
		if (user == null)
		{
			return NotFound();
		}

        // Map UserAccount sang UserProfileDto
        UserProfileDto profile = new UserProfileDto
        {
            UserId = userId,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            ProvinceId = user.ProvinceId,
            DistrictId = user.DistrictId,
            WardId = user.WardId,
            AddressDetail = user.AddressDetail,
            Role = user.Role,
            Status = user.Status
        };

		return Ok(profile);
	}


    [HttpGet("profilebyid/{id}")]
    [Authorize]
    public async Task<IActionResult> GetProfileById(int id)
    {

        // Lấy thông tin user từ database
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // Map UserAccount sang UserProfileDto
        var profile = new UserProfileDto
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            ProvinceId = user.ProvinceId,
            DistrictId = user.DistrictId,
            WardId = user.WardId,
            AddressDetail = user.AddressDetail,
            Role = user.Role,
            Status = user.Status
        };

        return Ok(profile);
    }



    [HttpGet("Alluser")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllAccount()
	{
        try
        {
            var users = await _userService.GetAllAccounts();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }

    [HttpPut("updateUserByToken")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileByToken([FromBody] UpdateUserProfileDto updateDto)
    {
        var userIdClaim = User.FindFirst("userId") ?? User.FindFirst("sub");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized();

        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
            return NotFound();

        // Cập nhật các trường cho phép
        if (updateDto.FullName != null)
            user.FullName = updateDto.FullName;

        if (updateDto.Phone != null)
            user.Phone = updateDto.Phone;

        if (updateDto.ProvinceId != null)
            user.ProvinceId = updateDto.ProvinceId;

        if (updateDto.DistrictId != null)
            user.DistrictId = updateDto.DistrictId;

        if (updateDto.WardId != null)
            user.WardId = updateDto.WardId;

        if (updateDto.AddressDetail != null)
            user.AddressDetail = updateDto.AddressDetail;

        await _userService.UpdateUserAsync(user);

        return NoContent();
    }
    [HttpPut("updateUserById/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileById(int id, [FromBody] UpdateUserProfileDto updateDto)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        // Cập nhật các trường cho phép

            user.FullName = updateDto.FullName;


            user.Phone = updateDto.Phone;


            user.ProvinceId = updateDto.ProvinceId;


            user.DistrictId = updateDto.DistrictId;


            user.WardId = updateDto.WardId;


            user.AddressDetail = updateDto.AddressDetail;

        await _userService.UpdateUserAsync(user);

        return NoContent();
    }

    [HttpDelete("deleteUser/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        else
        {
            try
            {
                await _userService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return NoContent();
    }

    [HttpPost("createUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] UserProfileDto userDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = User.FindFirst("Token")?.Value;
        var userId = User.FindFirst("userId")?.Value;

        Console.WriteLine(userId);

        var newUser = new UserAccount
        {
            
            FullName = userDto.FullName,
            PasswordHash = userDto.PasswordH,
            Email = userDto.Email,
            Phone = userDto.Phone,
            ProvinceId = userDto.ProvinceId,
            DistrictId = userDto.DistrictId,
            WardId = userDto.WardId,
            AddressDetail = userDto.AddressDetail,
            Role = userDto.Role,
            Status = userDto.Status,
            CreatedBy = int.Parse(userId),
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _userService.CreateAsync(newUser);
            return Ok(new { message = "User created successfully." });
        }
        catch (Exception ex)
        {
            var inner = ex.InnerException?.Message;
            Console.WriteLine("Lỗi tạo user:");
            Console.WriteLine(ex.Message);
            Console.WriteLine(inner); // 👈 log thêm lỗi bên trong

            return StatusCode(500, new
            {
                message = "An error occurred while creating the user.",
                error = ex.Message,
                innerError = inner
            });
        }

    }

    [HttpPut("changePass")]
    [Authorize]
    public async Task<IActionResult> changePass(ChangePasswordDto changePassworddto)
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        var user = await _userService.GetByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        else
        {
            string checkPass = HashPassword(changePassworddto.CurrentPassword);
            if (user.PasswordHash == checkPass)
            {
                if (changePassworddto.NewPassword == changePassworddto.ConfirmPassword)
                {
                    var result = await _userService.ChangePassWord(userId, HashPassword(changePassworddto.NewPassword));
                    if (result == true)
                    {
                        return Ok("Change password successfully!");
                    }
                    else
                    {
                        return BadRequest("Đổi mật khẩu thất bại, kiểm tra lại mật khẩu mới ");
                    }
                }
                else
                {
                    return BadRequest("Đổi mật khẩu thất bại, kiểm tra lại mật khẩu mới ");
                }
            }
            else
            {
                return BadRequest("Mật khẩu cũ không đúng");
            }

        }
    }
    //DANGDUC
    [HttpPost("reset-password")]
    [AllowAnonymous] // Nếu bạn không cần token
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            return BadRequest("Email và mật khẩu mới là bắt buộc.");
        }

        dto.NewPassword = HashPassword(dto.NewPassword);
        var success = await _userService.UpdatePasswordByEmailAsync(dto.Email, dto.NewPassword);

        if (!success)
        {
            return NotFound("Không tìm thấy tài khoản với email này.");
        }

        return Ok(new { message = "Mật khẩu đã được cập nhật." });
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    //DANGDUC
}
