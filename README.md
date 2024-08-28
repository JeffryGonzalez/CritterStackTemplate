# An Attempt At a Resonable Starter Project for the CritterStack

The configuration story for the Critter Stack is exemplary. It has every knob and dial a kid could want. However, it is a bit overwhelming in the "file->new project" story for newer developers getting started. 

## Using this Template

To experiment with this template (until I make it a Nuget, if I do):


### To Install The Template

If you just want to experiment, you can just clone this repository, or you can fork it and clone your copy locally if you are going to contribute.

Go into the `JasperFxTemplates/src` directory and run `dotnet pack`.

Type `dotnet new install bin/Release/JasperFx.Templates.0.0.1-beta.1.nupkg` to install the template.

![media/clone.gif](media/clone.gif)


To see the new templates, you can run:

```sh
dotnet new list jasperfx
```

**Note:** The `jasperfx` at the end just limits the list to those templates with that tag. Leave it off to see all your templates.

You can see the options for any template by appending the `--help` after `dotnet new TEMPLATENAME --help`

![media/list-help.gif](media/list-help.gif)


To uninstall the templates (you should do this whenever a new version is released. After it is on NuGet you can just update it) run:

```sh
dotnet new uninstall JasperFx.Templates
```

They should no longer appear when you run `dotnet new list`.



```
Template options:
  -da, --databaseName <databaseName>  Name of the database to connect to
                                      Type: string
                                      Default: critterstackdb
  -e, --exportOtel                    Export Open Telemetry Data
                                      Type: bool
                                      Default: true
  -p, --postgresPort <postgresPort>   Port to expose Postgres from Docker Compose
                                      Type: string
                                      Default: 5432
  -ad, --adminer                      Add Adminer to Docker Compose
                                      Type: bool
                                      Default: true
  -p:d, --dashboard                   Add the Aspire Dashboard to the Docker Compose
                                      Type: bool
                                      Default: true
  -c, --compose                       Create a Docker Compose File
                                      Type: bool
                                      Default: true
```

These are pretty self-explanatory, but need some work.

If you've installed this template, it should be available from the dotnet CLI to add a new project (example):

```
dotnet new critterapi -n GetmeStartedApi -p 5433 -ad false
```

This will create a new project with the name GetmeStartedApi, exposing (in the docker-compose.yml file) Postgres on port 5433 (and add that to the
connection string in the `appsettings.development.json`), and *not* add Adminer to the docker compose file.

This template can be used in Rider and Visual Studio. I've tested both on Linux (Pop_OS (Ubuntu) with Rider) and Windows (With Visual Studio and Rider),
I have not tested on a Mac yet, as I keep my Mac hidden lest someone think of me as a hipster or, worse, a designer. I will test it out, but if someone does first, let me know.


## Goal

I'd love to have a project template I could install for students in my class getting started with this. It might also be useful for others. So, ultimately something like:

```sh
dotnet new install CritterStackStarter
```

Or something like that. I'm a bit inspired (believe it or not) by the Aspire approach for the `ServiceDefaults` - which is have some code in the project that sets up the basic stuff, but make it just code - and obvious that you own it and can tweak it, etc.

Most of the meat of this is in `CritterStackExtensions.cs`. 

With this, the `Program.cs` just needs a couple of lines to get started.

```csharp
builder.AddCritterStackWebHost("ConnectionString");
```

And then, after the app is built,

```csharp
app.MapWolverineEndpoint();
await app.RunOaktonCommands(args);
```


Some of the things in the extension method:

### Adding Marten

Adding Marten with Wolverine integration. The OpenTelemetry stuff is added by flailing against the documentation - not sure yet what needs to stay here.

### Adding Wolverine
Storing the options in a static variable (feels bad) so that I can use them in the `ConfigureOpenTelemetry` stuff below.

### ConfigureOpenTelemetry

Just the basic stuff, taken from the JasperFX docs, and some borrowed from the Aspire stuff.

### AddDefaultHealthChecks

Just stuff from the Aspire sample code. I think it is probably an ok default. Needs more thought.

### AddOpenTelemetryExporter

The default configuration assumes you are just using the stand-alone Aspire dashboard (more on that below). That just uses the OtlpExporter, and the `grpc` exporter, which is the default. This is where you'd tweak for Prometheus or whatever.

### MapDefaultEndpoints

Not actually calling this from anywhere yet - would be in Program.cs - just copy-pastad from the Aspire stuff.

## Database

The sample uses a `docker-compose.yml` file in the `dev-environment` directory. It uses Postgres 16.2 by default, with dummy credentials. It initializes a new database with name `CritterStack` (in the `dev-environment/db/db.sql`).



## Dashboard

After doing `docker compose up -d` on the `/dev-environment/docker-compose.yml` file, run the following:

```sh
docker logs aspire-dashboard
```
Or, if you are on a more POSIXy shell, 

```sh
docker logs aspire-dashboard | grep "Login to the dashboard"
```

Find the line with the login url (like `http://0.0.0.0:188888/login?t=blahblahblah`) and use that to login and authenticate against the dashboard.

**Note**: Supposedly you can run the dashboard without needing that, but I was getting encryption errors.

**Note**: `appsettings.Development.json` has a connection string set to the default database, and the `OTEL_**` environment variables needed, including the service name. The `OTEL_SERVICE_NAME` has to match the application name for the dashboard not to be all stupid about it.

