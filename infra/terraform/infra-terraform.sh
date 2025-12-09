#!/bin/bash

# Deploy the Terraform configuration and capture outputs
# Equivalent to infra.sh but for Terraform

set -e

# Initialize Terraform
echo "Initializing Terraform..."
terraform init

# Plan the deployment
echo "Planning Terraform deployment..."
terraform plan -out=tfplan

# Apply the deployment
echo "Applying Terraform deployment..."
terraform apply -auto-approve tfplan

# Extract outputs
echo "Extracting Terraform outputs..."
webAppName=$(terraform output -raw webAppName)
webAppUrl=$(terraform output -raw webAppUrl)
location=$(terraform output -raw location)
resourceGroupName=$(terraform output -raw resourceGroupName)
resourceGroupId=$(terraform output -raw resourceGroupId)

# Alias for convenience (some pipelines expect 'resourceGroup')
resourceGroup=$resourceGroupName

# Write outputs for GitHub Actions
if [ -n "$GITHUB_OUTPUT" ]; then
  echo "Detected GITHUB_OUTPUT; emitting outputs for GitHub Actions context."
  echo "webAppName=$webAppName" >> "$GITHUB_OUTPUT"
  echo "webAppUrl=$webAppUrl" >> "$GITHUB_OUTPUT"
  echo "location=$location" >> "$GITHUB_OUTPUT"
  echo "resourceGroupName=$resourceGroupName" >> "$GITHUB_OUTPUT"
  echo "resourceGroupId=$resourceGroupId" >> "$GITHUB_OUTPUT"
  echo "resourceGroup=$resourceGroup" >> "$GITHUB_OUTPUT"
else
  echo "GITHUB_OUTPUT not set; no GitHub output file to write to."
fi

# Also display the values for logging purposes
echo "webAppName: $webAppName"
echo "webAppUrl: $webAppUrl"
echo "location: $location"
echo "resourceGroupName: $resourceGroupName"
echo "resourceGroupId: $resourceGroupId"
echo "resourceGroup: $resourceGroup"
