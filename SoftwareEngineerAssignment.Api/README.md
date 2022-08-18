# How to run
This solution contains two projects:
- Api
- Tests

## To run it just go to the sln folder and type
`dotnet run --project .\SoftwareEngineerAssignment.Api\SoftwareEngineerAssignment.Api.csproj`

This will run the Api project on __https://localhost:7285__ or __http://localhost:5285__.
The api has only one endpoint at __localhost/givemeadvice__ which receives a POST with an object on the form of:
    
    {
        "topic": "cars",
        "amount": 2,
    }

    
