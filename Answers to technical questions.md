### 1. Things I would do if I had more time

1. Create mapping for every URL in Coinmarketcap
2. Expand ExchangeApiService. Add more capabilities with better models. Using models instead of Dictionary for Query
   Params
3. Expand RestHelper (add more capabilities like Post)
4. Get a list of all crypto currencies (as metadata) and cache it so we can handle errors better, get better user
   experience (users do not have to wait for request to finish if they typed a wrong crypto currency symbol)

### 2. Most useful feature that was added to C# (9.0 or higher) is the target typed new expressions
Because it takes the work to write the same thing over and over out of the way.
```c#
var restHelperRequest = new RestHelperRequestModel
{
    ...
    QueryParams = new() // this is a dictionary type
    {
        { "symbol", request.Symbol! },
        { "convert", fiat }
    }
};
```

### 3. To track down performance issues in production

- Read the logs. Try and track down the issue from here.
- Read application diagnostics to find out which request takes more time to finish (given there is an implementation of
  this)
- Use SQL-profilers to see which query/queries take most time and resources to finish.

### 4. About Technical books and conferences

I have not recently been to any conference or read a book.

### 5. About the assessment

I think it's simple, thorough and makes the candidate think if it's meant to be expanded further. All in all, I think it
is on point. Good job.

### 6. About Me

```json
{
  "umit": {
    "attributes": [
      {
        "glasses": {
          "left": 1.75,
          "right": 1.25
        }
      }
    ],
    "traits": [
      "hard-worker",
      "open-minded",
      "curious"
    ],
    "tags": [
      "programmer",
      "c-sharp",
      "developer"
    ],
    "socials": {
      "linkedin": "https://www.linkedin.com/in/hyo/",
      "github": "https://github.com/umithyo/"
    }
  }
}
```