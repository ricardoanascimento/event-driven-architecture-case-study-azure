output "database_name" {
    value = azurerm_cosmosdb_sql_database.cosmosdb_sql_database.name
#   value = "AccountEndpoint=https://${azurerm_cosmosdb_account.cosmosdb_account.name}.documents.azure.com:443/;AccountKey=${azurerm_cosmosdb_account.cosmosdb_account.primary_key};"
}

output "primary_connection_string" {
    value = azurerm_cosmosdb_account.cosmosdb_account.connection_strings[0]
#   value = "AccountEndpoint=https://${azurerm_cosmosdb_account.cosmosdb_account.name}.documents.azure.com:443/;AccountKey=${azurerm_cosmosdb_account.cosmosdb_account.primary_key};"
}