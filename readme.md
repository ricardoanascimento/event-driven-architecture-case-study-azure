# healthcheck

local.settings.json

```JSON
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "VOLTAGE_THRESHOLD": "90",
    "TOPIC_NAME": "telemetry",
    "LEVEL1_SUBSCRIPTION_NAME": "level1outage",
    "LEVEL1_SERVICE_BUS_CONNECTION_STRING": "",
    "LEVEL1_TIME_THRESHOLD_IN_SECONDS": "1",
    "LEVEL2_SUBSCRIPTION_NAME": "level2outage",
    "LEVEL2_SERVICE_BUS_CONNECTION_STRING": "",
    "LEVEL2_TIME_THRESHOLD_IN_SECONDS": "3",
    "LEVEL1_AGGREGATE_CONTAINER_NAME": "Level1OutageAggregate",
    "LEVEL1_AGGREGATE_SUBSCRIPTION_NAME": "level1outageAggregation",
    "LEVEL1_AGGREGATE_SERVICE_BUS_CONNECTION_STRING": "",
    "LEVEL2_AGGREGATE_CONTAINER_NAME": "Level2OutageAggregate",
    "LEVEL2_AGGREGATE_SUBSCRIPTION_NAME": "level2outageAggregation",
    "LEVEL2_AGGREGATE_SERVICE_BUS_CONNECTION_STRING": "",
    "DATABASE_NAME": "Turbine",
    "COSMOS_DB_CONNECTION_STRING": ""
  }
}
```
