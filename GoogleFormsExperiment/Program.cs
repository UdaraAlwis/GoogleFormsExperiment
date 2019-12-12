using GoogleFormsExperiment.Helpers;
using GoogleFormsExperiment.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleFormsExperiment
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            //await ExecuteGoogleFormsSubmitAsync();

            var url = @"https://docs.google.com/forms/d/e/1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA/viewform";
            //var url = @"https://docs.google.com/forms/d/e/1FAIpQLScFM2ZEl1lVERQSoiDbwKggoTilpEdFQx0NNAfmYvJYcL8_TQ/viewform";

            //await ScrapeOffListOfFieldIdsFromGoogleFormsAsync(url);

            await ScrapeOffFormSkeletonFromGoogleFormsAsync(url);

            Console.ReadKey();

            return 0;
        }

        private static async Task ScrapeOffFormSkeletonFromGoogleFormsAsync(string yourGoogleFormsUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(yourGoogleFormsUrl);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//script").Where(
                x => x.GetAttributeValue("type", "").Equals("text/javascript") &&
                     x.InnerHtml.Contains("FB_PUBLIC_LOAD_DATA_"));

            var fbPublicLoadDataJsScriptContent = htmlNodes.First().InnerHtml;

            // cleaning up "var FB_PUBLIC_LOAD_DATA_ = " at the beginning and 
            // and ";" at the end of the script text  
            var beginIndex = fbPublicLoadDataJsScriptContent.IndexOf("[", StringComparison.Ordinal);
            var lastIndex = fbPublicLoadDataJsScriptContent.LastIndexOf(";", StringComparison.Ordinal);
            var fbPublicJsScriptContentCleanedUp = fbPublicLoadDataJsScriptContent
                                                        .Substring(beginIndex, lastIndex - beginIndex).Trim();

            var jArray = JArray.Parse(fbPublicJsScriptContentCleanedUp);

            var description = jArray[1][0].ToObject<string>();
            var title = jArray[1][8].ToObject<string>();
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
                // index [4] contains the Field Answer 
                // Submit Id of a Field Object 
                // ex: ignore Fields used as Description panels
                // ex: ignore Image banner fields
                if (field.Count() > 4 && field[4].HasValues)
                {
                    var questionString = field[1].ToObject<string>(); ; // Question

                    var questionTypeCodeString = field[3].ToObject<int>(); // Question Type Code   
                    var isRecognizedFieldType = Enum.TryParse(questionTypeCodeString.ToString(), out GoogleFormsFieldTypeEnum questionTypeCode);
                    var type = questionTypeCode.GetDescription();

                    List<string> answerOptionList = new List<string>();
                    var answerOptionsList = field[4][0][1].ToList(); // Get Answers List
                    // List of Answers Available
                    if (answerOptionsList.Count > 0)
                    {
                        foreach (var answerOption in answerOptionsList)
                        {
                            answerOptionList.Add(answerOption[0].ToString());
                        }
                    }

                    var answerSubmitId = field[4][0][0].ToObject<string>(); ; // Get Answer Submit Id
                    var isAnswerRequiredObject = field[4][0][2]; // Get if Answer is Required to be Submitted
                    var isAnswerRequired = isAnswerRequiredObject.ToObject<int>() == 1 ? true : false; // 1 or 0

                    // Printing Field Data
                    Console.WriteLine("QUESTION: " + questionString);
                    Console.WriteLine("TYPE: " + type);
                    Console.WriteLine("IS REQUIRED: " + (isAnswerRequired ? "YES" : "NO"));
                    if (answerOptionList.Count > 0)
                    {
                        Console.WriteLine("ANSWER LIST: ");
                        foreach (var answerOption in answerOptionList)
                        {
                            Console.WriteLine($"-{answerOption.ToString()}");
                        }
                    }
                    Console.WriteLine("SUBMITID: " + answerSubmitId + "\n\n");

                    Console.WriteLine("----------------------------------------\n\n");
                }
            }
        }

        private static async Task<List<string>> ScrapeOffListOfFieldIdsFromGoogleFormsAsync(string yourGoogleFormsUrl)
        {
            //HtmlWeb web = new HtmlWeb();
            //var htmlDoc = await web.LoadFromWebAsync(yourGoogleFormsUrl);

            //// Select the "input", "textarea" elements from the html content
            //var fields = new[] { "input", "textarea" }; // two types of fields
            //var htmlNodes = htmlDoc.DocumentNode.Descendants().
            //                    Where(x => fields.Contains(x.Name));

            //// Filter out the elements we need
            //htmlNodes = htmlNodes.Where(
            //    x =>
            //    // Get all that elements contains "entry." prefix in the name
            //    x.GetAttributeValue("name", "").Contains("entry.") &&
            //    // Ignored the "_sentinel" elements rendered for checkboxes fields
            //    !x.GetAttributeValue("name", "").Contains("_sentinel"));

            //// remove any duplicates (possibly caused by Checkboxes Fields)
            //var groupedList = htmlNodes.GroupBy(x => x.OuterHtml);
            //var cleanedNodeList = new List<HtmlNode>();
            //foreach (var groupedItem in groupedList)
            //{
            //    cleanedNodeList.Add(groupedItem.First());
            //}

            //// retrieve the Fields list
            var fieldIdList = new List<string>();
            //foreach (var node in cleanedNodeList)
            //{
            //    // grab the Field Id
            //    var fieldId = node.GetAttributeValue("name", "");
            //    fieldIdList.Add(fieldId);
            //    Console.WriteLine(fieldId);
            //}

            return fieldIdList;
        }

        private static async Task ExecuteGoogleFormsSubmitAsync()
        {
            //// Init HttpClient to send the request
            //HttpClient client = new HttpClient();

            //// Build the Field Ids and Answers dictionary object
            //// (replace with your Google Form Ids and answers)
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

            //// Encode object to application/x-www-form-urlencoded MIME type
            //var content = new FormUrlEncodedContent(bodyValues);

            //// Post the request (replace with your Google Form link)
            //var response = await client.PostAsync(
            //    "https://docs.google.com/forms/d/e/" +
            //    "1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA" + 
            //    "/formResponse",
            //    content);

            //// Use the StatusCode and Response Content
            //Console.WriteLine($"Status : {(int)response.StatusCode} {response.StatusCode.ToString()}");
            //Console.WriteLine($"Body : \n{await response.Content.ReadAsStringAsync()}");
        }
    }
}
