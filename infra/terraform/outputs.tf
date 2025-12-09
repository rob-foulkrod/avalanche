# Output values for Terraform configuration
# Equivalent to the outputs in main.bicep and webapp.bicep

output "webAppName" {
  value       = azurerm_linux_web_app.main.name
  description = "The name of the deployed web app"
}

output "webAppUrl" {
  value       = azurerm_linux_web_app.main.default_hostname
  description = "The default hostname of the deployed web app"
}

output "resourceGroupName" {
  value       = azurerm_resource_group.main.name
  description = "The name of the resource group"
}

output "resourceGroupId" {
  value       = azurerm_resource_group.main.id
  description = "The ID of the resource group"
}

output "location" {
  value       = azurerm_resource_group.main.location
  description = "The location of the resources"
}
