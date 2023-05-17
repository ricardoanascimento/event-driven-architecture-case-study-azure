output "level1_aggregate_service_bus_connection_string" {
    value = azurerm_servicebus_topic_authorization_rule.tar_level1outageAggregation.primary_connection_string
}

output "level1_service_bus_connection_string" {
    value = azurerm_servicebus_topic_authorization_rule.tar_level1outage.primary_connection_string
}

output "level2_aggregate_service_bus_connection_string" {
    value = azurerm_servicebus_topic_authorization_rule.tar_level2outageAggregation.primary_connection_string
}

output "level2_service_bus_connection_string" {
    value = azurerm_servicebus_topic_authorization_rule.tar_level2outage.primary_connection_string
}

output "producer_service_bus_connection_string" {
    value = azurerm_servicebus_topic_authorization_rule.tar_producer.primary_connection_string
}