# HttpRequestInterceptor
[![Build status](https://ci.appveyor.com/api/projects/status/r7hyu8qeq5jaoj3d/branch/master?svg=true)](https://ci.appveyor.com/project/MirzaMerdovic/httprequestinterceptor/branch/master) 
[![CodeFactor](https://www.codefactor.io/repository/github/mirzamerdovic/httprequestinterceptor/badge)](https://www.codefactor.io/repository/github/mirzamerdovic/httprequestinterceptor) 
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor?ref=badge_shield)
[![Total alerts](https://img.shields.io/lgtm/alerts/g/MirzaMerdovic/HttpRequestInterceptor.svg?logo=lgtm&logoWidth=18)](https://lgtm.com/projects/g/MirzaMerdovic/HttpRequestInterceptor/alerts/)

Highly configurable implementation of DelegatingHandler that can be used for mocking the behavior of requests sent to specific routes.

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

    services.AddMvc();
}
```

## Examples

Configuration for the interceptor is provider via appsetting.json through the usage of [options pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.2).

### HttpInterceptorOptions

This class represent a configuration entry that belong to a specific route or path. Configuration entry consist of:
* MethdoName: represents one of the standard [HTTP methods](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpmethod?view=netstandard-2.0#properties) and it is a required value
* Path: represents a route or a specific path that's going to be intercepted. Allows usage of wildecards "*". The parameter is required.
* ReturnStatusCode: represent response status code. The parameter is required and it must belong to one of standard status codes that can be found in [HttpStatusCode enum](https://docs.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode?view=netstandard-2.0).
* ReturnJsonContent: represents the serializes response content. It's an optional parameter.

### Examples

This simplest example is when we want to intercept a specific request for example: /api/product/2
Intercepting this request can be achieved by providing this configuration in appsettings.json

```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/2",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}
```

The above configuration will intercept only GET request that belong to "/api/product/2"

```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/59",
    "ReturnStatusCode": 404,
    "ReturnJsonContent": "{}"
}
```
The above configuration will return not found for the path: "api/product/59"

If we want to make the above example more generic, so that all the GET requests that belong to a route: /api/product got intercepted than we ca use this:
```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}
```
The above configuration will intercept any GET request that is submitted on "/api/product/{id}", so "api/product/2" and "api/product/3" will get intercepted.

You can use more than one "*" sign if you need to: 
```javascript
"HttpInterceptorOptions": [
{
    "MethodName": "GET",
    "Path": "api/product/*/locations/*",
    "ReturnStatusCode": 200,
    "ReturnJsonContent": "{ "Id": 1, "Name": "Product_1" }"
}
```
The above configuratio will intercept GET request that belong to routes similar to "api/product/2/locations/london" or "api/product/2/locations/42"


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor?ref=badge_large)
