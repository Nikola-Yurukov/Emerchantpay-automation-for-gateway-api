﻿Feature: PaymentGateway
	As a payment gateway
	I want to enable merchants to send credit card requests through provider Emerchantpay
	So that clients can deposit to their accounts

Scenario:01.Successful sale transaction
	Given the merchant's credentials are correct
	When a 'Sale' request is sent to the provider
	Then the response received from the provider has http status code '200'
	And the 'Sale' response received from the provider contains the following data:
		| Field   | Value                               |
		| status  | approved                            |
		| usage   | Coffeemaker                         |
		| amount  | 500                                 |
		| message | Your transaction has been approved. |

Scenario:02.Successful void transaction
	Given the merchant's credentials are correct
	And there is an original 'Sale' payment
	When a 'Void' request is sent to the provider
	Then the response received from the provider has http status code '200'
	And the 'Void' response received from the provider contains the following data:
		| Field     | Value                                         |
		| status    | approved                                      |
		| usage     | Coffeemaker                                   |
		| amount    | 500                                           |
		| message   | Your transaction has been voided successfully |

Scenario:03. Authorization Denied
	Given the merchant's credentials are incorrect
	When a 'Sale' request is sent to the provider
	Then the response received from the provider has http status code '401'

Scenario:04.Unsuccessful void transaction due to payment already voided
	Given the merchant's credentials are correct
	And the original payment has already been voided
	When a 'Void' request is sent to the provider
	Then the response received from the provider has http status code '422'

Scenario:05. Unsuccessful void transaction due to missing original payment
	Given the merchant's credentials are correct
	And the original payment is missing
	When a 'Void' request is sent to the provider
	Then the response received from the provider has http status code '422'