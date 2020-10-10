using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Interfaces;
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

        [When(@"a '(.*)' request is sent to the provider")]
        public void RequestIsSentToTheProvider(string requestType)
        {
            RequestExecution(requestType);
        }

        [Then(@"the response received from the provider has http status code '(.*)'")]
        public void ResponseIsReceivedFromTheProvider(int statusCode)
        {
            ResponseIsAsserted(statusCode);
        }

        [Then(@"the response received from the provider contains the following data:")]
        public void TheResponseContainsTheFollowingData(Table table)
        {
            AssertResponseBody(table);
        }

    }
}
