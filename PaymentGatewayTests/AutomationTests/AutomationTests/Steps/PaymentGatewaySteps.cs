using System;
using TechTalk.SpecFlow;

namespace AutomationTests.Steps
{

    /// <summary>
    /// This is where all feature related steps are described on a higher level - Implementation is in base class.
    /// </summary>

    [Binding]
    public class PaymentGatewaySteps : Base
    {
        private readonly Context _context;
        public PaymentGatewaySteps(Context context) : base(context)
        {
            _context = context;
        }

        [Given("the merchant's credentials are correct")]
        public void TheMerchantsCredentialsAreCorrect()
        {
            MerchantPrerequisites();
        }

        [Given(@"there is an original '(.*)' payment")]
        [When(@"a '(.*)' request is sent to the provider")]
        public void RequestIsSentToTheProvider(string transactionType)
        {            
            if (transactionType == "Sale")
            {
                ExecuteSaleRequest();
            }
            else if (transactionType == "Void")
            {
                ExecuteVoidRequest();
            }
        }

        [Then(@"the response received from the provider has http status code '(.*)'")]
        public void ResponseIsReceivedFromTheProvider(int statusCode)
        {
            ResponseIsAsserted(statusCode);
        }

        [Then(@"the '(.*)' response received from the provider contains the following data:")]
        public void TheResponseContainsTheFollowingData(string transactionType,Table table)
        {
            AssertVoidResponseBody(transactionType,table);
        }

        [Given(@"the original payment is missing")]
        public void GivenTheOriginalPaymentIsMissing()
        {
            _context.UniqueID = Guid.NewGuid().ToString();
        }

        [Given(@"the original payment has already been voided")]
        public void GivenTheOriginalPaymentHasAlreadyBeenVoided()
        {
            ExecuteSaleRequest();
            ExecuteVoidRequest();
        }

        [Given(@"the merchant's credentials are incorrect")]
        public void GivenTheMerchantsCredentialsAreIncorrect()
        {
            MerchantInvalidCredentials();
        }
    }
}
