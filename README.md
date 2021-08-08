# Currency Conversion API

This API purpose is convert a currency to another, using an external API to obtain the conversion rate. 
It's possible to list the available currencies allowed to conversion. Also, all the conversion transactions are available to list, in two separated endpoints, one for success and another to failure transactions.

## Tools used in development, and why to use

- ASP.NET Core (.NET 5) - It's one the most used open source web frameworks, nowadays;
- Docker Compose (Docker 3.4) - allows multi-container applications, making environment setup easy;
- MongoDB - most used documents database, easy to run over docker;
- ReSharper - powerful lint and time-saving tool;
- Open API (Swagger) - Rest API standard to present the application in a pre-defined way, regardless the programming language.
- RestSharp - easy-to-use REST API client;
- xunit - clean and widely used test framework;
- Moq - simple and widely used mock tool;
- coverlet report - free visual tool to identify tests coverage in the source code. 

## Layers

This app is divided in four layers:

- Presentation - API controllers, containing the endpoints;
- Application - communication between presentation and domain layers;
- Domain - business logics and core application;
- Infrastructure - database persistence and external services communication.

## Endpoints

### Currencies

#### GET api/currencies

Obtains the collection of available currencies to perform the conversion, which are passed in the conversion endpoint.

#### GET api/currencies/conversion

Returns conversion data from the informed currencies an value.

User id for testing purposes: 

Query Strings:

- fromValue - decimal - initial value, to be converted;
- fromCurrency: [ISO 4217](https://en.wikipedia.org/wiki/ISO_4217) Initial currency code;
- toCurrency: [ISO 4217](https://en.wikipedia.org/wiki/ISO_4217) To be converted currency code;
- userId: registered user id. User id for testing purposes: "78820307-844c-410a-935d-9ee90464d061".


#### GET /transactions​/success

Obtains the collection of successful conversion transactions and their data. 

#### GET /transactions​/failures

Obtains the collection of failed conversion transactions and their data. 

## Postman Documentation

Available at: https://documenter.getpostman.com/view/8706801/TzskEiFG
