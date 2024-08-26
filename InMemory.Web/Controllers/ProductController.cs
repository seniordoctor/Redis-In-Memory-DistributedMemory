using System;
using InMemory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            /* 1.yöntem, zaman adinda bir key yoksa Set eder
            if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }*/

            /* 2.yöntem, Get yapmaya calisir yoksa Set eder
            if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                // 10 saniye sonra cache silinir
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            }*/

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10);
            
            // 10 saniye icinde erisilse bile absolute verildigi icin yine de 1 dakika sonra silinecek, eski datayi almamak icin uygula
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            options.SlidingExpiration = TimeSpan.FromSeconds(10);

            // Priority sayesinde cache dolunca silinmesi gereken cache silinir, Low'dan High'a dogru siralama yapilir
            options.Priority = CacheItemPriority.High;

            // bir datanin hangi sebepten dolayi silindigini belirtir
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key} -> {value} => {reason} -> {state}");
            });

            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product product = new Product
            {
                ProductId = 1,
                ProductName = "Kalem",
                Price = 100
            };

            _memoryCache.Set<Product>("product:1", product);

            return View();
        }

        public IActionResult Show()
        {
            /* Get yapmaya calisir yoksa create eder
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });*/

            _memoryCache.TryGetValue("zaman", out string zamancache);
            _memoryCache.TryGetValue("callback", out string callbackcache);

            ViewBag.zaman = zamancache;
            ViewBag.callback = callbackcache;
            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}
