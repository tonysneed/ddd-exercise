# Exercise: Domain Driven Design

DDD Exercise

## Prerequisites
- [.NET Core SDK](https://dotnet.microsoft.com/download) (6.0 or greater)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- MongoDB Docker: `docker run --name mongo -d -p 27017:27017 -v /tmp/mongo/data:/data/db mongo`
- [MongoDB Client](https://robomongo.org/download):
  - Download Robo 3T only.
  - Add connection to localhost on port 27017.
- [Microsoft Tye](https://github.com/dotnet/tye/blob/main/docs/getting_started.md)
- [Specflow](https://specflow.org/) IDE Plugin for either:
  - [Visual Studio](https://docs.specflow.org/projects/getting-started/en/latest/GettingStarted/Step1.html)
  - [JetBrains Rider](https://docs.specflow.org/projects/specflow/en/latest/Rider/rider-installation.html)

> **Note**: Run `docker ps -a` to see if there is a `mongo` container with volumes mapped to the host. If so, run `docker rm -f mongo` to remove it.
>
> Run the Docker command above to create the `mongo` container.

## Steps

1. Build the **before** solution.
   - Open **ddd-before.sln** in Rider (or an IDE of your choice).
   - Build the solution.
     - Ignore build warnings for now.
2. Run user acceptance tests with Tye and SpecFlow.
   - Open a terminal at the **test/EventDriven.ReferenceArchitecture.Specs** directory.
   - Run: `tye run`
   - Open http://localhost:8000/
     - Make sure both CustomerService and OrderService are running.
   - Open the test explorer and run the **EventDriven.ReferenceArchitecture.Specs** acceptance test.
     - All the tests should fail.
3. Run the **CustomerService.Tests** unit tests.
   - All the test should fail.
4. Run the **orderService.Tests** unit tests.
   - All the test should fail.
5. Add `ICustomerRepository.cs` to **CusstomerService/Repositories**.
    ```csharp
    using CustomerService.Domain.CustomerAggregate;

    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAsync();
        Task<Customer?> GetAsync(Guid id);
        Task<Customer?> AddAsync(Customer entity);
        Task<Customer?> UpdateAsync(Customer entity);
        Task<int> RemoveAsync(Guid id);
    }
    ```
6. Add a ctor to `CustomerCommandController`.
   - Add parameters for `ICustomerRepository` and `IMapper`.
7. Flesh out unit tests in **CustomerCommandControllerTests**.
   - Copy code from the **after** solution.
   - Run the CustomerService tests.
     - All test should still fail.
8. Flesh out unit tests in **CustomerQueryControllerTests**.
   - Copy code from the **after** solution.
   - Run the CustomerService tests.
     - All test should still fail.
9. Run the unit tests for **CustomerService.Tests**.
   - The tests should fail but with `NotImplementedException`.
10. Repeat Steps 5-9 for **OrderService** and **OrderService.Tests**.
    - Copy `ICustomerRepository` from the **after** solution to **Repositories/OrderService**.
11. Flesh out `CustomerCommandController` in **CustomerService**.
    - Add a ctor copied from the **after** solution.
    - Flesh out actions by copying code from the **after** solution.
12. Flesh out `CustomerQueryController` in **CustomerService**.
    - Add a ctor copied from the **after** solution.
    - Flesh out actions by copying code from the **after** solution.
13. Run the unit tests for **CustomerService.Tests**.
    - The tests should now pass.
14. Repeat Steps 11-12 for **OrderService**.
15. Run the unit tests for **OrderrService.Tests**.
    - The tests should now pass.
16. Add concete repository classes to **CustomerService** and **OrderService**.
    - Copy code from the **after** solution.
17. Update code in **EventDriven.ReferenceArchitecture.Specs** so that the acceptance tests pass.
    - Look up each `TODO` item and uncomment code.
      - These should be in the `Hooks` and `StepDefinitions` classes.
    - Open a terminal at the Specs directory to run: `tye run`.
    - Run the acceptance tests. They should now pass.
    - Stop Tye with Ctrl+C.
18. Run **CustomerService** directly from the IDE.
    - Use Swagger to execute the GET action.
    - You should get a dependency injection error.
    - Resolve the error by updating `Program` to add database settings.
      - Resolve the namespace for `Customer` to `DomainAggregate.Customer`.
    ```csharp
    builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
    builder.Services.AddMongoDbSettings<CustomerDatabaseSettings, Customer>(builder.Configuration);
    ```
    - Rerun **CustomerService**. The error shold go away.
19. Repeat Step 19 for **OrderService**.