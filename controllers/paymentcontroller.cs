using Microsoft.AspNetCore.Mvc;
using Clothes33.Models;
using System.Collections.Generic;

namespace Clothes33.Controllers
{
    public class PaymentController : Controller
    {
        private List<CartItem> cart = new List<CartItem>(); // Your cart list

        [HttpGet]
        public IActionResult Payment()
        {
            var cartModel = new CartModel
            {
                CartItems = cart
            };

            return View(cartModel);
        }

        [HttpPost]
        public IActionResult ProcessPayment()
        {
            // Implement your payment processing logic here
            // For demonstration purposes, we're just clearing the cart after payment
            cart.Clear();

            return RedirectToAction("PaymentConfirmation");
        }

        public IActionResult PaymentConfirmation()
        {
            return View();
        }
    }
}
