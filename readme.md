# Project

This practice-project demonstrates how we can configure `Entity Framework Core` with `.NET 9` to connect to a `MySQL` database and perform some basic `CRUD` operations.

This project is built using the NuGet package: `MySql.EntityFrameworkCore`.

**Database Prerequisites**
- This project assumes you have a `MySQL` server instance running.
- Create a database named `testdb`.
- Create a user:
    ```sql
    CREATE USER 'testuser'@'localhost' IDENTIFIED BY 'p@ssword';
    GRANT ALL PRIVILEGES ON testdb.* TO 'testuser'@'localhost';
    ```
- If you prefer to use an existing db and user, feel free to change the connection strig on line 27 in [HardWareStoreContext](Data/HardWareStoreContext.cs)

# The Repository Pattern

In short, the repository pattern acts as an abstraction layer between the applications **domain** (Program.cs) and the **access layer** (Entity Framework Core).
- A repository is simply a class that implements an `interface` that knows how to `get`, `add`, `update` and `delete`.
- It hides the EFCore details behind how these operations are executed (queries, SaveChanges() etc.).
- The **domain layer** only communicates with the repository, never to the **access layer**. 

## Pros
- Adds separation of concerns
    - Business logic doesn't need to know about how EFCore operates.
- Testability
    - Easy to mock the `Interface` in unit tests and work with fake data instead of a real database.
- Decoupling
    - The application does not depend on EFCore, only the repositories, therefore EFCore can be replaced with a different framework easily, like for example **ADO.NET**.
- Adds abstraction over EF Core / DB
    - Keeps EFCore / Database in the infrastructure layer, away from domains / business logic.

## Cons
- Generates busy work
    - EFCore already has its own implementation of Unit-of-Work + Repository pattern internally through `DbContext + DbSet<T>`
- Over-abstraction
    - Adds more boilerplate

# Usage

1. Clone the repo
2. Install the necessary `Microsoft.EntityFrameworkCore` package files with `dotnet restore`.
3. Run it with `dotnet run`

# Technologies

- .NET 9
- Entity Framework Core 9
- MySql.EntityFrameworkCore
- Repository Pattern + Dependency Injection

# Author
[Yosmel Chiang](https://github.com/yosang)