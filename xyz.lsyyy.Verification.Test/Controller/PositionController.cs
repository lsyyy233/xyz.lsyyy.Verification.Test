using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using xyz.lsyyy.Verification.Extension;
using xyz.lsyyy.Verification.Protos;

namespace xyz.lsyyy.Verification
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class PositionController : ControllerBase
	{
		private readonly PositionService positionService;
		public PositionController(PositionService positionService)
		{
			this.positionService = positionService;
		}

		/// <summary>
		/// 添加职位
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<object> AddPositionAsync(PositionAddModel model)
		{
			GeneralResponse response = await positionService.AddPositionAsync(model);
			if (response.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(response.Message);
		}
	}
}
