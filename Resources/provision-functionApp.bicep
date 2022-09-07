param name string
param location string = resourceGroup().location

module st './storageAccount.bicep' = {
    name: 'StorageAccount_FunctionApp'
    params: {
        name: name
        location: location
    }
}

module wrkspc './logAnalyticsWorkspace.bicep' = {
    name: 'LogAnalyticsWorkspace_FunctionApp'
    params: {
        name: name
        location: location
    }
}

module appins './appInsights.bicep' = {
    name: 'ApplicationInsights_FunctionApp'
    params: {
        name: name
        location: location
        workspaceId: wrkspc.outputs.id
    }
}

module csplan './consumptionPlan.bicep' = {
    name: 'ConsumptionPlan_FunctionApp'
    params: {
        name: name
        location: location
    }
}

module fncapp './functionApp.bicep' = {
    name: 'FunctionApp_FunctionApp'
    params: {
        name: name
        location: location
        storageAccountConnectionString: st.outputs.connectionString
        appInsightsInstrumentationKey: appins.outputs.instrumentationKey
        appInsightsConnectionString: appins.outputs.connectionString
        consumptionPlanId: csplan.outputs.id
    }
}
