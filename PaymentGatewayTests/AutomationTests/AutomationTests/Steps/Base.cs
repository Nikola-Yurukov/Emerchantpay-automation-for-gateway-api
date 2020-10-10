using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using TechTalk.SpecFlow;

namespace AutomationTests.Steps
{
    public class Base
    {
        private readonly Context _context;
        public Base(Context context)
        {
            _context = context;
        }

        private RestRequest _restRequest = new RestRequest();
        private RestClient _restClient = new RestClient();

        public void MerchantPrerequisites()
        {
            _restClient = new RestClient("http://localhost:3001/payment_transactions");
            _restClient.Authenticator = new HttpBasicAuthenticator("codemonster", "my5ecret-key2o2o");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Content-Type", "application/json;charset=UTF-8");
        }

        public void RequestExecution(string requestType)
        {
        //The request bodies are located in files, which are selected based on the file name passed in the method
            var path = $@"\git\MyRepo\PaymentGatewayTests\AutomationTests\AutomationTests\Files\{requestType}.txt";
            var jsonText = File.ReadAllText(path);
            var jsonConverted = JsonConvert.DeserializeObject(jsonText);
            var body = _restRequest.AddJsonBody(jsonConverted);
            var restResponse = _restClient.Execute(body);
            _context.restResponse = restResponse;
        }

        //Assert the numeric status code of the response.
        public void ResponseIsAsserted(int statusCode)
        {
            int numericStatusCode = (int)_context.restResponse.StatusCode;
            Assert.That(numericStatusCode, Is.EqualTo(statusCode));
        }

        public void AssertResponseBody(Table table)
        {
            var expectedData = table.FirstTwoColumnsToDictionary().ToDictionary(
                kvp => kvp.Key.TrimSingleSurroundingCharacter('"'),
                kvp => kvp.Value.TrimSingleSurroundingCharacter('"'));

            string response = _context.restResponse.Content;

            // Create a dictionary of type string, string from the deserialized json object.
            Dictionary<string, string> actualResponse =
            JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            actualResponse.Remove("unique_id");
            actualResponse.Remove("transaction_time");

            Assert.AreEqual(expectedData, actualResponse);
        }
    }
}
