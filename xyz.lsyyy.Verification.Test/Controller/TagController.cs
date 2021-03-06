using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using xyz.lsyyy.Verification.Extension.Service;

namespace xyz.lsyyy.Verification.Test
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class TagController
	{
		private readonly MyDbContext db;
		private readonly ActionTagService actionTagService;

		public TagController(MyDbContext db, ActionTagService actionTagService)
		{
			this.db = db;
			this.actionTagService = actionTagService;
		}

		[HttpGet]
		public async Task<object> GetAllTagAsync()
		{
			return await actionTagService.GetAllTagAsync();
		}

		[HttpGet("status")]
		public async Task<object> GetTagStatusAsync()
		{
			return await actionTagService.GetTagStatus();
		}
	}
}
