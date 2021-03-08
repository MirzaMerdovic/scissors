# HttpRequestInterceptor
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=coverage)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=ncloc)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=MirzaMerdovic_HttpRequestInterceptor&metric=security_rating)](https://sonarcloud.io/dashboard?id=MirzaMerdovic_HttpRequestInterceptor)  
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
The above configuration will intercept GET request that belong to routes similar to "api/product/2/locations/london" or "api/product/2/locations/42"

### Notes
There is no ranking support for routes, so you need to be careful with potential unexpected behaviors.
For example imaginge you have configured two routes:
* api/route/*
* api/route/33

If you hit in your browser: `api/route/33` mock of that route would never be invoked because the first route that contains a wild-card would be triggered.  
In order to mitigate this change the order of the routes in your appsettings.json, so they go like this:
* api/route/33
* api/route/*

This is of course not a solution for all possible routing scenarios, so there will be cases impossible to mock with interceptor.

### TO DOs
Implement route ranking support.

## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FMirzaMerdovic%2FHttpRequestInterceptor?ref=badge_large)
