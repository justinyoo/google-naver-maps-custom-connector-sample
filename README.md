# Google Maps & Naver Map API Custom Connector Sample #

This provides a sample Azure Functions app to build a custom connector using both Google Maps and Naver Map APIs.


## Getting Started ##

1. Click the button below to autopilot all instances and apps on Azure. Make sure that you don't forget the app name you give.

    [![Deploy To Azure](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/deploytoazure.svg?sanitize=true)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjustinyoo%2Fgoogle-naver-maps-custom-connector-sample%2Ffeature%2Finitial%2FResources%2Fazuredeploy.json)

2. Visit the following URL to check whether all the apps have been properly provisioned and deployed.

   * `https://apim-<AZURE_RESOURCE_NAME>.azure-api.net/maps/swagger/ui`

3. Invoke any request on Swagger UI page by providing the latitude and longitude of any location. Here are two example locations:
   * Seattle, WA, USA:
     * latitude: 47.60599935141631
     * longitude: -122.34001068269315
   * Seoul, South Korea:
     * latitude: 37.5747027
     * longitude: 126.9785193

    > Make sure that the Nave Map API doesn't work outside Korea.
