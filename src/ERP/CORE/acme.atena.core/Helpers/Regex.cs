using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.core.Helpers
{
    public static class Regex
    {
        public static string RegexFilter(this string pattern, string input)
        {
            string result = String.Empty;

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            foreach (System.Text.RegularExpressions.Match match in regex.Matches(input))
            {
                foreach (System.Text.RegularExpressions.Capture capture in match.Captures)
                {
                    result += capture.Value;
                }
            }

            return result;
        }

        public static Dictionary<string, string> MatchNamedCaptures(this string pattern, string input)
        {
            return MatchNamedCaptures(new System.Text.RegularExpressions.Regex(pattern), input);
        }

        public static Dictionary<string, string> MatchNamedCaptures(this System.Text.RegularExpressions.Regex regex,
            string input)
        {
            var namedCaptureDictionary = new Dictionary<string, string>();
            var groups = regex.Match(input).Groups;
            var groupNames = regex.GetGroupNames();
            foreach (var groupName in groupNames)
                if (groups[groupName].Captures.Count > 0)
                    namedCaptureDictionary.Add(groupName, groups[groupName].Value);
            return namedCaptureDictionary;
        }
    }
}

