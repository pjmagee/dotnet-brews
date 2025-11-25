using Dagger;

namespace DotnetBrews;

/// <summary>
/// A Dagger module for building and running the dotnet-brews solution
///
/// This module provides functions to build, run, and generate CI workflows
/// for the dotnet-brews project using Dagger.
/// </summary>
[Object]
public class DotnetBrews
{
    /// <summary>
    /// A container that will build the solution in the given directory.
    /// </summary>
    [Function]
    public Container Build([DefaultPath("/")] Directory directoryArg)
    {
        var cache = Dag.CacheVolume("nuget");

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/sdk:10.0")
            .WithMountedCache("/root/.nuget/packages", cache)
            .WithMountedDirectory("/mnt", directoryArg)
            .WithWorkdir("/mnt")
            .WithExec(new[] { "dotnet", "build", "Brew.slnx", "-c", "Release", "-o", "/app" });
    }

    /// <summary>
    /// Run the built solution in the given directory.
    /// </summary>
    [Function]
    public Container Run([DefaultPath("/")] Directory directoryArg)
    {
        var build = Build(directoryArg);

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/runtime:10.0")
            .WithMountedDirectory("/app", build.Directory("/app"))
            .WithWorkdir("/app")
            .WithExec(new[] { "./Brew.Console" });
    }

    /// <summary>
    /// List all available brews
    /// </summary>
    [Function]
    public Container List([DefaultPath("/")] Directory directoryArg)
    {
        var build = Build(directoryArg);

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/runtime:10.0")
            .WithMountedDirectory("/app", build.Directory("/app"))
            .WithWorkdir("/app")
            .WithExec(new[] { "./Brew.Console", "list" });
    }

    /// <summary>
    /// Run a specific brew by name
    /// </summary>
    [Function]
    public Container RunBrew(string brewName, [DefaultPath("/")] Directory directoryArg)
    {
        var build = Build(directoryArg);

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/runtime:10.0")
            .WithMountedDirectory("/app", build.Directory("/app"))
            .WithWorkdir("/app")
            .WithExec(new[] { "./Brew.Console", "run", brewName });
    }

    /// <summary>
    /// Generate GitHub Actions workflows
    /// Usage: dagger call ci export --path .
    /// </summary>
    [Function]
    public async Task<Directory> Ci([DefaultPath("/")] Directory directoryArg)
    {
        // Get the list of available brews by executing the list command
        var listContainer = List(directoryArg);
        var brewListOutput = await listContainer.StdoutAsync();

        // Parse the brew names from the output (format: "- BrewName: Description")
        var brewNames = brewListOutput
            .Split('\n')
            .Where(line => line.Trim().StartsWith('-'))
            .Select(line => line.Trim().Substring(2).Split(':')[0].Trim())
            .Where(name => name.Length > 0)
            .ToList();

        // Workflow 1: CI Build (for README badge and PR checks)
        var buildJob = Dag.Gha().Job("build", "build", runner: new[] { "ubuntu-latest" });
        var buildWorkflow = Dag.Gha().Workflow("ci").WithJob(buildJob);

        // Workflow 2: List all available brews
        var listJob = Dag.Gha().Job("list", "list", runner: new[] { "ubuntu-latest" });
        var listWorkflow = Dag.Gha().Workflow("list-brews").WithJob(listJob);

        // Workflow 3: Run all brews in parallel
        var runAllJob = Dag.Gha().Job("run-all", "run", runner: new[] { "ubuntu-latest" });
        var runAllWorkflow = Dag.Gha().Workflow("run-all-brews").WithJob(runAllJob);

        // Workflow 4: Run individual brews (demonstrates running a specific brew)
        // Pick the first brew as an example
        var exampleBrewName = brewNames.Count > 0 ? brewNames[0] : "Brew.Features.Builders.Simple";
        var runOneJob = Dag.Gha()
            .Job(
                "run-one",
                $"run-brew --brew-name=\"{exampleBrewName}\"",
                runner: new[] { "ubuntu-latest" }
            );
        var runOneWorkflow = Dag.Gha().Workflow("run-one-brew").WithJob(runOneJob);

        return Dag.Gha()
            .WithWorkflow(buildWorkflow)
            .WithWorkflow(listWorkflow)
            .WithWorkflow(runAllWorkflow)
            .WithWorkflow(runOneWorkflow)
            .Generate();
    }
}
