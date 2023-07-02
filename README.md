# TwitterStatistics

This is a sample app to consume a stream of events from twitter(Sampled stream) and build aggregates on the events stream. 
At this point following aggregates are calculated:
- Tweet Count
- Tweets Per Minute

## Setup
- .Net Core 6.0
- Replace Bearer Token in appSetting.json
- Run the application.
- Hit GET /api/v1/twitter/statistics to fetch the aggregates.

## Design Approach
Pub-Sub design pattern has been used for implementation. In the current design, a simple InMemory Event Bus is used
to replicate the behaviour of full a fledged event Bus like Kafka or Azure EventBus.

## Components

![ArchitectureDiagram](https://github.com/poojajain-q16/TwitterAPISample/assets/138339043/f9995609-2fb4-442c-85dd-0e90724a43ed)


### App

A RESTful API endpoint which returns the current TweetCount and Avg TweetsPerMinute. This service interacts with
an InMemory TwitterStatistics model which can be extended as an Model mapped to a DataStore Table/Object.

### Consumer

This component polls the Event Bus continuously to fetch and process the latest Event and stores the updated aggregates in InMemory TwitterStatistics Model.

### Publisher

Consumes Twitter Stream and publishes events to the Event Bus.

### EventBus

InMemoryEventBus: This is a simplistic event Bus which queues the messages as they are published and then consumers can pull the
messages from the queue at their will.

### Models

- TwitterStatistics: InMemory model for Statistics.
- TweetResponseMessage: API Response Body.
