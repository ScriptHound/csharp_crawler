using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Path;

namespace JSON_PG.Controllers;
class JsonDocumentsLogic
{
    public static List<Dictionary<string, string>> FilterJson(string json, Dictionary<string, string> filters)
    {
        var instance = JsonNode.Parse(json);
        var jsonData = new List<Dictionary<string, string>> { };
        var foundData = new Dictionary<string, List<string>> { };

        // creates pandas Dataframe-like structure with keys as tags
        foreach (KeyValuePair<string, string> pair in filters)
        {
            var tag = pair.Key;
            var pathString = pair.Value;
            var path = JsonPath.Parse(pathString);
            var result = path.Evaluate(instance).Matches;

            var resultsList = new List<string> { };
            foundData.Add(tag, resultsList);
            foreach (var resultMember in result)
            {
                var resultString = resultMember.Value.ToString();
                if (resultString == null)
                {
                    resultString = "null";
                }
                resultsList.Add(resultString);
            }
        }

        var foundDataValueLength = foundData.Values.First().Count;

        // converts pandas Dataframe-like structure to List of Dictionaries
        // for easier serialization to JSON
        for (int i = 0; i < foundDataValueLength; i++)
        {
            var data = new Dictionary<string, string> { };
            foreach (KeyValuePair<string, List<string>> pair in foundData)
            {
                var tag = pair.Key;
                var result = pair.Value[i];
                data.Add(tag, result);
            }
            jsonData.Add(data);
        }
        return jsonData;
    }
}
