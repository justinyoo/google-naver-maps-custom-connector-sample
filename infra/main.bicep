targetScope = 'subscription'

param name string
param location string = 'Korea Central'

param apiManagementPublisherName string = 'Maps Proxy API'
param apiManagementPublisherEmail string = 'apim@contoso.com'
param gitHubBranchName string = 'main'

var suffix = 'api'
var storageContainerName = 'openapis'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
    name: 'rg-${name}'
    location: location
}

module apim './provision-apiManagement.bicep' = {
    name: 'ApiManagement'
    scope: rg
    params: {
        name: name
        location: location
        storageContainerName: storageContainerName
        apiMgmtPublisherName: apiManagementPublisherName
        apiMgmtPublisherEmail: apiManagementPublisherEmail
        apiMgmtPolicyFormat: 'xml-link'
        apiMgmtPolicyValue: 'https://raw.githubusercontent.com/justinyoo/google-naver-maps-custom-connector-sample/${gitHubBranchName}/Resources/apim-global-policy.xml'
    }
}

module fncapp './provision-functionApp.bicep' = {
    name: 'FunctionApp'
    scope: rg
    dependsOn: [
        apim
    ]
    params: {
        name: name
        suffix: suffix
        location: location
        storageContainerName: storageContainerName
    }
}
