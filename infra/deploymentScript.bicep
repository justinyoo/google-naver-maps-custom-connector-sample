param name string
param suffix string = 'api'
param location string = resourceGroup().location

param storageContainerName string
param gitHubBranchName string = 'main'
@secure()
param gitHubAccessToken string

var userAssignedIdentity = {
    name: 'spn-${name}'
    location: location
}

resource uai 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
    name: userAssignedIdentity.name
    location: userAssignedIdentity.location
}

var roleAssignment = {
    name: guid(resourceGroup().id, 'contributor')
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')
    principalType: 'ServicePrincipal'
}

resource role 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
    name: roleAssignment.name
    scope: resourceGroup()
    properties: {
        roleDefinitionId: roleAssignment.roleDefinitionId
        principalId: uai.properties.principalId
        principalType: roleAssignment.principalType
    }
}

var deploymentScript = {
    name: 'depscrpt-${name}'
    location: location
    resourceName: name
    suffix: suffix
    storageContainerName: storageContainerName
    gitHubBranchName: gitHubBranchName
    gitHubAccessToken: gitHubAccessToken
    containerGroupName: 'contgrp-${name}'
    azureCliVersion: '2.37.0'
    scriptUri: 'https://raw.githubusercontent.com/justinyoo/google-naver-maps-custom-connector-sample/${gitHubBranchName}/Resources/setup-resources.sh'
}

resource ds 'Microsoft.Resources/deploymentScripts@2020-10-01' = {
    name: deploymentScript.name
    location: deploymentScript.location
    dependsOn: [
        role
    ]
    kind: 'AzureCLI'
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${uai.id}': {}
        }
    }
    properties: {
        azCliVersion: deploymentScript.azureCliVersion
        containerSettings: {
            containerGroupName: deploymentScript.containerGroupName
        }
        environmentVariables: [
            {
                name: 'AZ_RESOURCE_NAME'
                value: deploymentScript.resourceName
            }
            {
                name: 'AZ_RESOURCE_SUFFIX'
                value: deploymentScript.suffix
            }
            {
                name: 'ST_CONTAINER_NAME'
                value: deploymentScript.storageContainerName
            }
            {
                name: 'GH_BRANCH_NAME'
                value: deploymentScript.gitHubBranchName
            }
            {
                name: 'GH_ACCESS_TOKEN'
                value: deploymentScript.gitHubAccessToken
            }
        ]
        primaryScriptUri: deploymentScript.scriptUri
        cleanupPreference: 'OnExpiration'
        retentionInterval: 'P1D'
    }
}
