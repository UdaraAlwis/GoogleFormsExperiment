# GoogleFormsExperiment

This is just me playing around with experimenting with Google Forms pages, extracting the page data programmatically and submitting to the rest endpoints..

### GoogleFormsSubmitFormData - 
Submitting Data to Google Forms programatically using the REST API. Bundle the Form data in a HttpRequest and submit using HttpClient.
### GoogleFormsScrapeOffFieldIDs -
Retriving the list of Field IDs of a given Google Form using HTML content scraping. These Field IDs are required for submitting data to Google Forms using REST API.
### GoogleFormsLoadFormStructure -
Access the complete Google Form structure skeleton programmatically, consisting Question Field data, Answer Options, Answer Submission IDs, etc.. We are parsing the mystery "FB_PUBLIC_LOAD_DATA_ " script content in the HTML, identifying the unique data structure and mapping it to the data fields that we need to extract.

## Blog stories

These little experiements led me to write up the series of blog posts: 
https://theconfuzedsourcecode.wordpress.com/tag/google-forms-hacks/

Post 4. [Programmatically access your complete Google Forms skeleton!](https://theconfuzedsourcecode.wordpress.com/2019/12/15/programmatically-access-your-complete-google-forms-skeleton/)

Post 3. [SCRIPTfully scrape off your Google Forms Field Ids…](https://theconfuzedsourcecode.wordpress.com/2019/11/24/scriptfully-scrape-off-your-google-forms-field-ids/)

Post 2. [You may RESTfully submit to your Google Forms…](https://theconfuzedsourcecode.wordpress.com/2019/11/11/you-may-restfully-submit-to-your-google-forms/)

Post 1. [Let’s auto fill Google Forms with URL parameters…](https://theconfuzedsourcecode.wordpress.com/2019/11/10/lets-auto-fill-google-forms-with-url-parameters/)



