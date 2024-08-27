using System;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /*public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name","recep", cacheEntryOptions);
            _distributedCache.SetString("sname","mert", cacheEntryOptions);

            return View();
        }*/

        public async Task<IActionResult> Index()    // async calisma
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

            // Complex type caching
            Product product = new Product { ProductId = 1, ProductName = "Kalem1", Price = 1509 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("product:1", byteProduct, cacheEntryOptions);

            //await _distributedCache.SetStringAsync("product:2", jsonProduct, cacheEntryOptions);

            /*_distributedCache.SetString("name", "recep", cacheEntryOptions);
            await _distributedCache.SetStringAsync("surname", "mert");  // bu islem bitene kadar bir alta devam etmeyecegi anlamina gelir.*/

            return View();
        }

        public IActionResult Show()
        {
            string jsonProduct = _distributedCache.GetString("product:1");

            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            /*string name = _distributedCache.GetString("name");
            string sname = _distributedCache.GetString("sname");
            ViewBag.name = name;
            ViewBag.sname = sname;*/
            ViewBag.product = product;

            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("name");

            return View();
        }

        public IActionResult ImageUrl()
        {
            // Index uzerinde goruntulemek istedigim icin Index.cshtml'e gidip goruntuleme islemini orada yaptik.
            Byte[] imageByte = _distributedCache.Get("resim");

            return File(imageByte, "image/png");
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/cache-temizle-tekrar-dene.png");

            Byte[] imageByte = System.IO.File.ReadAllBytes(path); // set bizden bir byte dizini istiyor.

            _distributedCache.Set("resim", imageByte);

            return View();
        }
    }
}
