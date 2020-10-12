using RestSharp;

namespace AutomationTests.Steps
{
    public class Context
    {
        public IRestResponse RestResponse { get; set; }

        public string ActualSaleData {get; set;}

        public string ActualVoidData {get; set;}

        public string UniqueID { get; set; }
    }
}
