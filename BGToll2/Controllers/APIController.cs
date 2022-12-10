using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace BGToll2.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult BGToll(string number)
        {
            var url = "https://check.bgtoll.bg/check/vignette/plate/BG/" + number;
            var client = new WebClient();
            var body = "";

            if (number != null && number != "")
            {
                body = client.DownloadString(url);
                JObject data = JObject.Parse(body);

                if ((bool)data["ok"])
                {
                    ViewData["result"] = "Намерена е винетка за номер " + number;

                    ViewData["number"] = data["vignette"]["vignetteNumber"];
                    ViewData["active"] = data["vignette"]["validityDateToFormated"];
                    ViewData["class"]  = data["vignette"]["vehicleClass"];
                }
                else
                {
                    ViewData["result"] = "Не е намерена е винетка за номер " + number;
                }

                ViewData["body"] = body;
            }

            return View();
        }

        public ActionResult Nasa()
        {

            var apiKey = "T7lPpBbIxAwr9GABoU6PQYrDHjgznoikS8GYM5zW";
            var url = "https://api.nasa.gov/planetary/apod?api_key=" + apiKey;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync("").Result;

                if(response.IsSuccessStatusCode)
                {
                    var body = response.Content.ReadAsStringAsync().Result;
                    JObject data = JObject.Parse(body);

                    ViewData["title"] = data["title"];
                    ViewData["explanation"] = data["explanation"];
                    ViewData["image"] = data["hdurl"];
                    ViewData["date"] = data["date"];
                    ViewData["copyright"] = data["copyright"];

                    ViewBag.result = body;
                }
                else
                {
                    ViewData["title"] = "There was an error.";
                }
            }

            return View();
        }
    }
}