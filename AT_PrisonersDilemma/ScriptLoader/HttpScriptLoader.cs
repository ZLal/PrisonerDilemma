using HttpParser;
using System.Net;
using System.Text.RegularExpressions;

namespace AT_PrisonersDilemma.ScriptLoader
{
    public class HttpScriptLoader
    {
        public bool IsLoaded { get; private set; } = false;

        private readonly Dictionary<string, string> ScriptVariables = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> ScriptItems = new(StringComparer.OrdinalIgnoreCase);

        public void LoadAssembly(string source)
        {
            MatchCollection matchCollection = Regex.Matches(source, @"###\s?\w*", RegexOptions.Singleline);

            string requestName, requestValue;
            if (matchCollection.Count > 0 && matchCollection[0].Index != 0)
            {
                int splitPos;
                string[] variableSource = source[..matchCollection[0].Index].Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in variableSource)
                {
                    splitPos = str.IndexOf('=');
                    if (str.StartsWith('@') && splitPos > 0)
                    {
                        ScriptVariables.Add(str[1..splitPos], str[(splitPos + 1)..]);
                    }
                }
            }
            for (int i = 0; i < matchCollection.Count; i++)
            {
                requestName = matchCollection[i].Value.Trim();
                if (i + 1 < matchCollection.Count)
                    requestValue = source[matchCollection[i].Index..matchCollection[i + 1].Index];
                else requestValue = source[matchCollection[i].Index..];
                requestValue = requestValue[requestName.Length..].Trim();
                requestName = requestName[3..].Trim();
                ScriptItems.Add(requestName, requestValue);
            }
            IsLoaded = true;
        }

        private string ReplaceVariables(string requestStr, Dictionary<string, string>? parameters)
        {
            MatchCollection matchCollection = Regex.Matches(requestStr, @"{{(\w+)}}", RegexOptions.Singleline);
            foreach (Match match in matchCollection.Cast<Match>())
            {
                string? variable = null;
                if (ScriptVariables.ContainsKey(match.Groups[1].Value))
                    variable = ScriptVariables[match.Groups[1].Value];
                else if (parameters is not null)
                {
                    string? key = parameters.Keys.SingleOrDefault(x => x.Equals(match.Groups[1].Value, StringComparison.OrdinalIgnoreCase));
                    if (key is not null)
                        variable = parameters[key];
                }
                if (variable is not null)
                    requestStr = requestStr.Replace(match.Value, variable, StringComparison.OrdinalIgnoreCase);
            }
            return requestStr;
        }

        public string ExecuteMethod(string name, Dictionary<string, string>? parameters)
        {
            string requestStr = ScriptItems[name];
            requestStr = ReplaceVariables(requestStr, parameters);

            // Format - RFC 7230
            var parsed = Parser.ParseRawRequest(requestStr);
            HttpWebRequest request = HttpWebRequestBuilder.InitializeWebRequest(parsed);
            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Request failed (Status {response.StatusCode})");
            using var sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd();
        }

        public void Test()
        {
            string fileData = @"
@baseURL=https://httpbin.org

### TestRequest

POST {{baseURL}}/anything HTTP/1.1
Content-Type: application/json

{
  ""player"": ""{{player}}"",
  ""opponent"": ""{{opponent}}""
}
";
            fileData = fileData.Trim();
            LoadAssembly(fileData);
            Dictionary<string, string> parameters = new()
            {
                { "Player", "5" },
                { "Opponent", "65" }
            };
            string result = ExecuteMethod("TestRequest", parameters);
            Console.WriteLine(result);
        }
    }
}
