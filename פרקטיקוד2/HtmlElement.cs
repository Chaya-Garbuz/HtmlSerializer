using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace פרקטיקוד2
{
    public class HtmlElement
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement? Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();

                yield return current;

                foreach (HtmlElement child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = Parent;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public bool Matches(Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) &&
                Name != selector.TagName)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.Id) &&
                Id != selector.Id)
            {
                return false;
            }

            foreach (string className in selector.Classes)
            {
                if (!Classes.Contains(className))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
