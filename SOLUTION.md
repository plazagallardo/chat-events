
# Technical considerations.

## Architecture

I used the flavour of hexagonal architecture also known as port and adapters to decouple the persistence layer. That allows great flexibility to introduce new modules and replace them from the core like Logging, Security, etc.

## Implementation 

As the exercise does not specify any filters(DateFrom, DateTo, Pagination, etc), I do retrieve all events from the persistence layer. 
The Domain Service takes care of fetching all events (AccessEvent, SendMessageEvent, HighFiveEvents) and maps them out into a list of ChatEvent DTOs which consist in a timeScope and a description based on the granularity chosen.
 
## Persistence

Implemented an inmemory repository for simplicity as suggested.
*Keep in mind in a real word solution, I would have created a User Entity in the Domain and create a relation with the ChatEvent entity instead of using the "UserName". 

## UI

Implemented a Console Application as suggested.

## API

Simple REST API created as an addition to the exercise which can be used for testing purposes too.

## Testing

Unit tests added for all the assemblers which map from domain events to dto models based on time granularity.
Unit tests added for the main method "GetAllEventsActivityAsync" using Moq verify interface calls.

## Left overs:

The idea was to create a couple of Functional/Integration tests with the examples provided hitting the API but I run out of time.

## Alternative approach

Another option I had in mind was to calculate the granularity and group the events straight from the persistence layer using a light ORM like Dapper and some SQL Queries with DATEPART and then keep the Domain layer very thin with little logic for getting the Events activity.

