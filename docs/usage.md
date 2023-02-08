# USING
After installing this library from nuget in your start-up class add specified parts of code:
```csharp
public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefinitionDecorates(typeof(Startup));
        }
```
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefinitionDecorates(env);
        }
```

So your start-up class can be reduced to two methods with one line of code.
In dependence on how many parameters you have in your class, you can choose from several available methods.

**ConfigureServices**
* `AddDefinitionDecorates(this IServiceCollection serviceCollection, params Type[] entryPoints)`;
* `AddDefinitionDecorates(this IServiceCollection serviceCollection,             IConfiguration configuration, params Type[] entryPoints)`;
* `AddDefinitionDecorates(this IServiceCollection serviceCollection, IApplicationBuilder builder, params Type[] entryPoints)`;
* `AddDefinitionDecorates(this IServiceCollection serviceCollection, IApplicationBuilder builder, IConfiguration configuration, params Type[] entryPoints)`.

**Configure**
* `UseDefinitionDecorates(this IApplicationBuilder applicationBuilder)`;
* `UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env)`;
* `UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env, IHostApplicationLifetime applicationLifetime)`;
* `UseDefinitionDecorates(this IApplicationBuilder applicationBuilder, IHostEnvironment env, IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)`.

<hr/>

In addition, from the box is available to set priority on initialize components (`InitializeOrder`).

Another feature of this lib is the posibility to enable or disable any feature by using the property `IsDecorationEnable`. In case when you want to use some feature on the dev environment, but not in production, set the `IsDecorationEnable = false` by some variable in your configuration file.

An example of set parameters:
```csharp
public class TestClass : InitDefinitionDecorate
    {
        public TestClass()
        {
            //  Set initialize order of the current application definition
            base.InitializeOrder = 0;
            
            //  Set if current application definition si enabled for intialization 
            base.IsDecorationEnable = true;
        }
    }
```

**For more clearly using understanding please check the project(`TestWebApp`) from the current solution with an example of use.**

