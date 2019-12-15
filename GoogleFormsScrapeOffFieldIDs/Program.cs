using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleFormsScrapeOffFieldIDs
{
    /// <summary>
    /// Retriving the list of Field IDs of a 
    /// given Google Form using HTML content scraping
    /// </summary>
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Retrieve the Field ID List of my sample Google Forms page
            // https://docs.google.com/forms/d/e/1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA/viewform

            // Each Question Field in your Google Form has a unique identier
            // These IDs are required to submit data for each Field in Google Forms
            // We need to bundle Answer data against those Field Answer Submission Ids
            // When we Submit data to our Google Form programmatically

            var googleFormLink =
            "https://docs.google.com/forms/d/e/" +
            "1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA" +
            "/formResponse";

            await ScrapeOffListOfFieldIdsFromGoogleFormsAsync(googleFormLink);

            Console.ReadKey();

            return 0;
        }

        private static async Task<List<string>> ScrapeOffListOfFieldIdsFromGoogleFormsAsync(string yourGoogleFormsUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(yourGoogleFormsUrl);

            // Select the "input", "textarea" elements from the html content
            var fields = new[] { "input", "textarea" }; // two types of fields
            var htmlNodes = htmlDoc.DocumentNode.Descendants().
                                Where(x => fields.Contains(x.Name));

            // Filter out the elements we need
            htmlNodes = htmlNodes.Where(
                x =>
                // Get all that elements contains "entry." prefix in the name
                x.GetAttributeValue("name", "").Contains("entry.") &&
                // Ignored the "_sentinel" elements rendered for checkboxes fields
                !x.GetAttributeValue("name", "").Contains("_sentinel"));

            // remove any duplicates (possibly caused by Checkboxes Fields)
            var groupedList = htmlNodes.GroupBy(x => x.OuterHtml);
            var cleanedNodeList = new List<HtmlNode>();
            foreach (var groupedItem in groupedList)
            {
                cleanedNodeList.Add(groupedItem.First());
            }

            // retrieve the Fields list
            var fieldIdList = new List<string>();
            foreach (var node in cleanedNodeList)
            {
                // grab the Field Id
                var fieldId = node.GetAttributeValue("name", "");
                fieldIdList.Add(fieldId);
                Console.WriteLine(fieldId);
            }

            return fieldIdList;
        }
    }
}
