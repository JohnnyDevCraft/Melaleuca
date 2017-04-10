using System;
using Microsoft.AspNetCore.Mvc;
using ServerLoad.Core.Abstraction.Logic;
using System.Threading.Tasks;
using ServerLoad.Core.Abstraction.Model;
using ServerLoad.Domain.ViewModel;
using ServerLoad.Domain.DataModel;
namespace ServerLoad.WebApi.Controllers
{
	[Route("api/[controller]")]
	public class ServerController : Controller
	{
		private IServerLogic _logic;

		public ServerController(IServerLogic logic)
		{
			_logic = logic;
		}

		[HttpPost("{serverName}/checkin")]
		public async Task<IActionResult> Checkin(string serverName, [Bind("SampleTime, CpuUtilization, RamUtilization")]Checkin model)
		{

			CheckinActionVm vm = new CheckinActionVm()
			{
				SampleTime = model.SampleTime,
				CpuUtilization = model.CpuUtilization,
				RamUtilization = model.RamUtilization,
				ServerName = serverName
			};

			return Json(await _logic.CheckinAsync(vm));
		}

		[HttpGet("{serverName}/loads")]
		public async Task<IActionResult> Loads(string serverName)
		{

			return Json(await _logic.LoadsAsync(serverName));
		}

		[HttpGet("{serverName}/loadsByMinute/{start}/{end}")]
		public async Task<IActionResult> LoadsByMinute(string serverName, DateTime start, DateTime end)
		{

			return Json(await _logic.LoadsByMinuteAsync(serverName, start, end));
		}

		[HttpGet("{serverName}/loadsByHour/{start}/{end}")]
		public async Task<IActionResult> LoadsByHour(string serverName, DateTime start, DateTime end)
		{

			return Json(await _logic.LoadsByHourAsync(serverName, start, end));
		}
	}
}
