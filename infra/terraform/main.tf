# Terraform configuration for Azure Web App deployment
# Equivalent to the Bicep templates in main.bicep and webapp.bicep

terraform {
  required_version = ">= 1.0"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "gh-200"
    storage_account_name = "gh200terraform"
    container_name       = "prodstatefiles"
    key                  = "prod.terraform.tfstate"
    use_oidc             = true
    use_azuread_auth     = true
  }
}

provider "azurerm" {
  features {}
}

# Generate a unique token for resource naming
resource "random_string" "resource_token" {
  length  = 13
  lower   = true
  upper   = false
  special = false
  numeric = true
}

locals {
  resource_token = random_string.resource_token.result
  tags = {
    "env-name" = var.environment_name
  }
}

# Resource Group
resource "azurerm_resource_group" "main" {
  name     = "rg-${var.environment_name}"
  location = var.location
  tags     = local.tags
}

# Log Analytics Workspace
resource "azurerm_log_analytics_workspace" "main" {
  name                = "log-${local.resource_token}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = local.tags
}

# Application Insights
resource "azurerm_application_insights" "main" {
  name                = "appi-${local.resource_token}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  workspace_id        = azurerm_log_analytics_workspace.main.id
  application_type    = "web"
  tags                = local.tags
}

# Application Insights Dashboard
resource "azurerm_portal_dashboard" "main" {
  name                = "dash-${local.resource_token}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  tags                = local.tags

  dashboard_properties = jsonencode({
    lenses = {
      "0" = {
        order = 0
        parts = {
          "0" = {
            position = {
              x       = 0
              y       = 0
              colSpan = 6
              rowSpan = 4
            }
            metadata = {
              inputs = [
                {
                  name  = "resourceTypeMode"
                  value = "workspace"
                },
                {
                  name  = "ComponentId"
                  value = azurerm_application_insights.main.id
                }
              ]
              type = "Extension/AppInsightsExtension/PartType/AspNetOverviewPinnedPart"
            }
          }
        }
      }
    }
    metadata = {
      model = {
        timeRange = {
          value = {
            relative = {
              duration = 24
              timeUnit = 1
            }
          }
          type = "MsPortalFx.Composition.Configuration.ValueTypes.TimeRange"
        }
      }
    }
  })
}

# App Service Plan
resource "azurerm_service_plan" "main" {
  name                = "appserviceplan-webapp-${var.environment_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  os_type             = "Linux"
  sku_name            = var.sku

  tags = local.tags
}

# App Service Plan Diagnostic Settings
resource "azurerm_monitor_diagnostic_setting" "app_service_plan" {
  name                       = "basicSetting"
  target_resource_id         = azurerm_service_plan.main.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id

  metric {
    category = "AllMetrics"
    enabled  = true
  }
}

# Web App
resource "azurerm_linux_web_app" "main" {
  name                = "webapp-${var.environment_name}"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  service_plan_id     = azurerm_service_plan.main.id
  https_only          = true

  identity {
    type = "SystemAssigned"
  }

  site_config {
    always_on = true

    application_stack {
      dotnet_version = "8.0"
    }
  }

  app_settings = {
    "APPLICATIONINSIGHTS_CONNECTION_STRING" = azurerm_application_insights.main.connection_string
  }

  tags = local.tags
}

# Web App Diagnostic Settings
resource "azurerm_monitor_diagnostic_setting" "web_app" {
  name                       = "basicSetting"
  target_resource_id         = azurerm_linux_web_app.main.id
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id

  enabled_log {
    category = "AppServiceHTTPLogs"
  }

  enabled_log {
    category = "AppServiceConsoleLogs"
  }

  enabled_log {
    category = "AppServiceAppLogs"
  }

  metric {
    category = "AllMetrics"
    enabled  = true
  }
}
