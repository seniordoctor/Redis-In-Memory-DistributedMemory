using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            db.StringSet("name", "Recep MERT");
            db.StringSet("ziyaretci", 100);

            return View();
        }

        public IActionResult Show()
        {
            var name = db.StringGet("name");
            var ziyaretci = db.StringGet("ziyaretci");

            db.StringIncrement("ziyaretci", 1);

            if (name.HasValue && ziyaretci.HasValue)
            {
                ViewBag.Name = name.ToString();
                ViewBag.Ziyaretci = ziyaretci.ToString();
            }

            ViewBag.Name = name;
            ViewBag.Ziyaretci = ziyaretci;

            return View();
        }
    }
}
