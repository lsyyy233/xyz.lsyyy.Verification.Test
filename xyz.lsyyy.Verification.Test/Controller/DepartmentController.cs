using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xyz.lsyyy.Verification.Extension;
using xyz.lsyyy.Verification.Extension.Service;
using xyz.lsyyy.Verification.Protos;

namespace xyz.lsyyy.Verification.Test
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DepartmentController : ControllerBase
	{
		private readonly DepartmentService departmentService;
		public DepartmentController(DepartmentService departmentService)
		{
			this.departmentService = departmentService;
		}

		/// <summary>
		/// 添加部门
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> AddDepartmentAsync([FromBody] DepartmentAddModel model)
		{
			GeneralResponse response = await departmentService.AddDepartmentAsync(model);
			if (response.IsSuccess)
			{
				return Ok();
			}

			return BadRequest(response.Message);
		}
	}
}
