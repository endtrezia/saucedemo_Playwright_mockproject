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
    // /This class is used to deserialize JSON data from a file into a list of ExpandoObject (which can be replaced with any Object Type).
    {
        public static List<ExpandoObject> LoadJsonObjectData(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Date file not found!");

            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("Date file is empty!");
            if(JsonConvert.DeserializeObject<List<ExpandoObject>>(json) is null)
                throw new InvalidOperationException("Date file is not in the correct format!");
            return JsonConvert.DeserializeObject<List<ExpandoObject>>(json);
        }
    }
}
