using Microsoft.AspNetCore.Mvc;
using Clothes33.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Clothes33.Controllers
{
    public class HomeController : Controller
    {
        private List<ClothingItem> clothesList = new List<ClothingItem>
        {
            new ClothingItem { Id = 1, Name = "Reuseable straw", Price = 19.99, ImageUrl = "/images/tshirt.jpg" },
            new ClothingItem { Id = 2, Name = "Cutlerry", Price = 49.99, ImageUrl = "/images/jeans.jpg" },
            new ClothingItem { Id = 3, Name = "Reuseable bottle", Price = 59.99, ImageUrl = "/images/dress.jpg" },
            new ClothingItem { Id = 4, Name = "Bamboo Straw", Price = 39.99, ImageUrl = "/images/sweater.jpg" },
        };

        private List<CartItem> cart = new List<CartItem>();
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Welcome to Your eco friendly store";
            ViewData["ClothesList"] = clothesList;
            return View(clothesList);
        }

        public IActionResult Cart()
        {
            var cartModel = new CartModel
            {
                CartItems = cart
            };
            return View(cartModel);
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var clothingItem = clothesList.Find(item => item.Id == id);
            if (clothingItem != null)
            {
                var existingCartItem = cart.Find(item => item.ClothingItemId == id);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity++;
                }
                else
                {
                    cart.Add(new CartItem
                    {
                        ClothingItemId = id,
                        Name = clothingItem.Name,
                        Price = clothingItem.Price,
                        Quantity = 1
                    });
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cartItem = cart.Find(item => item.ClothingItemId == id);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            var cartItem = cart.Find(item => item.ClothingItemId == id);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Payment(int id)
        {
            var clothingItem = clothesList.Find(item => item.Id == id);
            return View(clothingItem);
        }

        public IActionResult CardPayment(int id)
        {
            var clothingItem = clothesList.Find(item => item.Id == id);
            if (clothingItem == null)
            {
                return NotFound();
            }

            return View(clothingItem);
        }


        public IActionResult ProcessCardPayment(int id, string cardNumber, string cvv, string expiryDate)
        {
            // Process the card payment here
            // You can implement your payment gateway integration logic here

            // For now, let's assume the payment is successful
            ViewBag.Message = "Payment Successful!";
            return View("PaymentSuccess");
        }


        [HttpPost]
        public IActionResult InsertPaymentData(string name, double price, int quantity, int cardNumber)
        {
            _InsertPaymentData(name, price, quantity, cardNumber);
            return Ok(); // Return an HTTP response indicating success
        }

        private void _InsertPaymentData(string name, double price, int quantity, int cardNumber)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO payment (Name, Price, Quantity, CardNum) VALUES (@Name, @Price, @Quantity, @CardNumber)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@CardNumber", cardNumber);

                    command.ExecuteNonQuery();
                    connection.Close();
                }

               
            }




        }

    }
}
