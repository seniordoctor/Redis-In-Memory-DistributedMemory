using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedSetNames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                /*db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });*/

                // score degeri default kucukten buyugedir(asc), ornek olarak buyukten kucuge yaptik(desc).
                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }

            return View(list);
        }

        [HttpPost]
        public IActionResult Add(string name, double score)
        {
            db.SortedSetAdd(listKey, name, score);
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            return RedirectToAction("Index");
        }

        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
