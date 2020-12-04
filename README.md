# CoterieBackendProject

## Instructions

**To first get it running:**
1. Clone the repo
2. Open the solution in Visual Studio
3. Run IIS Express

The browser will open at the URL: https://localhost:44314/api/quote. No GET call was implemented, so there won't be anything there. I didn't want to implement something that shouldn't belong in the API.

**To test:**

In the repo there's a file called `CoterieBackendProject.postman_collection.json`. In Postman you should be able to import that file and get the following 3 requests:
* PostPremium
* PostNewStates
* PostNewBusinesses

`PostNewStates` and `PostNewBusinesses` should be run *before* `PostPremium` in order to populate the In-Memory database with states and businesses. To run get the Total Premium, run `PostPremium`.

If the postman collection file doesn't work, you can populate the database with the following:

~~~~
POST: https://localhost:44314/api/quote/states
Body:
[
    {
        "name": "TX",
        "factor": 0.943
    },
    {
        "name": "OH",
        "factor": 1
    },
    {
        "name": "FL",
        "factor": 1.2
    }
]

POST: https://localhost:44314/api/quote/businesses
Body:
[
    {
        "name": "Plumber",
        "factor": 0.5
    },
        {
        "name": "Architect",
        "factor": 1
    },
        {
        "name": "Programmer",
        "factor": 1.25
    }
]
~~~~

You can then call the PostPremium endpoint:

~~~~
POST: https://localhost:44314/api/quote
Body:
{
    "revenue": 6000000,
    "state": "TX",
    "business": "Plumber"
}
~~~~

## Next Steps
There's a bunch of things that I would improve upon with more time:
* I would implement a persistent **database** instead of the In-Memory SQL one that I used. My experience is mostly with Azure resources, so I would create a SQL database in Azure. After creating the database, to connect it with the API, I would add the connection string to the Azure App Service that the API is in. The connection string can be fairly barebones with just the database and server name as long as Managed Identity is enabled. In terms of code, QuoteContext and Startup.cs will need to be updated with more configurations.
* Something else I would add is **unit tests**. Within the same solution, I would create a Tests project. Tests for the controller would be really simple, mostly just checking that responses are what we expect. The service tests would be testing the business logic, in this case the total premium calculation. Cases to try include the happy path case and edge cases where maybe the given state or business is invalid. The database can be mocked with an In-Memory database.
* I would add some form of **authentication** so the API isn't accessible by just anyone. To do this I would do implement authorization code flow with Azure Active Directory (since this is the only authentication type I have experience with) by updating Startup.cs with the authentication scheme and its audience, scope, and other configurations.
* More **validation** and **error detection**. This goes with both receiving and sending data. When we receive the payload, it would be best to check that the fields are valid. Since my implementation requires the user to add states and businesses, I should verify that the inputs meet whatever constraints state and business data have. For instance, if the state name must be 2 characters and all caps, I should handle for cases where the input violates that, whether that's returning an error or implementing a method to truncate and capitalize the string. Another constraint I would imagine makes sense is that the premium is non-negative.
* A cleaner **architecture**. Currently, I'm injecting the QuoteContext into both the controller and service. Ideally I would like to only inject it into the service layer. I left it in the controller because of the `PostNewStates` and `PostNewBusinesses` methods. I'm not sure if logically those 2 methods belong in the same controller as `PostPremium`, but I didn't want to overarchitect or overcomplicate this. I would get some clarity on that and then remove the QuoteContext dependency injection from the controller, so that it's only in the service layer.

All of these are how I would like to do things, but I recognize that there will always be hiccups on the way to getting these outcomes! So more research and trial and error are part of all of these bullets.
