resource "azurerm_servicebus_namespace" "servicebus_namespace" {
  name                = var.name
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  sku                 = "Standard"
  tags                                = {}
  zone_redundant                      = false
}

resource "azurerm_servicebus_topic" "telemetry" {
  name         = "telemetry"
  namespace_id = azurerm_servicebus_namespace.servicebus_namespace.id
  enable_batched_operations = true
  enable_express = false
  enable_partitioning = false
  requires_duplicate_detection = false
  support_ordering = true
  timeouts {}
}

resource "azurerm_servicebus_topic_authorization_rule" "tar_producer" {
  name         = "producer"
  topic_id           = azurerm_servicebus_topic.telemetry.id

  listen = false
  send   = true
  manage = false
}

resource "azurerm_servicebus_subscription" "level1outageAggregation" {
  name               = "level1outageAggregation"
  topic_id           = azurerm_servicebus_topic.telemetry.id
  enable_batched_operations = true
  dead_lettering_on_filter_evaluation_error = false  
  max_delivery_count = 10
  timeouts {}
}

resource "azurerm_servicebus_topic_authorization_rule" "tar_level1outageAggregation" {
  name         = "level1outageAggregation"
  topic_id           = azurerm_servicebus_topic.telemetry.id

  listen = true
  send   = false
  manage = false
}

resource "azurerm_servicebus_subscription" "level1outage" {
  name               = "level1outage"
  topic_id           = azurerm_servicebus_topic.telemetry.id
  enable_batched_operations = true
  dead_lettering_on_filter_evaluation_error = false
  max_delivery_count = 10
  timeouts {}
}

resource "azurerm_servicebus_topic_authorization_rule" "tar_level1outage" {
  name         = "level1outage"
  topic_id           = azurerm_servicebus_topic.telemetry.id

  listen = true
  send   = false
  manage = false
}

resource "azurerm_servicebus_subscription" "level2outageAggregation" {
  name               = "level2outageAggregation"
  topic_id           = azurerm_servicebus_topic.telemetry.id
  enable_batched_operations = true
  dead_lettering_on_filter_evaluation_error = false
  max_delivery_count = 10
  timeouts {}
}

resource "azurerm_servicebus_topic_authorization_rule" "tar_level2outageAggregation" {
  name         = "level2outageAggregation"
  topic_id           = azurerm_servicebus_topic.telemetry.id

  listen = true
  send   = false
  manage = false
}

resource "azurerm_servicebus_subscription" "level2outage" {
  name               = "level2outage"
  topic_id           = azurerm_servicebus_topic.telemetry.id
  enable_batched_operations = true
  dead_lettering_on_filter_evaluation_error = false
  max_delivery_count = 10
  timeouts {}
}

resource "azurerm_servicebus_topic_authorization_rule" "tar_level2outage" {
  name         = "level2outage"
  topic_id           = azurerm_servicebus_topic.telemetry.id

  listen = true
  send   = false
  manage = false
}
