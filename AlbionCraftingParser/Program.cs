using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlbionCraftingParser
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            JObject result = JObject.Parse(webClient.DownloadString("https://raw.githubusercontent.com/broderickhyman/ao-bin-dumps/master/items.json"));
            JToken weapons = result["items"]["weapon"];
            JToken equipments = result["items"]["equipmentitem"];
            JArray itemList = new JArray();
            foreach (JObject weapon in weapons)
            {
                if (weapon.ContainsKey("craftingrequirements"))
                {
                    JToken craftingRecepie = weapon["craftingrequirements"]["craftresource"];
                    JToken uniquename = weapon["@uniquename"];
                    var item = new
                    {
                        name = uniquename,
                        craftingrecepie = craftingRecepie
                    };
                    string json_data = JsonConvert.SerializeObject(item, Formatting.None);
                    JObject json_object = JObject.Parse(json_data);
                    itemList.Add(json_object);
                }
            }
            foreach (JObject equipment in equipments)
            {
                if (equipment.ContainsKey("craftingrequirements"))
                {
                    JToken craftingRecepie = null;
                    if (equipment["craftingrequirements"] is JArray)
                        craftingRecepie = equipment["craftingrequirements"][0]["craftresource"];
                    else
                        craftingRecepie = equipment["craftingrequirements"]["craftresource"];
                    JToken uniquename = equipment["@uniquename"];
                    var item = new
                    {
                        name = uniquename,
                        craftingrecepie = craftingRecepie
                    };
                    string json_data = JsonConvert.SerializeObject(item, Formatting.None);
                    JObject json_object = JObject.Parse(json_data);
                    itemList.Add(json_object);
                }
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\Users\Ilan\source\repos\AlbionCraftingParser\AlbionCraftingParser", "Craftable.json")))
            {
                    outputFile.Write(itemList.ToString());
            }
        }
    }
}
