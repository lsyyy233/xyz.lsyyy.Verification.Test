using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using xyz.lsyyy.Verification.Extension.Service;
using xyz.lsyyy.Verification.Protos;
using xyz.lsyyy.Verification.Util;

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
		public async Task<object> RegistUserAsync([FromBody] UserRegistModel model)
		{
			if (string.IsNullOrWhiteSpace(model.UserName))
			{
				return BadRequest(WebResultHelper.JsonMessageResult("用户名不能为空"));
			}

			if (string.IsNullOrWhiteSpace(model.Password))
			{
				return BadRequest(WebResultHelper.JsonMessageResult("密码不能为空"));
			}
			GeneralResponse result = await userService.RegistUserAsync(new Extension.UserRegistModel
			{
				Name = model.UserName,
				Password = model.Password,
				PositionId = model.PositionId
			});
			if (result.IsSuccess)
			{
				return Ok();
			}

			return BadRequest(WebResultHelper.JsonMessageResult(result.Message));
		}

		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("login/")]
		public async Task<object> LoginAsync(LoginModel model)
		{
			UserLoginResponse result = await userService.UserLoginAsync(new Extension.LoginModel
			{
				Name = model.UserName,
				Password = model.Password
			});
			if (result.IsSuccess)
			{
				//HttpContext.Session.SetString("Token", result.Token);
				return Ok(new 
				{
					result.Token
				});
			}
			return BadRequest(WebResultHelper.JsonMessageResult("用户名或密码错误"));
		}

		/// <summary>
		/// 注册管理员用户
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("admin")]
		public async Task<object> RegistAdmin([FromBody] AdminRegistModel model)
		{
			if (string.IsNullOrWhiteSpace(model.UserName))
			{
				return BadRequest(WebResultHelper.JsonMessageResult("用户名不能为空"));
			}
			if (string.IsNullOrWhiteSpace(model.Password))
			{
				return BadRequest(WebResultHelper.JsonMessageResult("密码不能为空"));
			}

			GeneralResponse response = await userService.RegistAdminAsync(new Extension.AdminRegistModel
			{
				
				Name = model.UserName,
				Password = model.Password
			});
			if (response.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(WebResultHelper.JsonMessageResult(response.Message));
		}
	}
}
