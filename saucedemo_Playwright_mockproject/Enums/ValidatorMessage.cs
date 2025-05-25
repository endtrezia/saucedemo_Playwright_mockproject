using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Enums
{
    public class ValidatorMessage
    {
        //Define static messages for validation
        public static readonly ValidatorMessage UserNotAt = new("User is not at {0}");
        public static readonly ValidatorMessage NoInventory = new("There is no intentory item or inventory list is not visible");
        public static readonly ValidatorMessage InventoryMissing = new("There is missing data record {0}");
        public static readonly ValidatorMessage WrongSortOrder = new("Wrong sort order");
        public static readonly ValidatorMessage WrongProductDetail = new("Wrong product {0} detail");
        public static readonly ValidatorMessage CartIsNotEmpty = new("Cart is not empty");
        public static readonly ValidatorMessage CartIsEmpty = new("Cart is empty");
        public static readonly ValidatorMessage ProductNotFoundInOverview = new("Product {0} is not in the overview cart");
        public static readonly ValidatorMessage SubtotalValueNotCorrect = new("Subtotal value is not correct");

        private readonly string _template;

        // Constructor
        private ValidatorMessage(string template)
        {
            _template = template;
        }

        // Method to add params to the message template
        public string ReturnMessageWithParam(string text)
        {
            return string.Format(_template, text);
        }
        // Method to return only template
        public string RetunMessage()
        {
            return _template;
        }
    }
}
