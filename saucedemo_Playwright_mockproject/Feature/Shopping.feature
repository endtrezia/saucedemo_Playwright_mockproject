Feature: Shopping and Payment

Add Product to Cart and perform payment

Background: 
    Given User logged in as a "standard_user" and "secret_sauce"

  Scenario: Add some product to cart and payment successfully
    When User adds the following products to the cart:
      | product                  |
      | Sauce Labs Fleece Jacket |
      | Sauce Labs Bolt T-Shirt  |
      | Sauce Labs Backpack      |
    And User proceeds to payment steps
    And User enters payment information:
      | firstName | lastName | postalCode |
      | Zia      | Nguyen      | H5202      |
    And User continues payment
    And User finishes payment
    Then The payment should be completed successfully