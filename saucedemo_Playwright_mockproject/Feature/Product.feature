﻿Feature: Product Listing and Details

Product features including the product listing, sorting functionality, and product details page.

  Scenario: Verify all products are listed with correct names, description, and prices
    Given User logged in as a "standard_user" and "secret_sauce"
    When User view the product list
    Then User should see all products with correct names, description, and prices

  Scenario: Check sorting functionality by price and name
    Given User logged in as a "standard_user" and "secret_sauce"
    When User sort products by "Price (low to high)"
    Then Products should be sorted by price in ascending order
    When User sort products by "Price (high to low)"
    Then products should be sorted by price in descending order
    When User sort products by "Name (A to Z)"
    Then products should be sorted alphabetically by name

  Scenario: Verify product details page opens correctly
    Given User logged in as a "standard_user" and "secret_sauce"
    When User click on the <product>
    Then The product details page should display the correct product information
    Examples: 
      | product         |
      | Sauce Labs Backpack |
      | Sauce Labs Bike Light |
      | Sauce Labs Bolt T-Shirt |
      | Sauce Labs Fleece Jacket |
      | Sauce Labs Onesie |