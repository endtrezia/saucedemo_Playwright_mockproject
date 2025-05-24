using System;
using Reqnroll;

namespace saucedemo_Playwright_mockproject.Steps
{
    [Binding]
    public class ShoppingAndPaymentStepDefinitions
    {
        [When("User adds the following products to the cart:")]
        public void WhenUserAddsTheFollowingProductsToTheCart(DataTable dataTable)
        {
            throw new PendingStepException();
        }

        [When("User proceeds to payment steps")]
        public void WhenUserProceedsToPaymentSteps()
        {
            throw new PendingStepException();
        }

        [When("User enters payment information:")]
        public void WhenUserEntersPaymentInformation(DataTable dataTable)
        {
            throw new PendingStepException();
        }

        [When("User continues payment")]
        public void WhenUserContinuesPayment()
        {
            throw new PendingStepException();
        }

        [When("User finishes payment")]
        public void WhenUserFinishesPayment()
        {
            throw new PendingStepException();
        }

        [Then("The payment should be completed successfully")]
        public void ThenThePaymentShouldBeCompletedSuccessfully()
        {
            throw new PendingStepException();
        }
    }
}
