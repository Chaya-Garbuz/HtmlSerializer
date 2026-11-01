using System.Text.RegularExpressions;

namespace פרקטיקוד2
{
    public class Selector
    {
        public string? TagName { get; set; }
        public string? Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector? Parent { get; set; }
        public Selector? Child { get; set; }

        public static Selector ToSelector(string sql)
        {
            Selector selector = new Selector();
            Selector current = selector;

            var x = sql.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            );

            for (int i = 0; i < x.Length; i++)
            {
                string[] parts = Regex.Split(x[i], @"(?=[#.])");

                foreach (string part in parts)
                {
                    if (part.StartsWith("#"))
                    {
                        current.Id = part.Substring(1);
                    }
                    else if (part.StartsWith("."))
                    {
                        current.Classes.Add(part.Substring(1));
                    }
                    else if (HtmlHelper.Show.AllTags.Contains(part))
                    {
                        current.TagName = part;
                    }
                }

                if (i < x.Length - 1)
                {
                    current.Child = new Selector();
                    current.Child.Parent = current;
                    current = current.Child;
                }
            }

            return selector;
        }
    }
}
