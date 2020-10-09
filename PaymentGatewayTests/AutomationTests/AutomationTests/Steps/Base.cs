using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

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
            var path = $@"C:\git\MyRepo\PaymentGatewayTests\AutomationTests\AutomationTests\Files\{requestType}.txt";
            var jsonText = File.ReadAllText(path);
            var jsonConverted = JsonConvert.DeserializeObject(jsonText);
            var body = _restRequest.AddJsonBody(jsonConverted);
            var executeRequest = _restClient.Execute(body);
            _context.restResponse = executeRequest;
        }

        public void ResponseIsAsserted(int statusCode)
        {
            int numericStatusCode = (int)_context.restResponse.StatusCode;
            Assert.That(numericStatusCode, Is.EqualTo(statusCode));
        }
    }
}
