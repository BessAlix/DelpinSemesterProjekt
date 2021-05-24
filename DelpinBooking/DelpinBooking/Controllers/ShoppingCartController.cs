using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DelpinBooking.Models;
using Microsoft.AspNetCore.Session;
using System.Configuration;
using DelpinBooking.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace DelpinBooking.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Machine>>(HttpContext.Session, "cart");
            return View(cart);
        }

        [Route("[action]")]
        public IActionResult Add(Machine machine)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Machine>>(HttpContext.Session, "cart");

            if (cart ==null)
            {
                cart = new List<Machine>();
            }
            cart.Add(machine);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction("Index");

        }
        
        [Route("[action]")]
        public void Clear()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);
        }
    }
}
