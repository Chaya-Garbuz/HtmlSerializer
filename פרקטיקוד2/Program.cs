using System.Text.RegularExpressions;
using פרקטיקוד2;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

static HtmlElement CreateElement(string[] html)
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

static string[] SplitHtml(string html)
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

static void PrintTree(HtmlElement element, int depth = 0)
{
    Console.WriteLine(new string(' ', depth * 2) + element.Name);

    foreach (var child in element.Children)
    {
        PrintTree(child, depth + 1);
    }
}

static void TestSelector(
    HtmlElement root,
    Selector selector,
    string title)
{
    var results = root.FindElements(selector);

    Console.WriteLine("\n" + title);
    Console.WriteLine("מספר תוצאות: " + results.Count());

    foreach (var result in results)
    {
        Console.WriteLine(
            $"תגית: {result.Name}, ID: {result.Id}, Classes: {string.Join(", ", result.Classes)}"
        );
    }
}

var html = await Load("https://www.w3schools.com/html/html_basic.asp");

var tagyot = SplitHtml(html);
HtmlElement root = CreateElement(tagyot);

var element = root.FindElements(
    Selector.ToSelector("input#filter-certified-input")
).First();

Console.WriteLine("ההורים של האלמנט:");

foreach (var ancestor in element.Ancestors())
{
    Console.WriteLine(ancestor.Name);
}

// יצירת הבדיקות
Selector selector1 = Selector.ToSelector("input#filter-certified-input");
Selector selector2 = Selector.ToSelector("h1.tutnav-main-title");
Selector selector3 = Selector.ToSelector("div input#filter-certified-input");

// הרצת הבדיקות
TestSelector(root, selector1, "בדיקת תגית ו-ID");
TestSelector(root, selector2, "בדיקת תגית ומחלקה");
TestSelector(root, selector3, "בדיקה היררכית");

var descendants = root.Descendants().ToList();

Console.WriteLine("מספר צאצאים כולל השורש: " + descendants.Count);
Console.WriteLine("האלמנט הראשון: " + descendants.First().Name);