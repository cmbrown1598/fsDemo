type PaymentType = 
| Cash
| Check of CheckNumber : int
| CreditCard of CardNumber : string * ExpirationMonth : int * ExpirationYear : int


