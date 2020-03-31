using BalanceSheet.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BalanceSheet.Helper
{
    public class JsonHelper
    {
        public static List<T> LoadJsonFromFile<T>(string fileName)
        {
            using (StreamReader r = File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);

                return items;
            }
        }

        public static void WriteToJson<T>(List<T> jsonList, string fileName)
        {
            using (StreamWriter file = File.CreateText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, jsonList);
            }
        }
    }
}
