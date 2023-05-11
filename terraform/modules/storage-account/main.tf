resource "azurerm_storage_account" "storage_account" {
  name                              = var.name
  resource_group_name               = var.resource_group_name
  location                          = var.resource_group_location
  default_to_oauth_authentication   = true
  account_kind                      = "Storage"
  account_tier                      = "Standard"
  account_replication_type          = "LRS"
  cross_tenant_replication_enabled  = false
  timeouts {}
}