using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Clothes33.Views.Home
{
    public class CardModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CardModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }
    }
}

        