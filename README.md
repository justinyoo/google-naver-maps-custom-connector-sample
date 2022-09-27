# Google Maps & Naver Map API Custom Connector Sample #

This provides a sample Azure Functions app to build a [Power Platform Custom Connector](https://docs.microsoft.com/connectors/custom-connectors/?WT.mc_id=dotnet-75362-juyoo) using both Google Maps and Naver Map APIs.



## Getting Started ##

1. Fork this repository into your account.
2. Get your GitHub secrets ready:
   * Azure resource name ➡️ `AZURE_RESOURCE_NAME`
   * [Azure credentials](https://github.com/Azure/login#configure-deployment-credentials) ➡️ `AZURE_CREDENTIALS`
   * [Google Maps API key](https://developers.google.com/maps/documentation/maps-static) ➡️ `MAPS_GOOGLE_APIKEY`
   * [Naver Map API client ID (optional)](https://api.ncloud-docs.com/docs/en/ai-naver-mapsstaticmap) ➡️ `MAPS_NAVER_CLIENTID`
   * [Naver Map API client secret (optional)](https://api.ncloud-docs.com/docs/en/ai-naver-mapsstaticmap) ➡️ `MAPS_NAVER_CLIENTSECRET`

3. Get your [GitHub Personal Access Token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token) ready.
4. Click the button below to autopilot all relevant resources and apps on Azure.

   [![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjustinyoo%2Fgoogle-naver-maps-custom-connector-sample%2Fmain%2FResources%2Fazuredeploy.json)

   Make sure that:

   * You know your resource name.
   * You know your GitHub Personal Access Token.

5. Visit the following URL to check whether you can see all the apps properly provisioned and deployed, or not.

   * `https://apim-<AZURE_RESOURCE_NAME>.azure-api.net/maps/swagger/ui`

6. Invoke any request on Swagger UI page by providing the latitude and longitude of any location. Here are several example locations:
   * Seattle, WA, USA:
     * latitude: 47.607388
     * longitude: -122.332456
   * Seoul, South Korea:
     * latitude: 37.574703
     * longitude: 126.978519
   * Singapore, Singapore:
     * latitude: 1.359976
     * longitude: 103.873703
   * Sydney, NSW, Australia:
     * latitude: -33.868647
     * longitude: 151.211761

7. Visit the following URL to check whether you can see the OpenAPI document (v2) or not.

   * `https://st<AZURE_RESOURCE_NAME>.blob.core.windows.net/openapis/maps.json`

8. Build your [Power Platform Custom Connector](https://docs.microsoft.com/connectors/custom-connectors/?WT.mc_id=dotnet-75362-juyoo) with this OpenAPI document URL.
9. Use the custom connector on either [Power Apps](https://docs.microsoft.com/power-apps/powerapps-overview?WT.mc_id=dotnet-75362-juyoo) or [Power Automate](https://docs.microsoft.com/power-automate/getting-started?WT.mc_id=dotnet-75362-juyoo).


## More Readings ##

* [#ServerlessSeptember: Where am I now? - Serverless Power Platform custom connector for Google Maps and Naver Map](https://azure.github.io/Cloud-Native/blog/to-be-announced)
* [30 Days of Serverless](https://azure.github.io/Cloud-Native/serverless-september/30DaysOfServerless)
