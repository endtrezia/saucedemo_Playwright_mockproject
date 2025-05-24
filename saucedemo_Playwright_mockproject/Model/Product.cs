using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Model
{
    public class Product(decimal price, string name, string description)
    {
        public decimal Price { get; } = price;
        public string ProductName { get; } = name;
        public string Description { get; } = description;

        /*This method is for comparing two Product objects for equality, Nunit Equals not working with custom objects, its a pain.
         Have to create a override method to force EqualTo comparing with this
         As research, FluentAssertions may help, but i want to keep the project simple and not add any extra dependencies.
        */
        public override bool Equals(object product)
        {
            return product is Product comparable &&
                   Price == comparable.Price &&
                   ProductName == comparable.ProductName &&
                   Description == comparable.Description;
        }
        public override int GetHashCode() => HashCode.Combine(Price, ProductName, Description);
    }

}
