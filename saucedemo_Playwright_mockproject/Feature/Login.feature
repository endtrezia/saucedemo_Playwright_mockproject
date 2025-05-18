Feature: Login

Login feature, cheking the login functionality including positive and negative scenarios.

@Login @postive
Scenario: Login with valid credentials
	Given Annonymous user is on the login page
	When User enters valid username "standard_user" and password "secret_sauce"
	Then User should be redirected to the home page

@Login @negative
Rule: User with invalid creedentials shouldn't be able to login

Example: Invalid credentials
	Given Annonymous user is on the login page
	When User enters invalid username "TestUser" and password "1234"
	Then Error message related to Invalid_User will be prompted
	* User should not be redirected to the inventory page

Example: Locked out user
	Given Annonymous user is on the login page
	When User enters locked out username "locked_out_user" and password "secret_sauce"
	Then Error message related to Locked_Out state will be prompted
	* User should not be redirected to the inventory page
