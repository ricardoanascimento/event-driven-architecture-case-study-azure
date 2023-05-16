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

# module "function_app_producer" {
#   source             = "./modules/function-app"
#   function_app_name  = "turbine19027572"
#   app_service_plan_id = module.app_service_plan.app_service_producer.name
#   location           = "East US"
# }

# module "app_service_health_check" {
#   source              = "./modules/app-service-plan"
#   app_service_plan_id = "ASP-edastudycase-b0ec"
#   location            = "East US"
#   tier                = "Standard"
#   size                = "S1"
# }

# module "function_app_health_check" {
#   source             = "./modules/function-app"
#   function_app_name  = "healthckeck125154"
#   app_service_plan_id = module.app_service_plan.app_service_health_check.name
#   location           = "East US"
# }
