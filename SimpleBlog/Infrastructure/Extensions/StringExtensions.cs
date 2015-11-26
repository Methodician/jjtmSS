using System.Text.RegularExpressions;
using Microsoft.Ajax.Utilities;

namespace SimpleBlog.Infrastructure.Extensions
{
    public static class StringExtensions // String extensions classes must be static
    {
        public static string Slugify(this string that) // Now he says that "to make it an extension of strings we write "this string that" but I wonder if it's flexible. Though I see no reason not to use this convention.
        {
            that = Regex.Replace(that, @"[^a-zA-Z0-9\s]", "");
            that = that.ToLower();
            that = Regex.Replace(that, @"\s", "-");
            return that;
        }
    }
}