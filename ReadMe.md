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

> **Note**: Run `docker ps -a` to see if there is a `mongo` container with volumes mapped to the host. If so, run `docker rm -f mongo` to remove it. Then run the **MongoDB Docker** command above to re-create the `mongo` container.

## Steps

> **Note**: This exercise uses a **BDD / TDD** development approach, in which the specification is first defined using SpecFlow acceptance tests via feature files using Gherkin syntax. Enough of the source code is written so that the acceptance and unit tests compile but fail when they are run.
> 
> First code is written in order to make the **unit tests** pass, but the acceptance tests will still fail because the repository classes have not yet been written. Once the repositories are written, the SpecFlow **acceptance tests** will pass.

1. Build the **before** solution.
   - Open **ddd-before.sln** in Rider (or an IDE of your choice).
   - Build the solution.
     - Ignore build warnings for now.
2. Run user acceptance tests with **Tye** and **SpecFlow**.
   >**Note**: While it is possible to run each service directly from the IDE, Tye sets **environment variables** which point to test databases that are populated by the `Hooks` class. You can then set breakpoints and debug source code by attaching to service processes. 
   - Open a terminal at the **test/EventDriven.ReferenceArchitecture.Specs** directory.
   - Run: `tye run`
   - Open http://localhost:8000/
     - Make sure both CustomerService and OrderService are running.
   - Open the test explorer and run the **EventDriven.ReferenceArchitecture.Specs** acceptance test.
     - *All the tests should fail.*
3. Run the **CustomerService.Tests** unit tests.
   - *All the test should fail.*
4. Run the **orderService.Tests** unit tests.
   - *All the test should fail.*
5. Add `ICustomerRepository.cs` to **CustomerService/Repositories**.
    ```csharp
    using CustomerService.Domain.CustomerAggregate;

    namespace CustomerService.Repositories;

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
   - Add fields for `Mock<ICustomerRepository>` and `IMapper`.
     - Initialize them in a ctor.
    ```csharp
    public CustomerCommandControllerTests()
    {
        _repositoryMoq = new Mock<ICustomerRepository>();
        _mapper = MappingHelper.GetMapper();
    }
    ```
   - In each test create a new `CustomerCommandController`, passing in `_repositoryMoq` and `_mapper`.
   - Use `_mapper` to map a `Customer` DTO from a `Customer` entity.
    ```csharp
    var customerOut = _mapper.Map<Customer>(Customers.Customer1);
    ```
   - Call `_repositoryMoq.Setup` to set up the method that is called by the controller.
    ```csharp
    _repositoryMoq.Setup(x => x.AddAsync(It.IsAny<Customer>()))
        .ReturnsAsync(customerOut);
    ```
   - Call the controller method being tested and cast the result.
    ```csharp
    var actionResult = await controller.Create(Customers.Customer1);
    var createdResult = actionResult as CreatedResult;
    ```
   - Assert the correct result was returned.
    ```csharp
    Assert.NotNull(actionResult);
    Assert.NotNull(createdResult);
    Assert.Equal($"api/customer/{customerOut.Id}", createdResult!.Location, true);
    ```
   - Repeat the process for the remaining unit tests for **CustomerService**.
   - Run the **CustomerService** tests.
     - All test should still fail.
8. Flesh out unit tests in **CustomerQueryControllerTests**.
   - Repeat the prior step.
   - Run the CustomerService tests.
     - *All test should still fail.*
9. Run the unit tests for **CustomerService.Tests**.
   - The *tests should fail* with `NotImplementedException`.
10. Repeat *steps 5-9* for **OrderService** and **OrderService.Tests**.
    - Add `IOrderRepository.cs` to **OrderService/Repositories**.
    ```csharp
    using OrderService.Domain.OrderAggregate;

    namespace OrderService.Repositories;

    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAsync();
        Task<IEnumerable<Order>> GetByCustomerAsync(Guid customerId);
        Task<Order?> GetAsync(Guid id);
        Task<Order?> AddAsync(Order entity);
        Task<Order?> UpdateAsync(Order entity);
        Task<Order?> UpdateAddressAsync(Guid orderId, Address address);
        Task<int> RemoveAsync(Guid id);
        Task<Order?> UpdateOrderStateAsync(Order entity, OrderState orderState);
    }
    ```
    - Add a ctor to `OrderCommandController`.
    - Flesh out unit tests in **OrderCommandControllerTests**.
    - Flesh out unit tests in **OrderQueryControllerTests**.
    - Run the unit tests for **OrderService.Tests**.
      - The *tests should fail* with `NotImplementedException`.
11. Flesh out `CustomerCommandController` in **CustomerService**.
    - Add a ctor that accepts `ICustomerRepository` and `IMapper`.
    - Map incoming DTO's to entities.
    ```csharp
    var customerIn = _mapper.Map<Customer>(customerDto);
    ```
    - Use the repository.
    ```csharp
    var result = await _repository.AddAsync(customerIn);
    ```
    - Map the outgoing entities to DTO's.
    ```csharp
    var customerOut = _mapper.Map<DTO.Write.Customer>(result);
    ```
    - Return an action result.
    ```csharp
    return Created($"api/customer/{customerOut.Id}", customerOut);
    ```
    - Repeat for the remaining controller actions.
12. Flesh out `CustomerQueryController` in **CustomerService**.
    - Repeat the prior step for `CustomerQueryController`.
13. Run the unit tests for **CustomerService.Tests**.
    - The tests should now pass.
14. Repeat *steps 11-12* for **OrderService**.
15. Run the unit tests for **OrderService.Tests**.
    - The tests should now pass.
16. Add `CustomerRepository` and `OrderRepository` classes to **CustomerService** and **OrderService** respectively.
    - Add a `[ExcludeFromCodeCoverage]` attribute to the class.
    > **Note**: Repositories are best covered by **integration tests**, *not unit tests*, because the underlying provider cannot be adequately mocked.
    - Extend `DocumentRepository<Customer>` and implement `ICustomerRepository`.
    - Call the base class methods.
    ```csharp
    public async Task<IEnumerable<Customer>> GetAsync() =>
        await FindManyAsync();

    public async Task<Customer?> GetAsync(Guid id) =>
        await FindOneAsync(e => e.Id == id);

    public async Task<int> RemoveAsync(Guid id) =>
        await DeleteOneAsync(e => e.Id == id);
    ```
    - For `AddAsync` and `UpdateAsync` set the entity `ETag`.
    ```csharp
    entity.ETag = Guid.NewGuid().ToString();
    ```
    - For `UpdateAsync` throw a `ConcurrencyException` if the incoming `ETag` does not match the existing one.
    ```csharp
    if (string.Compare(entity.ETag, existing.ETag, StringComparison.OrdinalIgnoreCase) != 0 )
        throw new ConcurrencyException();
    ```
17. Update code in **EventDriven.ReferenceArchitecture.Specs** so that the acceptance tests pass.
    - Look up each `TODO` item and uncomment code.
      - These should be in the `Hooks` and `StepDefinitions` classes.
    - Open a terminal at the Specs directory to run: `tye run`.
    - Run the acceptance tests. *The acceptance tests should now pass.*
    - Stop Tye with Ctrl+C.
18. Run **CustomerService** directly from the IDE.
    - Use Swagger to execute the GET action.
    - You should get a *dependency injection error*.
    - Resolve the error by updating `Program` to add database settings.
      - Be sure to resolve the namespace for `Customer` to `DomainAggregate.Customer` entity, not the `Customer` DTO. 
    ```csharp
    builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
    builder.Services.AddMongoDbSettings<CustomerDatabaseSettings, Customer>(builder.Configuration);
    ```
    - The `AddMongoDbSettings` method maps a class that implements `IMongoDbSettings` to a section in appsettings.json with a name that matches the class name.
      - For example, the `CustomerDatabaseSettings` class has properties which match the `CustomerDatabaseSettings` section in appsettings.json.
    - Rerun **CustomerService**. *The error should go away.*
19. Repeat Step 19 for **OrderService**.
    - Update `Program` to add database settings.
    - You can then execute actions for OrderService via Swagger without getting an error.