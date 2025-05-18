using Newtonsoft.Json;
using saucedemo_Playwright_mockproject.Model;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Utils
{
    public static class DeserializeJson
    {
        public static List<ExpandoObject> LoadJsonObjectData(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Date file not found!");

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(json);
        }
    }
}
