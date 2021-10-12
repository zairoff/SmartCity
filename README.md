# SmartCity
### Module: Sport

This project developed with **Onion Architecture** using C# programming language. Followed by `SOLID` principles of Object Oriented Programming and design architecture. We can add new features without affecting application. Each service & class is loosly coupled and not dependent to others.

Project structure:
* SmartCity
    * docs
    * src
      * Sport.API
      * Sport.Service
      * Sport.Domain
      * Sport.Infrastructure
    * tests
      * Sport.API.Tests
      * Sport.Service.Tests
---

* `API` - [API]("https://en.wikipedia.org/wiki/API") for Sport Complex Module, consist of Controllers, Mappers, DTO's, Custom Exctensions, Authentication Services
* `Service` - Consist of Bussines Logic Services
* `Domain` - Consist of pure, [POCO]("https://en.wikipedia.org/wiki/Plain_old_CLR_object") classes
* Infrastructure - Which is data access layer

## Security

Tried to make application as simple as possible. Used only `basic authentication` to make Log-In functionality for administrators

**Authentication** - API secured with basic [JWT]("https://jwt.io/introduction")

**Authorization** - No

**Cross-Origin Resource Sharing (CORS)** - For simplecity allowed any domain

**SSL** - Used `SSL Certificate` to encrypt traffic

## Application use cases

1. Trainee regsitration and enrolling proccess
2. Employee (trainer) hire proccess
3. Registering SportComplex proccess
4. Advertising Sport Complex and publishing events
5. Creating an event in Sport Complex
6. Payment check proccess
7. Education check proccess
8. Notifying other modules about event

## Database design

![database](/docs/images/database.png)

> **Note!** For simplicity considered:

* *SportTypes* (Football, tennis etc..), *Pocket*, *SportGroup* and *Position* same for all Sport Complexes
* For subscription to sport complexes, made simple pockets with pricepermonth
* Subscription duration is ignored like (From July to September on Premium pocket)
* For payment used isPaid or not

**Use-case - 1: Trainee registration and enrolling poroccess**

![use-case](/docs/images/use-case-1.png)

**Use-case - 8: Notifying other modules**

Used simple way for demonstration, like [Fire and Forget](https://en.wikipedia.org/wiki/Fire-and-forget#:~:text=Fire%2Dand%2Dforget%20is%20a,of%2Dsight%20of%20the%20target)

* Created a controller `EventSubscribeController` and `Subscribe` method parametr *url*.
  > **Attention!** Who wants to get notified about an event, they should subscribe to *eventSubscriptionList* in `EventSubscribeController`. The **url** parametr of `Subscribe` method is, subscribers *api.url's* which listens for an event. (**example:** https:10.10.10.10/api/eventlisten)
* Actually we have option to get status of receive by api response, but ignored

> Custom exceptions. Created custome exceptions to handle different exceptions

```csharp
[Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
```
> Custom exctensions. Created custom extension making an object as querable object

```csharp
public static class ObjectToQuerableExtension
    {
        public static IQueryable<T> ToQueryable<T>(this T instance)
        {
            return new[] { instance }.AsQueryable();
        }
    }
```

## Connections to other modules
* City Administration
* People Management
* Education

## Project dependencies
* `Sport.API` is dependent to
    * Sport.Service
  ---
* `Sport.Service` is dependent to
  * Sport.Domain
  * Sport.Infrastructure
  ---
* `Sport.Infrastructure` is dependent to
  * Sport.Domain
  ---
*  `Sport.Domain` is not dependent
  ---
* `Sport.API.Tests` is dependent to
  * Sport.API
  * Sport.Service
  ---
* `Sport.Service.Tests` is dependent to
  * Sport.Domain
  * Sport.Infrastructure
  * Sport.Service
  ---
## Project third party libraries
* Sport.Api
  * [AutoMapper](https://www.nuget.org/packages/AutoMapper/) version 10.1.1
  * [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer) version 5.0.9
  * [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) version 6.2.1
  ---
* Sport.Infrastructure
  * [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/) version 5.0.9
  * [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/) version 5.0.9
  * [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/) version 5.0.9
  * [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools) version 5.0.9
  ---
* Sport.Domain
  * No
  ---
* Sport.Service
    * No

## Tests

**Tested all services and Controllers using xUnit Test. Third party libraries:**
* Used [Moq](https://www.nuget.org/packages/Moq/) version: 4.16.1 to isolate classes from dependencies.
* Used [MockQuerable](https://www.nuget.org/packages/MockQueryable.Moq/) extension for mocking Entity Framework Core operations such ToListAsync, FirstOrDefaultAsync

![xUnit-Test](/docs/images/xUnit.png)

Integration tests done with [Postman]("https://www.postman.com/"). Created `collection` for each methods of **Controller** which makes testability of application easy.

![postman](/docs/images/Postman.png)

Created global variables for `url` and `Token` which makes application loosly coupled to deployed **server**. With easy steps we can achieve our test with any production server.

![postman](/docs/images/Postman-2.png)

## Client

>Client app [`description in client folder`]:

![home](/docs/images/client-1.png)
![sign-in](/docs/images/sign-in.png)
![admin](/docs/images/admin-8.png)
![client](/docs/images/client-5.png)