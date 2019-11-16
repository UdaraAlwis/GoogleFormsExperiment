using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleFormsExperiment
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            //await ExecuteGoogleFormsSubmitAsync();

            var html = @"https://docs.google.com/forms/d/e/1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA/viewform";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);

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

            Console.ReadKey();

            return 0;
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
