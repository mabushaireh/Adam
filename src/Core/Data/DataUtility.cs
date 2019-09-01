using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace i2fam.Core.Data
{
    public static class DataUtility
    {
        public static ICollection<Tuple<string, string>> GetCountries(string lang = "en")
        {
            var jsonString = GetData("countries", lang);

            return JsonConvert.DeserializeObject<ICollection<Tuple<string, string>>>(jsonString);
        }

        public static ICollection<Tuple<string, string>> GetHoroscopes(string lang = "en")
        {
            var jsonString = GetData("horoscopes", lang);

            return JsonConvert.DeserializeObject<ICollection<Tuple<string, string>>>(jsonString);
        }


        public static dynamic GetFamilyMembers()
        {
            dynamic data = JsonConvert.DeserializeObject(GetData("familymembers"));

            return data;

        }

        private static string GetData(string datatype, string lang)
        {
            var assembly = typeof(DataUtility).GetTypeInfo().Assembly;

            using (
                var textStream =
                    new StreamReader(assembly.GetManifestResourceStream($"i2fam.Core.Data.json.{lang}.{datatype}.json")))
            {
                return textStream.ReadToEnd();
            }
        }

        private static string GetData(string datatype)
        {
            var assembly = typeof(DataUtility).GetTypeInfo().Assembly;

            using (
                var textStream =
                    new StreamReader(assembly.GetManifestResourceStream($"i2fam.Core.Data.json.{datatype}.json")))
            {
                return textStream.ReadToEnd();
            }
        }
    }
}
