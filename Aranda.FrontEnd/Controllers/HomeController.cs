using Aranda.FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aranda.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
            Task<HttpResponseMessage> result = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/GettAll");
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(result.Result.Content.ReadAsStringAsync().Result);
            return View(products is null ? new List<Product>() :products.ToList());
        }
    }
}