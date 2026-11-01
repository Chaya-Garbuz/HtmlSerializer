using System.Collections.Generic;
using System.Linq;

namespace פרקטיקוד2
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> FindElements(
            this HtmlElement element,
            Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();

            FindElementsRecursive(element, selector, results);

            return results;
        }

        private static void FindElementsRecursive(
            HtmlElement element,
            Selector selector,
            HashSet<HtmlElement> results)
        {
            var matchingElements = element.Descendants()
                .Where(el => el != element && el.Matches(selector));

            if (selector.Child == null)
            {
                foreach (var item in matchingElements)
                {
                    results.Add(item);
                }
            }
            else
            {
                foreach (var item in matchingElements)
                {
                    FindElementsRecursive(item, selector.Child, results);
                }
            }
        }
    }
}