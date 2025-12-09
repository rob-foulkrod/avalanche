#!/bin/bash

# Deploy the Bicep template and capture the JSON result
jsonResult=$(az deployment sub create \
  --template-file main.bicep \
  --location "EastUS2" \
  --parameters dev.parameters.json)

# Display the full result for debugging
echo "$jsonResult"

# Extract the outputs section
ovar=$(echo "$jsonResult" | jq '.properties.outputs')
echo "$ovar"

# Extract individual output values
webAppName=$(echo "$jsonResult" | jq -r '.properties.outputs.webAppName.value')
webAppUrl=$(echo "$jsonResult" | jq -r '.properties.outputs.webAppUrl.value')
location=$(echo "$jsonResult" | jq -r '.properties.outputs.location.value')
resourceGroupName=$(echo "$jsonResult" | jq -r '.properties.outputs.resourceGroupName.value')
resourceGroupId=$(echo "$jsonResult" | jq -r '.properties.outputs.resourceGroupId.value')

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