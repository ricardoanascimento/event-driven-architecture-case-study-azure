resource "azurerm_cosmosdb_account" "cosmosdb_account" {
  name                = var.name
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"
  enable_free_tier    = true

  enable_automatic_failover = false

  network_acl_bypass_ids = []

  tags  = {
    "defaultExperience"       = "Core (SQL)"
    "hidden-cosmos-mmspecial" = ""
  }

  analytical_storage {
    schema_type = "WellDefined"
  }

  backup {
    interval_in_minutes = 240
    retention_in_hours  = 8
    storage_redundancy  = "Geo"
    type                = "Periodic"
  }

  capacity {
    total_throughput_limit = 1000
  }

  consistency_policy {
    consistency_level       = "Session"
    max_interval_in_seconds = 5
    max_staleness_prefix    = 100
  }

  geo_location {
    failover_priority = 0
    location          = "westeurope"
    zone_redundant    = false
  }

  timeouts {}
}

resource "azurerm_cosmosdb_sql_database" "cosmosdb_sql_database" {
  name                = "Turbine"
  resource_group_name = var.resource_group_name
  account_name        = azurerm_cosmosdb_account.cosmosdb_account.name

  autoscale_settings {
    max_throughput = 1000
  }  

  timeouts {}
}

resource "azurerm_cosmosdb_sql_container" "sql_container_level1outage" {
  name                  = "Level1OutageAggregate"
  resource_group_name   = var.resource_group_name
  account_name          = azurerm_cosmosdb_account.cosmosdb_account.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosdb_sql_database.name
  partition_key_path    = "/turbineId"
  partition_key_version = 2

  conflict_resolution_policy {
    conflict_resolution_path = "/_ts"
    mode                     = "LastWriterWins"
  }

  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }
  }

  timeouts {}
}

resource "azurerm_cosmosdb_sql_container" "sql_container_level2outage" {
  name                  = "Level2OutageAggregate"
  resource_group_name   = var.resource_group_name
  account_name          = azurerm_cosmosdb_account.cosmosdb_account.name
  database_name         = azurerm_cosmosdb_sql_database.cosmosdb_sql_database.name
  partition_key_path    = "/turbineId"
  partition_key_version = 2

  conflict_resolution_policy {
    conflict_resolution_path = "/_ts"
    mode                     = "LastWriterWins"
  }

  indexing_policy {
    indexing_mode = "consistent"

    included_path {
      path = "/*"
    }    
  }

  timeouts {}
}