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

# Concepts covered

## Overview
- LINQ
- Repository Pattern

## Language Integrated Query (LINQ)

LINQ is a set of features that allow us to work with different sources of data with one single uniform query syntax in C#, this can be objects, collections or EFCore context.

Its part of the .NET and C# libraries, so we dont need to install anything in order to use it.

### Usage
There is two ways we can use LINQ:
- `Query syntax` - Very similar to SQL:
    ```c#
    FROM object in source
    WHERE condition
    SELECT object
    ```
- `Method syntax` - Most classes implement the `IEnumerable` and `IQueryable` interfaces, which allows us to use extension methods that implement most LINQ standard query operators.
    ```c#
    Collection.ExtentionMethod(Lambda expression)
    ```

### Providers
LINQ offers so-called **providers**, which is just a fancy word for a translation layer between a `LINQ query` and a specific `data source`. (databases, XML, JSON etc...)

A few of them are:
- LINQ to Objects - Applies for most in-memory collections, such as arrays, lists that implement the `IEnurmeable` interface.
- LINQ to Entities (Entity Framework) - Applies to queries that must be translated to SQL for a variety of databases (MySQL, SQL server, PostgreSQL, SQLite etc...).
- LINQ to SQL (MSSQL) - This is the old school translator (ORM) that only targets SQL Server databases.

There are some **Third-party** providers that we can use if necessary (LINQ to MongoDSB / Azure / Amazon DynamoDB etc...)

In this project we are using `LINQ to Entities (EFCore)` extensively.

For example, in `ToolRepository`, we initiate a `DbContext` property through dependency injection`(DI)` from the domain layer, which is across the entire class.
```c#
    readonly HardwareStoreContext? _db;

    public ToolRepository(HardwareStoreContext db)
    {
        _db = db;
    }
```

Here is one example where we are using the `LINQ to Entity` provider.

```c#
    public double GetAveragePriceForCategory(int categoryId)
    {
        if (_db == null) return 0;

        return _db.Tools
                .Where(t => t.CategoryID == categoryId) // First we join the tables
                .Average(t => t.Price); // Then we find the average price
    }
```

### Nice to know
- `IEnumerable<T>`: Enables local execution (query executes in memory).
- `IQueryable<T>`: Enables remote execution (query executes on the server), becomes `IEnumerable<T>` after `ToList()`, `FirstOrDefault()` etc.
    - The way this works is that `IQueryable`, builds an expression tree (blueprint) and defers execution, until actually used, in for example `ToList` or a `foreach`, which are `IEnumerable` and executes in RAM (local)

For example:
```c#
var query = _db.Tools
    .Where(t => t.CategoryID == 5)
    .OrderBy(t => t.Price)
    .Select(t => new { t.Name, t.Price });
    // At this point, nothing has hit the database yet
    // query is just an `IQueryable<T>` holding an expression tree, its just a blueprint
```

This is the moment the query is actually sent off:
```c#
var results = query.ToLisT(); // From this point, EF builds and sends of SELECT ... WHERE CategoryID = 5 etc...
```

Now results is an `IEnumerable` and we can enumerate it:
```c#
    foreach (var t in query) { ... }
```

In short, the moment we materialize execution, by requesting a list, or `Count()` or `Single()` or `FirstOrDefault()` from EF, only then the query is sent.

#### Why is this important?
Well, If we for some reason materialize too early, for example:
```c#
var noBueno = _db.Tools.ToList()
             .Where(t => t.CategoryID == 5)
```

`noBueno` becomes an `IEnumerable` the moment we materialize with `ToList()`, which means that when we add the `Where()` we are basically sorting `ALL` the tools in memory, instead of in the server.

This is not good for performance if say we got 10,000 tools being filtered in memory...

#### Debugging
In EF Core 5+, we can use `query.ToQueryString()` on any `IQueryable<T>` to see the query string created by EF.

However, this shows the query string UPTO a `IQueryable`, but not for materialized (server) execution, for this we can debug by adding the following to `OnConfiguring`.

```c#
optionsBuilder.LogTo(Console.WriteLine);
```


## The Repository Pattern

In short, the repository pattern acts as an abstraction layer between the applications **domain** (Program.cs) and the **access layer** (Entity Framework Core).
- A repository is simply a class that implements an `interface` that knows how to `get`, `add`, `update` and `delete`.
- It hides the EFCore details behind how these operations are executed (queries, SaveChanges() etc.).
- The **domain layer** only communicates with the repository, never to the **access layer**. 

### Pros
- Adds separation of concerns
    - Business logic doesn't need to know about how EFCore operates.
- Testability
    - Easy to mock the `Interface` in unit tests and work with fake data instead of a real database.
- Decoupling
    - The application does not depend on EFCore, only the repositories, therefore EFCore can be replaced with a different framework easily, like for example **ADO.NET**.
- Adds abstraction over EF Core / DB
    - Keeps EFCore / Database in the infrastructure layer, away from domains / business logic.

### Cons
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