module "servicebus" {
  source                  = "./modules/service-bus"
  name                    = "turbine13124"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}
