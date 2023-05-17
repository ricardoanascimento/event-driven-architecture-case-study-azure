module "study_case_storage_account" {
  source                  = "./modules/storage-account"
  name                    = "edastudycaseb64e"
  resource_group_name     = azurerm_resource_group.edastudycase.name
  resource_group_location = "West Europe"
}