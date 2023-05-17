resource "azurerm_windows_function_app" "windows_function_app" {
  name                = var.name
  resource_group_name = var.resource_group_name
  location            = var.resource_group_location

  storage_account_name       = var.storage_account_name
  storage_account_access_key = var.storage_account_access_key
  service_plan_id            = var.app_service_plan_id
  
  builtin_logging_enabled = false
  client_certificate_mode = "Required"

  site_config {
    ftps_state = "FtpsOnly"
    application_insights_connection_string = var.application_insights_connection_string
    application_insights_key = var.application_insights_key

    cors {
      allowed_origins = [ "https://portal.azure.com" ]
      support_credentials = false
    }
    
  }

  app_settings = jsondecode(var.app_settings)

  timeouts {}

  lifecycle {
    ignore_changes = [ tags ]
  }
}