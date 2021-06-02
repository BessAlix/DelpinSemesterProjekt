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
        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Machine>>(HttpContext.Session, "cart");

            return View(cart);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Add(Machine machine)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Machine>>(HttpContext.Session, "cart");

            if (cart ==null)
            {
                cart = new List<Machine>();
            }

            if (!IsInCart(machine.Id, cart))
            {
                cart.Add(machine);
            }
            
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Machine>>(HttpContext.Session, "cart");

            if (cart == null)
            {
                cart = new List<Machine>();
            }

            Machine machineToRemove = new Machine();
            foreach (Machine m in cart)
            {
                if (m.Id == id)
                {
                    machineToRemove = m;
                }
            }
            cart.Remove(machineToRemove);

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction(nameof(Index));
        }
        
        [HttpDelete]
        [Route("[action]")]
        public void Clear()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);
        }

        private bool IsInCart(int machineId, List<Machine> cart)
        {
            foreach (Machine m in cart)
            {
                if (machineId == m.Id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
