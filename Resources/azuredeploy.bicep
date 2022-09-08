targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string = 'Maps Proxy API'
param apiManagementPublisherEmail string = 'apim@contoso.com'
param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

var suffix = 'api'
var storageContainerName = 'openapis'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module resources './main.bicep' = {
    name: 'Resources'
    scope: rg
    params: {
        name: name
        suffix: suffix
        location: location
        apiMgmtPublisherName: apiManagementPublisherName
        apiMgmtPublisherEmail: apiManagementPublisherEmail
        storageContainerName: storageContainerName
        gitHubBranchName: gitHubBranchName
        gitHubAccessToken: gitHubAccessToken
    }
}
