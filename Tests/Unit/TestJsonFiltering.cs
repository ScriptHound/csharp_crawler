using Xunit;

using JSON_PG.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MyUnitTests
{
    public class TestFilteringSuite
    {
        [Fact]
        public void TestFilteringWithSingleJsonObject()
        {
            var filters = new Dictionary<string, string> {
                {"mytag", "$.name"},
            };
            var myJson = @"{
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
            }";
            var result = JsonDocumentsLogic.FilterJson(myJson, filters);
            Assert.True(result.Count == 1);
            Assert.True(result[0]["mytag"] == "John");
        }

        [Fact]
        public void TestFilteringWithMultipleJsonObjects()
        {
            var filters = new Dictionary<string, string> {
                {"mytag", "$..name"},
            };
            var myJson = @"
            [
                {
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
                },
                {
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
                }
            ]";
            var result = JsonDocumentsLogic.FilterJson(myJson, filters);
            Assert.True(result.Count == 2);
            Assert.True(result[0]["mytag"] == "John");
            Assert.True(result[1]["mytag"] == "John");
        }

        [Fact]
        public void TestFilteringWithMultipleJsonObjectsAndFilters()
        {
            var filters = new Dictionary<string, string> {
                {"mytag", "$..name"},
                {"city", "$..city"}
            };
            var myJson = @"
            [
                {
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
                },
                {
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
                }
            ]";
            var result = JsonDocumentsLogic.FilterJson(myJson, filters);
            Assert.True(result.Count == 2);
            Assert.True(result[0]["mytag"] == "John");
            Assert.True(result[1]["mytag"] == "John");
        }

        [Fact]
        public void TestFilteringWithNonExistentFilters()
        {
            var filters = new Dictionary<string, string> {
                {"mytag", "$..name"},
                {"city", "$..city"}
            };
            var myJson = @"
            [
                {
                ""name"": ""John"",
                ""age"": 31
                },
                {
                ""name"": ""John"",
                ""age"": 31,
                ""city"": ""New York""    
                }
            ]";
            var result = JsonDocumentsLogic.FilterJson(myJson, filters);
            Assert.True(result.Count == 2);
            Assert.True(result[0]["mytag"] == "John");
            Assert.True(result[1]["mytag"] == "John");
        }

    }
}

