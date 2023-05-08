# Event Driven Architecture Case Study

## 1. Desing considerations around the microservice

I am using Azure Function Apps as the compute solution for deploying microservices,
it is optimized to Event Driven Architecture, the cost depends on the time processing and memory usage
what for this specific use case is very low.

In terms of application architecture I am going with layred architecture where I have Models, Services, Extension and Functions.
Improvements can be implemente is using Azure Function dependency injection and write test for it, to do that it is also necessary
apply a proxy pattern on services.

I am storing the data in Azure Cosmos DB, the coast is also very low considering the troughput the application requires and amount of storage it will consume. As an improvement I would recomend cofnigure the time to live for the data and possible move old data to a cheaper kind of storage.
Eveything resides in the same database with two different containers, one to level 1 outage and other to level 2 outage aggregations.

## 2. Desing document around event driven solution option with pros / cons along these parameters

### a. Ease of Development Effort

All depends how familiar developers are with technologyes they will use, I would classify as low to mid effor.

### b. Cost

#### Cosmos DB

Data Storage: Assuming that you are storing 256 bytes per second from 1000 microservices, the total amount of data stored per day would be:

```
256 bytes/s * 60 s/min * 60 min/hour * 24 hour/day * 1000
microservices = 22.1184 GB/day

22.1184 GB/day * 30 days = 663.552 GB/Month
```

Request Units (RUs): The performance of Cosmos DB is measured in Request Units (RUs), which are a measure of the throughput and resource consumption of a database operation. The number of RUs required for each operation depends on several factors, including the size of the data, the consistency level, and the indexing configuration.

Assuming that each write operation consumes 5 RUs and each read operation consumes 10 RUs, the total number of RUs consumed per day would be:

```
2000 write operations/s * 5 RUs/op + 2000 read operations/s * 10 RUs/op =  20000 RUs/s
```

**Total coast**: 1,168.00 EUR/Month (Cheaper if opt for reserved instances)

#### Azure Functions

The amoutn of requests to beused by each function is expressed by:

```
1000 turbines * 60 s/min * 60 min/hour * 24 hour/day * 30 days = 2,592,000,000 request
```

For 1 bilion requests consuming 128 memory size and 300 execution time in miliseconds, azure charges 593.60 (execution price) + 199.80 (price per billion requests) = 793.40

For 600 milion requests consuming 120 memory size and 300 execition time in miliseconds, azure charges 353.60 (execution price) + 119.80 (price per 600 milition requests) = 473.40

```
793.40 * 2 + 473.40 = 2060.20
```

**Total coast**: 2060.20 EUR/Month

#### Azure Service Bus

Primium tier can process exactly 1000 messages per second, using only 1 message unit.

**\*Total coast**: 677.08 EUR/Month

#### **Total Solution coast**

4,071.27 EUR/Month

### c. Ease of extension and integration with business components

Cosmos DB offers a variety of integration mechanisms out of the box using Azure Resources, ont top of that Azure Function can be integrated with pretty uch any interface that is exposed throughtout the internet.

### d. Scalability & Performenace

- All selected components can easily be scaled or configured for higher deamands.
- Azure function auto scale to accommodate the hight traffic.
- Cosmos DB fo this specific use case was consifured using provisined throughput, that is when you know exactly the amount of throughput the application needs and will pay for it wheter used or not.
- Azure Services bus Premium tier process up to 1000 requests per second using for each Messaging unit, in this use case only one is used and upt to 4 can be configured.

### e. Reliabilty & Guaranteed Delivery

- Azure Service Bus SLA: 99.9% uptime translates to 8.76 hours of downtime per year.
- Azure Functions SLA: 99.95% uptime translates to 4.38 hours of downtime per year.
- Azure Cosmos DB SLA: 99.999% uptime translates to 5.26 minutes of downtime per year.

```
1 - ((1 - 0.9995) x (1 - 0.999) x (1 - 0.99999))
= 1 - (0.00005 x 0.001 x 0.00001)
= 0.9999499995
```

So it means (1 - 0.9999499995) x 365.25 = 2.06 hours of downtime per year.

## 3. Cloud Service (Event Hub, Event Grid, etc.. any cloud specific services) Design with

### Azure Service Bus

- Offers the exact amount of thoughput the solution required, and it is esay to scale.
- For this use case desing I am not considering an approac where historical data is kept in Topic leval, otherwise another resouce such as event hub might be a better option.

### Azure Cosmos DB

- Has many offers, guarated thoughput, and highliy available

### Azure Functions

- That is Azure most recomended solution for Event Driven Architecture

![E2E Cloud Architecture](/case-study-eda.jpg)

## Cloud Service Provisioning

For Azure it is possible to use ARM Templates, Bicep or Terraform.

## Sample implementation with the event driven design

This project

## Integration between services via event driven solution

This project
