#!/bin/bash

set -e

urls=$(curl -H "Accept: application/vnd.github.v3+json" \
    https://api.github.com/repos/justinyoo/google-naver-maps-custom-connector-sample/releases/latest | \
    jq '[.assets[] | .browser_download_url]')

resource_group="rg-$AZ_RESOURCE_NAME"

# Deploy Function app
fncapp_name="fncapp-$AZ_RESOURCE_NAME-$AZ_RESOURCE_SUFFIX"
fncapp_zip=$(echo $urls | jq --argjson value 0 '.[$value]' -r)
fncapp=$(az functionapp deploy -g $resource_group -n $fncapp_name --src-url $fncapp_zip --type zip)
fncapp_url="https://$fncapp_name.azurewebsites.net/api/openapi/v3.json"

# Provision APIs to APIM
apim_name="apim-$AZ_RESOURCE_NAME"
bicep_url="https://raw.githubusercontent.com/justinyoo/google-naver-maps-custom-connector-sample/$GH_BRANCH_NAME/Resources/provision-apiManagementApi.json"
api_path="maps"
api_name="MAPS"
api_nv_name="X_FUNCTIONS_KEY"
api_nv_value=$(az functionapp keys list -g $resource_group -n $fncapp_name --query "functionKeys.default" -o tsv)
api_policy_url="https://raw.githubusercontent.com/justinyoo/google-naver-maps-custom-connector-sample/$GH_BRANCH_NAME/Resources/apim-api-policy.xml"

az deployment group create \
    -n ApiManagementApi \
    -g $resource_group \
    -u $bicep_url \
    -p name=$AZ_RESOURCE_NAME \
    -p apiMgmtNameValueName=$api_nv_name \
    -p apiMgmtNameValueDisplayName=$api_nv_name \
    -p apiMgmtNameValueValue=$api_nv_value \
    -p apiMgmtApiName=$api_name \
    -p apiMgmtApiDisplayName=$api_name \
    -p apiMgmtApiDescription=$api_name \
    -p apiMgmtApiPath=$api_path \
    -p apiMgmtApiFormat="openapi+json-link" \
    -p apiMgmtApiValue=$fncapp_url \
    -p apiMgmtApiPolicyFormat="xml-link" \
    -p apiMgmtApiPolicyValue=$api_policy_url

# Update hostnames on function apps
apim_url="https://$apim_name.azure-api.net/$api_path"
appsettings_hostnames=$(az functionapp config appsettings list -g $resource_group -n $fncapp_name | jq '.[] | select(.name == "OpenApi__HostNames") | .value' -r)
if [ "$appsettings_hostnames" == "" ]
then
    openapi_hostnames=$apim_url
else
    openapi_hostnames=$apim_url,$appsettings_hostnames
fi

appsettings_updated=$(az functionapp config appsettings set -g $resource_group -n $fncapp_name --settings OpenApi__HostNames=$openapi_hostnames)

# Export swagger.json from APIM API
subscription_id=$(az account show --query id -o tsv)
st_name="st$AZ_RESOURCE_NAME"
api_version="2021-12-01-preview"
api_name="maps"
apim_export_url="https://management.azure.com/subscriptions/$subscription_id/resourceGroups/$resource_group/providers/Microsoft.ApiManagement/service/$apim_name/apis/$api_name\?api-version=$api_version&export=true&format=swagger-json"
swaggerdoc=$(az rest --method get --url $apim_export_url)
swaggerjson="$api_name.json"
echo $swaggerdoc | jq 'del(.paths."/openapi/v2.json") |
                       del(.paths."/openapi/v3.json") |
                       del(.paths."/swagger.json") |
                       del(.paths."/swagger/ui")' > $swaggerjson

# Upload swagger.json to Azure Blob
st_container_name="$ST_CONTAINER_NAME"
st_connstring=$(az storage account show-connection-string -g $resource_group -n $st_name --query "connectionString" -o tsv)
blob_updated=$(az storage blob upload --connection-string $st_connstring -f $swaggerjson -c $st_container_name -n "$swaggerjson" --overwrite)
