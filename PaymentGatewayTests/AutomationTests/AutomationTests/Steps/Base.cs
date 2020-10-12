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

        public void MerchantInvalidCredentials()
        {
            _restClient = new RestClient("http://localhost:3001/payment_transactions");
            _restClient.Authenticator = new HttpBasicAuthenticator("codemonsr", "my5ecret-key2o2o");
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Content-Type", "application/json;charset=UTF-8");
        }

        public void ExecuteSaleRequest()
        {
            //The request bodies are located in files, which are selected based on the file name passed in the method
            var path = $@"\git\MyRepo\PaymentGatewayTests\AutomationTests\AutomationTests\Files\Sale.txt";
            var jsonText = File.ReadAllText(path);
            var jsonConverted = JsonConvert.DeserializeObject(jsonText);
            var body = _restRequest.AddJsonBody(jsonConverted);
            var restResponse = _restClient.Execute(body);
            _context.RestResponse = restResponse;

            var unique_id = string.Empty;
            if ((int)restResponse.StatusCode == 200 && !GetActualResponse(restResponse.Content).TryGetValue("unique_id", out unique_id))
            {
                return;
            }

            _context.ActualSaleData = restResponse.Content;
            _context.UniqueID = unique_id;
        }

        //Assert the numeric status code of the response.
        public void ResponseIsAsserted(int statusCode)
        {
            int numericStatusCode = (int)_context.RestResponse.StatusCode;
            Assert.That(numericStatusCode, Is.EqualTo(statusCode));
        }

        public void AssertVoidResponseBody(string transactionType,Table table)
        {
            var expectedData = table.FirstTwoColumnsToDictionary().ToDictionary(
                kvp => kvp.Key.TrimSingleSurroundingCharacter('"'),
                kvp => kvp.Value.TrimSingleSurroundingCharacter('"'));

            var actualData = new Dictionary<string, string>();
            if (transactionType == "Sale")
            {
                actualData = GetActualResponse(_context.ActualSaleData);
            }
            else if (transactionType == "Void")
            {
                actualData = GetActualResponse(_context.ActualVoidData);
            }

            actualData.Remove("unique_id");

            Assert.AreEqual(expectedData.Count, actualData.Count);
            foreach (var item in expectedData)
            {
                var actualValue = actualData[item.Key];
                Assert.AreEqual(item.Value, actualValue, $"Expected value was {item.Value}, but was {actualValue}");
            }
        }

        public void ExecuteVoidRequest()
        {
            _restRequest = new RestRequest();
            _restRequest = new RestRequest(Method.POST);
            _restRequest.AddHeader("Content-Type", "application/json;charset=UTF-8");

            var path = $@"\git\MyRepo\PaymentGatewayTests\AutomationTests\AutomationTests\Files\Void.txt";

            var jsonText = File.ReadAllText(path);
            jsonText = jsonText.Replace("$$UniqueID$$", _context.UniqueID);
            var jsonConverted = JsonConvert.DeserializeObject(jsonText);
            var body = _restRequest.AddJsonBody(jsonConverted);
            var restResponse = _restClient.Execute(body);

            _context.RestResponse = restResponse;
            _context.ActualVoidData = restResponse.Content;
        }

        private Dictionary<string,string> GetActualResponse(string actualResponse)
        {
            // Create a dictionary of type string, string from the deserialized json object.
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(actualResponse);
            result.Remove("transaction_time");
            return result;
        }
    }
}
