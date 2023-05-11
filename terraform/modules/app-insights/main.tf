resource "azurerm_log_analytics_workspace" "log_analytics_workspace" {
  name                  = var.workspace_name
  location              = var.resource_group_location
  resource_group_name   = var.resource_group_name
  sku                   = "PerGB2018"
  retention_in_days     = 30
  cmk_for_query_forced  = false
  tags                  = {}
}

resource "azurerm_application_insights" "application_insights" {
  name                = var.name
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  workspace_id        = azurerm_log_analytics_workspace.log_analytics_workspace.id
  application_type    = "web"
  sampling_percentage = 0
  timeouts {}
}