using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace פרקטיקוד2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _show = new HtmlHelper();

        public static HtmlHelper Show => _show;
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {
            string json = File.ReadAllText("Data/HtmlTags.json");
            AllTags = JsonSerializer.Deserialize<string[]>(json)
                      ?? Array.Empty<string>();

            string jsonVoid = File.ReadAllText("Data/HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(jsonVoid)
                              ?? Array.Empty<string>();
        }



    }
}
