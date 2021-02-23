using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using xyz.lsyyy.Verification.Extension.Service;
using xyz.lsyyy.Verification.Protos;
using LoginModel = xyz.lsyyy.Verification.Test.Model.User.LoginModel;

namespace xyz.lsyyy.Verification.Test
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly UserService userService;
		public UserController(UserService userService)
		{
			this.userService = userService;
		}

		/// <summary>
		/// 用户注册
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> RegistUserAsync([FromBody] UserAddModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Name))
			{
				return BadRequest("用户名不能为空");
			}
			RegistUserResponse result = await userService.RegistUserAsync(model);
			if (result.Success)
			{
				return Ok();
			}

			return BadRequest(result.Message);
		}

		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("login/")]
		public async Task<object> LoginAsync(LoginModel model)
		{
			LoginResponse result = await userService.UserLoginAsync(new Extension.LoginModel
			{
				Name = model.UserName,
				Password = model.Password
			});
			if (result.Success)
			{
				HttpContext.Session.SetString("UserId", result.UserResponse.Id);
				return Ok();
			}
			return BadRequest("用户不存在或密码错误");
		}
	}
}
