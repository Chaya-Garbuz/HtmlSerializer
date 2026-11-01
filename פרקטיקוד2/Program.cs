using פרקטיקוד2;

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

var html = await HtmlSerializer.Load("https://www.w3schools.com/html/html_basic.asp");

var tagyot = HtmlSerializer.SplitHtml(html);
HtmlElement root = HtmlSerializer.CreateElement(tagyot);

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