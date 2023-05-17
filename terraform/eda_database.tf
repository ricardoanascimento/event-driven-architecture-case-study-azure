module "cosmos_db" {
  source                  = "./modules/cosmos-db"
  name                    = "turbine1235512"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}
