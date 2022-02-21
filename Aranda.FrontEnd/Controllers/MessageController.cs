using Aranda.FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Aranda.FrontEnd.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ModalDelete(string id)
        {
            ViewBag.ProductId = id;
            return View();
        }
        public ActionResult ModalCreate()
        {
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
            };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
            Task<HttpResponseMessage> result = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Category/GettAll");
            List<Category> category = JsonConvert.DeserializeObject<List<Category>>(result.Result.Content.ReadAsStringAsync().Result);
            ViewBag.Categories = category.ToList();
            return View();
        }
        public ActionResult ModalEdit(string id)
        {
            try
            {
                ViewBag.ProductId = id;
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                Task<HttpResponseMessage> result = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Category/GettAll");
                List<Category> category = JsonConvert.DeserializeObject<List<Category>>(result.Result.Content.ReadAsStringAsync().Result);
                httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                Task<HttpResponseMessage> product = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/Find{id}");
                Product response = JsonConvert.DeserializeObject<Product>(product.Result.Content.ReadAsStringAsync().Result);
                ProductDetail productDetail = new ProductDetail()
                {
                    Categories = category,
                    ProductEdit = response
                };
                return View(productDetail);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ModalImages(string id)
        {
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                Task<HttpResponseMessage> result = httpClient.GetAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/GetAllImages{Guid.Parse(id)}");
                List<ProductImages> productImages = JsonConvert.DeserializeObject<List<ProductImages>>(result.Result.Content.ReadAsStringAsync().Result);
                ViewBag.ProductImages = (productImages?.Count < 0 || productImages is null) ? new List<ProductImages>() : productImages.ToList();
                ViewBag.ProductId = id;
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult FileUpload(HttpPostedFileBase imagesUrl)
        {
            if (imagesUrl != null)
            {
                string url = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                {
                    imagesUrl.InputStream.CopyTo(ms);
                    url = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(imagesUrl.FileName));
                    imagesUrl.SaveAs(url);
                }
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Convert.ToString(ConfigurationManager.AppSettings["Aranda.WebApi"]))
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                ProductImagesCommand productImagesRequest = new ProductImagesCommand()
                {
                    ProductId = Request.Params["productId"],
                    ImagesUrl = new List<string> { url }
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(productImagesRequest), Encoding.UTF8, Constants.ApplicationJson);
                Task<HttpResponseMessage> result = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/CreateImages", content);
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult ModalView(string id)
        {
            ViewBag.ProductId = id;
            return View();
        }
        [HttpPost]
        public JsonResult Create()
        {
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["Aranda.WebApi"]),
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                Product product = new Product()
                {
                    BriefDescription = Request.Params["BriefDescription"],
                    Name = Request.Params["Name"],
                    CategoryId = Guid.Parse(Request.Params["CategoryId"])
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, Constants.ApplicationJson);
                HttpResponseMessage consumer = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/Create", content).Result;
                return Json(consumer.StatusCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CategoryCreate()
        {
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["Aranda.WebApi"]),
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                CategoryRequest category = new CategoryRequest()
                {
                    CategoryName = Request.Params["CategoryName"],
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, Constants.ApplicationJson);
                HttpResponseMessage consumer = httpClient.PostAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Category/Create", content).Result;
                return Json(consumer.StatusCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Update()
        {
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["Aranda.WebApi"]),
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                Product product = new Product()
                {
                    ProductId = Guid.Parse(Request.Params["ProductId"]),
                    BriefDescription = Request.Params["BriefDescription"],
                    Name = Request.Params["Name"]
                };
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, Constants.ApplicationJson);
                HttpResponseMessage consumer = httpClient.PutAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/{Guid.Parse(Request.Params["CategoryId"])}", content).Result;
                return Json(consumer.StatusCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete()
        {
            try
            {
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri(ConfigurationManager.AppSettings["Aranda.WebApi"]),
                };
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.ApplicationJson));
                HttpResponseMessage consumer = httpClient.DeleteAsync($"{ConfigurationManager.AppSettings["Aranda.WebApi"]}/api/Products/{Guid.Parse(Request.Params["ProductId"])}").Result;
                return Json(consumer.StatusCode, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
