using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using GoogleFormsExperiment.Models;

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

            var description = jArray[1][0].ToObject<string>();
            var title = jArray[3].ToObject<string>();
            var formId = jArray[14].ToObject<string>();
            
            Console.WriteLine("\n");
            Console.WriteLine("Title: " + title);
            Console.WriteLine("Description: " + description);
            Console.WriteLine("Form ID: " + formId);
            Console.WriteLine("\n");
            
            var arrayOfFields = jArray[1][1];

            foreach (var field in arrayOfFields)
            {
                // Check if this Field is submittable or not
                // ex: Image banner fields are not submittable
                if (field.Count() > 4 && field[4].HasValues)
                {
                    GoogleFormField googleFormField = new GoogleFormField();

                    var answerSubmitId = field[4][0][0]; // Get Answer Submit Id
                    googleFormField.SubmissionId = answerSubmitId.ToObject<string>();

                    var question = field[1]; // Question
                    googleFormField.QuestionString = question.ToObject<string>();

                    var questionTypeCodeString = field[3].ToObject<int>(); // Question Type Code   
                    var isRecognizedFieldType = Enum.TryParse(questionTypeCodeString.ToString(), out GoogleFormsFieldTypeEnum questionTypeCode);
                    googleFormField.Type = questionTypeCode;

                    var answerOptionsList = field[4][0][1].ToList(); // Get Answers List
                    // List of Answers Available
                    if (answerOptionsList.Count > 0) 
                    {
                        foreach (var answerOption in answerOptionsList)
                        {
                            googleFormField.AnswerList.Add(answerOption[0].ToString());
                        }
                    }

                    // Printing Field Data
                    Console.WriteLine("QUESTION: " + googleFormField.QuestionString);
                    Console.WriteLine("TYPE: " + googleFormField.Type);
                    if (googleFormField.AnswerList.Count > 0)
                    {
                        Console.WriteLine("ANSWER LIST: ");
                        foreach (var answerOption in googleFormField.AnswerList)
                        {
                            Console.WriteLine($"-{answerOption.ToString()}");
                        }
                    }
                    Console.WriteLine("SUBMITID: " + googleFormField.SubmissionId + "\n\n");


                    Console.WriteLine("----------------------------------------\n\n");
                }
            }
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
