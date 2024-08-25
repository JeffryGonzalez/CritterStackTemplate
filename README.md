# An Attempt At a Resonable Starter Project for the CritterStack

The configuration story for the Critter Stack is exemplary. It has every knob and dial a kid could want. However, it is a bit overwhelming in the "file->new project" story for newer developers getting started. 

## Goal

I'd love to have a project template I could install for students in my class getting started with this. It might also be useful for others. So, ultimately something like:

```sh
dotnet new install CritterStackStarter
```

Or something like that. I'm a bit inspired (believe it or not) by the Aspire approach for the `ServiceDefaults` - which is have some code in the project that sets up the basic stuff, but make it just code - and obvious that you own it and can tweak it, etc.

Most of the meat of this is in `CritterStackExtensions.cs`. 

With this, the `Program.cs` just needs a couple of lines to get started.

```csharp
builder.UseCritterStackWithReasonableDefaults();
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

