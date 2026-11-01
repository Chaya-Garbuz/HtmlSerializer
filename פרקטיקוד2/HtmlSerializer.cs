using System.Text.RegularExpressions;

namespace פרקטיקוד2
{
    public static class HtmlSerializer
    {
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        public static HtmlElement CreateElement(string[] html)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement current = root;

            foreach (var item in html)
            {
                if (item.StartsWith("</"))
                {
                    string closingTag = Regex.Match(
                        item, @"^</(\w+)"
                    ).Groups[1].Value;

                    if (closingTag == "html")
                        break;

                    if (current.Name == closingTag && current.Parent != null)
                        current = current.Parent;

                    continue;
                }

                if (item.StartsWith("<"))
                {
                    HtmlElement element = new HtmlElement();

                    string tagName = Regex.Match(item, @"^<(\w+)").Groups[1].Value;

                    if (!HtmlHelper.Show.AllTags.Contains(tagName))
                        continue;

                    element.Name = tagName;

                    var attributeRegex = new Regex(@"([^\s]*?)=""(.*?)""");

                    foreach (Match item2 in attributeRegex.Matches(item))
                    {
                        string attributeName = item2.Groups[1].Value;
                        string attributeValue = item2.Groups[2].Value;

                        element.Attributes.Add(item2.Value);

                        if (attributeName == "id")
                        {
                            element.Id = attributeValue;
                        }

                        if (attributeName == "class")
                        {
                            element.Classes.AddRange(
                                attributeValue.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            );
                        }
                    }

                    current.Children.Add(element);
                    element.Parent = current;

                    bool selfClosing =
                        item.TrimEnd().EndsWith("/") ||
                        HtmlHelper.Show.SelfClosingTags.Contains(tagName);

                    if (!selfClosing)
                        current = element;

                    continue;
                }

                if (!item.StartsWith("<"))
                {
                    current.InnerHtml += item;
                }
            }

            return root;
        }

        public static string[] SplitHtml(string html)
        {
            string[] tagyot = Regex.Matches(html, @"(<[^>]+>|[^<]+)")
                .Select(m => m.Value.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.StartsWith("<")
                    ? s
                    : Regex.Replace(s, @"\s+", " "))
                .ToArray();

            return tagyot;
        }
    }
}
