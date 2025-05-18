using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Model
{
    public class Product(double price, string name, string description)
    {
        public double Price = price;
        public string ProductName = name;
        public string Description= description;
    }
}
