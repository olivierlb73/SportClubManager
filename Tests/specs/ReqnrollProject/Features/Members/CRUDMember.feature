Feature: Create, Retrieve, Update and Delete a new member

  Scenario: Create and retrieve a new member
    When I create a new member with
      | firstName | lastName | email              |
      | John      | Doe      | john.doe@gmail.com |
    Then I can retrieve the new member
