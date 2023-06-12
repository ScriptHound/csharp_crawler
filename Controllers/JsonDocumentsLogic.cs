using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Path;

namespace JSON_PG.Controllers;
class JsonDocumentsLogic
{

    private static int GetTheLongestColumnInDataFrameLike(Dictionary<string, List<string>> data)
    {
        var longestColumn = 0;
        foreach (KeyValuePair<string, List<string>> pair in data)
        {
            var columnLength = pair.Value.Count;
            if (columnLength > longestColumn)
            {
                longestColumn = columnLength;
            }
        }
        return longestColumn;
    }
    public static List<Dictionary<string, string>> FilterJson(string json, Dictionary<string, string> filters)
    {
        var instance = JsonNode.Parse(json);
        var resultingDataToJsonSerialization = new List<Dictionary<string, string>> { };
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
                resultsList.Add(resultString);
            }
        }

        var foundDataValueLength = GetTheLongestColumnInDataFrameLike(foundData);

        // converts pandas Dataframe-like structure to List of Dictionaries
        // for easier serialization to JSON
        for (int i = 0; i < foundDataValueLength; i++)
        {
            var data = new Dictionary<string, string> { };
            foreach (KeyValuePair<string, List<string>> pair in foundData)
            {
                var tag = pair.Key;
                if (i >= pair.Value.Count)
                {
                    data.Add(tag, "");
                }
                else
                {
                    var result = pair.Value[i];
                    data.Add(tag, result);
                }
            }
            resultingDataToJsonSerialization.Add(data);
        }
        return resultingDataToJsonSerialization;
    }
}
