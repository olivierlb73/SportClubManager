Feature: Create, Retrieve, Update and Delete a new member

  Scenario: Create, Retrieve, Update and Delete a new member
    When I create a new member with
      | firstName | lastName | email              |
      | John      | Doe      | john.doe@gmail.com |
    Then I can retrieve the new member
    And I can retrieve the list of members
    When I update the new member with
      | firstName | lastName | email                   |
      | Johnny    | Begood   | johnny.begood@gmail.com |
    Then I can retrieve the new member
    When I delete the new member
    Then I can not retrieve the new member
