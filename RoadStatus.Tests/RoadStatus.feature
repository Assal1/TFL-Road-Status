Feature: Road Status

Scenario: Display Name should be displayed
	Given a valid road ID 'A2' is specified
	When the client is run with a valid road ID
	Then the road displayName should be displayed

Scenario: Road Status should be displayed
	Given a valid road ID 'A2' is specified
	When the client is run with a valid road ID
	Then the road statusSeverity should be displayed as Road Status

Scenario: Road Status Description should be displayed
	Given a valid road ID 'A2' is specified
	When the client is run with a valid road ID
	Then the road statusSeverityDescription should be displayed as Road Status Description

Scenario: Invalid road ID should display an informative message
	Given an invalid road ID 'A233' is specified
	When the client is run with an invalid road ID
	Then the application should return an informative error

Scenario: Invalid road ID should return nonzero exit code
	Given an invalid road ID 'A233' is specified
	When the client is run with an invalid road ID
	Then the application should exit with a non-zero System Error code