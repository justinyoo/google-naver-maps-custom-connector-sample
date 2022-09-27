param name string
param suffix string = 'api'
param location string = resourceGroup().location

param apiMgmtPublisherName string
param apiMgmtPublisherEmail string
param storageContainerName string
param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

module apim './provision-apiManagement.bicep' = {
    name: 'ApiManagement'
    params: {
        name: name
        location: location
        storageContainerName: storageContainerName
        apiMgmtPublisherEmail: apiMgmtPublisherEmail
        apiMgmtPublisherName: apiMgmtPublisherName
        apiMgmtPolicyFormat: 'xml-link'
        apiMgmtPolicyValue: 'https://raw.githubusercontent.com/justinyoo/google-naver-maps-custom-connector-sample/${gitHubBranchName}/Resources/apim-global-policy.xml'
    }
}

module fncapp './provision-functionApp.bicep' = {
    name: 'FunctionApp'
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

module depscrpt './deploymentScript.bicep' = {
    name: 'DeploymentScript'
    dependsOn: [
        apim
        fncapp
    ]
    params: {
        name: name
        suffix: suffix
        location: location
        gitHubBranchName: gitHubBranchName
        storageContainerName: storageContainerName
        gitHubAccessToken: gitHubAccessToken
    }
}
