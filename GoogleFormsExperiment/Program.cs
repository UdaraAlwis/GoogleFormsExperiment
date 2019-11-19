using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleFormsExperiment
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            //await ExecuteGoogleFormsSubmitAsync();

            var url = @"https://docs.google.com/forms/d/e/1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA/viewform";
            //var url = @"https://docs.google.com/forms/d/e/1FAIpQLScFM2ZEl1lVERQSoiDbwKggoTilpEdFQx0NNAfmYvJYcL8_TQ/viewform";
            
            //await ScrapeListOfFieldsFromHtmlAsync(url);

            await ScrapeListOfFieldsFromFacebookJsScriptAsync(url);

            Console.ReadKey();

            return 0;
        }

        private static async Task ScrapeListOfFieldsFromFacebookJsScriptAsync(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//script").Where(
                x => x.GetAttributeValue("type", "").Equals("text/javascript") &&
                     x.InnerHtml.Contains("FB_PUBLIC_LOAD_DATA_"));

            var facebookJsScriptContent = htmlNodes.First().InnerHtml;

            // cleaning up "var FB_PUBLIC_LOAD_DATA_ = " at the beginning and 
            // and ";" at the end of the script text  
            var beginIndex = facebookJsScriptContent.IndexOf("[", StringComparison.Ordinal);
            var lastIndex = facebookJsScriptContent.LastIndexOf(";", StringComparison.Ordinal);
            var facebookJsScriptContentCleanedUp = facebookJsScriptContent.Substring(beginIndex, lastIndex - beginIndex).Trim();

            var jArray =  JArray.Parse(facebookJsScriptContentCleanedUp);
            var arrayOfFields = jArray[1][1];

            foreach (var field in arrayOfFields)
            {
                //Console.WriteLine("------" + item.ToString().Replace(Environment.NewLine, ""));

                var question = field[1]; // Question
                Console.WriteLine("------" + question.ToObject<string>());

                var questionTypeCode = field[3].ToObject<GoogleFormsFieldTypeEnum>(); // Question Type Code   
                Console.WriteLine("------" + questionTypeCode);

                var answerSubmitId = field[4][0][0]; // Answer Submit Id
                var answersList = field[4][0][1].ToList(); // Answers List

                Console.WriteLine("------" + answerSubmitId + "\n\n");
            }

            //var splitNodes = facebookJsScriptContent.Split("\n]\n]\n,");

            ////var firstQuestionNodes =  splitNodes[0].Split("\",null,");

            //// Identify Radio Button Question
            //var radioButtonQuestions = splitNodes.Where(x => x.Contains(",null,2,[[")).ToList();
            //foreach (var questionNode in radioButtonQuestions)
            //{
            //    var radioButtonOptions = questionNode.Split(",null,null,null,0]");
            //}
        }

        private static async Task ScrapeListOfFieldsFromHtmlAsync(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//input").Where(
                x => x.GetAttributeValue("name", "").Contains("entry.") &&
                     (x.GetAttributeValue("type", "").Equals("hidden") || x.GetAttributeValue("type", "").Equals("text")) &&
                     !x.GetAttributeValue("name", "").Contains("_sentinel"));

            var htmlNodesList = htmlNodes.ToList();
            var groupedList = htmlNodesList.GroupBy(x => x.OuterHtml);

            var cleanedNodeList = new List<HtmlNode>();
            foreach (var groupedItem in groupedList)
            {
                cleanedNodeList.Add(groupedItem.First());
            }

            foreach (var node in cleanedNodeList)
            {
                //Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml + "\n");
                Console.WriteLine(node.GetAttributeValue("name", ""));
            }
        }

        private static async Task ExecuteGoogleFormsSubmitAsync()
        {
            //HttpClient client = new HttpClient();

            //var bodyValues = new Dictionary<string, string>
            //{
            //    {"entry.1277095329","Orange Snails"},

            //    {"entry.995005981","Banana Plums"},

            //    {"entry.1155533672","Monkeys with hoodies"},

            //    {"entry.1579749043","Jumping Apples"},

            //    {"entry.815399500_year","2019"},
            //    {"entry.815399500_month","11"},
            //    {"entry.815399500_day","11"},

            //    {"entry.940653577_hour","04"},
            //    {"entry.940653577_minute","12"},
            //};

            //var content = new FormUrlEncodedContent(bodyValues);

            //var response = await client.PostAsync(
            //    "https://docs.google.com/forms/d/e/" +
            //    "1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA" +
            //    "/formResponse",
            //    content);

            //Console.WriteLine($"Status : {(int)response.StatusCode} {response.StatusCode.ToString()}");
            //Console.WriteLine($"Body : \n{await response.Content.ReadAsStringAsync()}");
        }
	}
}
