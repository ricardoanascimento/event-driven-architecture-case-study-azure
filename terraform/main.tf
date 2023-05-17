terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
  }
}
provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "edastudycase" {
  name     = "eda-study-case"
  location = "westus"
}

module "turbine_producer_service_plan" {
  source                  = "./modules/service-plan"
  name                    = "ASP-edastudycase-9187"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
  os_type                 = "Windows"
  sku_name                = "Y1"
}

module "turbine_consumer_service_plan" {
  source                  = "./modules/service-plan"
  name                    = "ASP-edastudycase-b0ec"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
  os_type                 = "Windows"
  sku_name                = "Y1"
}

module "study_case_storage_account" {
  source                  = "./modules/storage-account"
  name                    = "edastudycaseb64e"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}

module "application_insights" {
  source                  = "./modules/app-insights"
  name                    = "apiturbinefunctions"
  workspace_name          = "DefaultWorkspace-73799131-dcdc-4c94-bf7d-87596a4d51c0-WEU"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}

module "cosmos_db" {
  source                  = "./modules/cosmos-db"
  name                    = "turbine1235512"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}

module "servicebus" {
  source                  = "./modules/service-bus"
  name                    = "turbine13124"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}

module "function_app_producer" {
  source             = "./modules/windows-function-app"
  name  = "turbine19027572"
  resource_group_name = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
  app_service_plan_id = module.turbine_producer_service_plan.app_service_plan_id
  storage_account_name = module.study_case_storage_account.name
  storage_account_access_key = module.study_case_storage_account.primary_access_key
  application_insights_connection_string = module.application_insights.connection_string
  application_insights_key = module.application_insights.instrumentation_key
  app_settings = <<EOF
  {
    "AzureWebJobs.TurbineProducer.Disabled": "1",
    "AzureWebJobsStorage": ${jsonencode(module.study_case_storage_account.primary_access_key)},
    "WEBSITE_RUN_FROM_PACKAGE": "1",
    "FUNCTIONS_EXTENSION_VERSION": "~4",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING": ${jsonencode(module.study_case_storage_account.primary_access_key)},
    "WEBSITE_CONTENTSHARE": "turbine19027572a010",
    "WEBSITE_RUN_FROM_PACKAGE": "1",
    "TOPIC_NAME": "telemetry",
    "SERVICE_BUS_CONNECTION_STRING": ${jsonencode(module.servicebus.producer_service_bus_connection_string)}    
  }
  EOF
}

module "function_app_healthcheck" {
  source             = "./modules/windows-function-app"
  name  = "healthckeck125154"
  resource_group_name = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
  app_service_plan_id = module.turbine_consumer_service_plan.app_service_plan_id
  storage_account_name = module.study_case_storage_account.name
  storage_account_access_key = module.study_case_storage_account.primary_access_key
  application_insights_connection_string = module.application_insights.connection_string
  application_insights_key = module.application_insights.instrumentation_key
  app_settings = <<EOF
  {
    "AzureWebJobsStorage": ${jsonencode(module.study_case_storage_account.primary_access_key)},
    "WEBSITE_RUN_FROM_PACKAGE": "1",    
    "FUNCTIONS_EXTENSION_VERSION": "~4",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING": ${jsonencode(module.study_case_storage_account.primary_access_key)},
    "WEBSITE_CONTENTSHARE": "healthckeck125154bdf0",
    "WEBSITE_RUN_FROM_PACKAGE": "1",
    "COSMOS_DB_CONNECTION_STRING": ${jsonencode(module.cosmos_db.primary_connection_string)},
    "DATABASE_NAME": "Turbine",
    "LEVEL1_AGGREGATE_CONTAINER_NAME": "Level1OutageAggregate",
    "LEVEL1_AGGREGATE_SERVICE_BUS_CONNECTION_STRING": ${jsonencode(module.servicebus.level1_aggregate_service_bus_connection_string)},
    "LEVEL1_AGGREGATE_SUBSCRIPTION_NAME": "level1outageAggregation",
    "LEVEL1_SERVICE_BUS_CONNECTION_STRING":  ${jsonencode(module.servicebus.level1_service_bus_connection_string)},
    "LEVEL1_SUBSCRIPTION_NAME": "level1outage",
    "LEVEL1_TIME_THRESHOLD_IN_SECONDS": "30",
    "LEVEL2_AGGREGATE_CONTAINER_NAME": "Level2OutageAggregate",
    "LEVEL2_AGGREGATE_SERVICE_BUS_CONNECTION_STRING": ${jsonencode(module.servicebus.level2_aggregate_service_bus_connection_string)},
    "LEVEL2_AGGREGATE_SUBSCRIPTION_NAME": "level2outageAggregation",
    "LEVEL2_SERVICE_BUS_CONNECTION_STRING": ${jsonencode(module.servicebus.level2_service_bus_connection_string)},
    "LEVEL2_SUBSCRIPTION_NAME": "level2outage",
    "LEVEL2_TIME_THRESHOLD_IN_SECONDS": "60",    
    "TOPIC_NAME": "telemetry",
    "VOLTAGE_THRESHOLD": "90"    
  }
  EOF
}
