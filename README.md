# Google Maps & Naver Map API Custom Connector Sample #

This provides a sample Azure Functions app to build a custom connector using both Google Maps and Naver Map APIs.


## Getting Started ##

1. Get your [GitHub Personal Access Token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/creating-a-personal-access-token) ready.

2. Click the button below to autopilot all instances and apps on Azure. Make sure that:

   * You know your resource name.
   * You know your GitHub Personal Access token.

    [![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjustinyoo%2Fgoogle-naver-maps-custom-connector-sample%2Fmain%2FResources%2Fazuredeploy.json)

3. Visit the following URL to check whether all the apps have been properly provisioned and deployed.

   * `https://apim-<AZURE_RESOURCE_NAME>.azure-api.net/maps/swagger/ui`

4. Invoke any request on Swagger UI page by providing the latitude and longitude of any location. Here are example locations:
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

    > Make sure that the Nave Map API doesn't work outside Korea.

5. Visit the following URL to check whether the OpenAPI document (v2) is rendered or not.

   * `https://st<AZURE_RESOURCE_NAME>.blob.core.windows.net/openapis/maps.json`
