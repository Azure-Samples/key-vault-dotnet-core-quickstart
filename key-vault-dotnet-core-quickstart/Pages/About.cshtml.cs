using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace key_vault_dotnet_core_quickstart.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }

        public AboutModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration = null;

        public void OnGet()
        {
            Message = "My key val = " + _configuration["AppSecret"];
        }
    }
}
