# Input variables for Terraform configuration
# Equivalent to the parameters in main.bicep and dev.parameters.json

variable "environment_name" {
  type        = string
  description = "Name of the environment that can be used as part of naming resource convention"

  validation {
    condition     = length(var.environment_name) >= 1 && length(var.environment_name) <= 64
    error_message = "Environment name must be between 1 and 64 characters."
  }
}

variable "location" {
  type        = string
  description = "Primary location for all resources"

  validation {
    condition     = length(var.location) >= 1
    error_message = "Location must be specified."
  }
}

variable "sku" {
  type        = string
  description = "Tier of the App Service plan"
  default     = "S1"
}
