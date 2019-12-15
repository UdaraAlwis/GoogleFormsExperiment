using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoogleFormsSubmitFormData
{
    /// <summary>
    /// Submitting Data to Google Forms 
    /// programatically using the REST API
    /// </summary>
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Submit data to my sample Google Forms page
            // https://docs.google.com/forms/d/e/1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA/viewform

            var googleFormLink = 
            "https://docs.google.com/forms/d/e/" +
            "1FAIpQLSeuZiyN-uQBbmmSLxT81xGUfgjMQpUFyJ4D7r-0zjegTy_0HA" +
            "/formResponse";

            // Given you have retrieved the list of Field Answer Submission IDs 
            // Build the Field Answer Submission Ids and Answer Values dictionary object
            // (replace with your Google Form Ids and answers)
            var formDataDdictionary = new Dictionary<string, string>
            {
                {"entry.1277095329","Orange Snails"}, // Question Field 1

                {"entry.995005981","Banana Plums"}, // Question Field 2

                {"entry.1155533672","Monkeys with hoodies"},  // Question Field 3

                {"entry.1579749043","Jumping Apples"}, // Question Field 4

                {"entry.815399500_year","2019"},  // Question Field 5
                {"entry.815399500_month","11"},
                {"entry.815399500_day","11"},

                {"entry.940653577_hour","04"},  // Question Field 6
                {"entry.940653577_minute","12"},
            };

            await ExecuteGoogleFormsSubmitAsync(formDataDdictionary, googleFormLink);

            Console.ReadKey();

            return 0;
        }

        private static async Task ExecuteGoogleFormsSubmitAsync(
            Dictionary<string, string> formDataDictionary, 
            string googleFormUrl)
        {
            // Init HttpClient to send the request
            HttpClient client = new HttpClient();

            // Encode object to application/x-www-form-urlencoded MIME type
            var content = new FormUrlEncodedContent(formDataDictionary);

            // Post the request (replace with your Google Form link)
            var response = await client.PostAsync(
                googleFormUrl,
                content);

            // Use the StatusCode and Response Content
            Console.WriteLine($"Status : {(int)response.StatusCode} {response.StatusCode.ToString()}");

            Console.WriteLine("Would you like to see the Response Content? y/n");
            var isShowResponseContent = Console.ReadLine();
            if(isShowResponseContent == "y")
                Console.WriteLine($"Body : \n{await response.Content.ReadAsStringAsync()}");
        }
    }
}
