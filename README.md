# What is it?
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=coverage)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=ncloc)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=security_rating)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)  
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor?ref=badge_shield)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/MirzaMerdovic/HttpRequestInterceptor.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/MirzaMerdovic/HttpRequestInterceptor/alerts/)

Highly configurable implementation of [DelegatingHandler](https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-message-handlers) that can be used for mocking the behavior of requests sent to specific routes.

## Usage
In order to use the interceptor all you need to do is register in using the DI of your choice, below example uses Microsoft built in DI:
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<Collection<HttpInterceptorOptions>>(Configuration.GetSection("HttpInterceptorOptions"));
    services.AddTransient<InterceptingHandler>();
    
    services.AddHttpClient("GitHubClient", client =>
    {
        client.BaseAddress = new Uri("https://api.github.com/");
        client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        client.DefaultRequestHeaders.Add("User-Agent", "HttpRequestInterceptor-Test");
    })
    .AddHttpMessageHandler<InterceptingHandler>();

    ...
}
```

## Examples

Configuration for the interceptor is provider via `appsetting.json` through the usage of [options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.2).

### HttpInterceptorOptions

This class represent a configuration entry that belongs to a specific route or path. Configuration entry consist of:
* MethdoName: represents one of the standard [HTTP methods](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpmethod?view=netstandard-2.0#properties) and it is a required value
* Host: represent the domain name for example `github.com`
* Path: represents a route or a specific path that's going to be intercepted. Allows usage of wildecards "*". The parameter is required.
* ReturnStatusCode: represent response status code. The parameter is required and it must belong to one of standard status codes that can be found in [HttpStatusCode enum](https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=netstandard-2.0).
* ReturnJsonContent: represents the serializes response content. It's an optional parameter.
* Rank: represents route's rank. It can be used to prioritize how the route will be picked up by the interceptor. If rout has the value: `0` it will have the highest rank.
 
### Examples
We have several options:
* Static routing with fully specified routes e.g `api/product/1`
* Dynamic routing using wildcards `*` so that we can aim for any value e.g `api/product/*`
* Routing based on host name in case that we want to have different behavior for the same route based on their domain
* Rank based routing in order to solve ranking of more complex routing scenarios where we have mixed all startegies

#### Static routing
This is the simplest way to configure a route to be intercepted. For example if we want to intercept route: `/api/product/2`
we can use add the below configuration to our `appsettings.json`

```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/2",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}]
```

The above configuration will intercept only GET request that belong to "/api/product/2" any other request will be forwarded to the next handler in HttpClient's pipeline.  

#### Dynamic routing
If we want to make the above example more generic, so that all GET requests that belong to a route: `/api/product` get intercepted, we can use this:
```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}]
```

The above configuration will intercept any GET request that is submitted on `/api/product/{id}`, so `api/product/2` and `api/product/two` will be intercepted and will have the same configured response status and content.  

You can use more than one wildcard `*` when configuring your route if you need to: 
```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/*/locations/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}]
```

The above configuration will intercept GET request that belong to routes similar to `api/product/2/locations/london` or `api/product/2/locations/42`

#### Host based routing
If you have run into a scenario where you have a need to mock two routes that have the same path for example: `api/users/*`, but depending on domain they will have different response content you can solve that by add host to your route configuration, which will look close to this:
```javascript
"HttpInterceptorOptions": [{
    "MethodName": "GET",
    "Host": "blue-users.com"
    "Path": "api/user/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Blue_1", "Age": 43 }"
}, {
    "MethodName": "GET",
    "Host": "red-users.com",
    "Path": "api/user/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "UserName": "Red_Baron", "IsActive": true }"
}]
```
Now if you send a request to:  `blue-users.com/api/user/44` you will get a response that contains properties: `Id`, `Name` and `Age`, and if you send a request to: `red-users.com/api/user/44` you will get a response that contains properties: `Id`, `UserName` and `IsActive`.

#### Rank based routing
Let's say that you have a route: `api/product/*`, but you also want to treat the route: `api/product/1` differently you can resolve this buy using ranks. Using ranks means applying a rank value to a route so it takes a presedence over the other matching routes. The biggest value for the rank is `0`, so if we set the rank for `api/product/1` to `0` and rank for `api/product/*` to anything bigger then `0` like in the configuration example below:
```javascript
"HttpInterceptorOptions": [{
    "MethodName": "GET",
    "Path": "api/product/1",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Blue_1", "Age": 43 }",
    "Rank": 0
}, {
    "MethodName": "GET",
    "Path": "api/user/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "UserName": "Red_Baron", "IsActive": true }",
    "Rank": 1
}]
```
Rank values of course don't have to be: `0` and `1`, the only requirement is that the route you want to get picked has the rank value with smaller number then the other matching routes, so in the case above value coud've been: `21` and `100` respectively.  

#### Configuring response content
The response content can be anything, or any string value. It doesn't have to be valid JSON, since the value will just be returned as a string, but my guess that in most cases the value will be serialized presentation of returning type, so that the code execution can continue without problems, but to make things clear all example below are valid responses:
* "ReturnJsonContent": "{ "Id": 1, "Name": "Blue_1", "Age": 43 }"
* "ReturnJsonContent": "{}"
* "ReturnJsonContent": "",
* "ReturnJsonContent": "2",
* "ReturnJsonContent": "{ "Id": "invalid json"

In case that you expect that HTTP response also contain some specific headers you can configure them like this:
```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/*/locations/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }",
    "Headers": [{
        "Name": "User-Agent",
        "Value": "scissors"
    }, {
        "Name": "my-header",
        "Value": "mine"
    }]
}]
```

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor?ref=badge_large)
