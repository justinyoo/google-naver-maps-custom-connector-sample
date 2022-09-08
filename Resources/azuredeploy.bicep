targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string
param apiManagementPublisherEmail string
var storageContainerName = 'openapis'
param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module resources './main.bicep' = {
    name: 'Resources'
    scope: rg
    params: {
        name: name
        location: location
        apiMgmtPublisherName: apiManagementPublisherName
        apiMgmtPublisherEmail: apiManagementPublisherEmail
        storageContainerName: storageContainerName
        gitHubBranchName: gitHubBranchName
        gitHubAccessToken: gitHubAccessToken
    }
}
