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
            .WithExec(["dotnet", "build", "Brew.slnx", "-c", "Release", "-o", "/app"]);
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
            .WithExec(["./Brew.Console"]);
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
            .WithExec(["./Brew.Console", "list"]);
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
            .WithExec(["./Brew.Console", "run", brewName]);
    }

    /// <summary>
    /// Format the repository with CSharpier and return the updated tree
    /// Usage: dagger call format export --path .
    /// </summary>
    [Function]
    public Directory Format([DefaultPath("/")] Directory directoryArg)
    {
        // reuse the nuget cache from build so restoring tools is fast
        var cache = Dag.CacheVolume("nuget");

        var container = Dag.Container()
            .From("mcr.microsoft.com/dotnet/sdk:10.0")
            .WithMountedCache("/root/.nuget/packages", cache)
            .WithMountedDirectory("/mnt", directoryArg)
            .WithWorkdir("/mnt")
            // restore the local tool manifest and run CSharpier in-place
            .WithExec([
                "sh",
                "-c",
                "export PATH=/root/.dotnet/tools:/usr/local/bin:/usr/bin:/bin && dotnet tool restore && (/root/.dotnet/tools/csharpier format . || dotnet tool run csharpier -- format .)",
            ]);

        var resultDir = container.Directory("/mnt");
        return resultDir.WithoutDirectory(".git");
    }

    /// <summary>
    /// Run repository checks: restore tools, run CSharpier in --check mode, and build the solution.
    /// Usage: dagger call check --path .
    /// </summary>
    [Function]
    [Check]
    public Container Check([DefaultPath("/")] Directory directoryArg)
    {
        var cache = Dag.CacheVolume("nuget");

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/sdk:10.0")
            .WithMountedCache("/root/.nuget/packages", cache)
            .WithMountedDirectory("/mnt", directoryArg)
            .WithWorkdir("/mnt")
            // restore local tools and run CSharpier in check mode so CI fails on formatting issues
            .WithExec([
                "sh",
                "-c",
                "export PATH=/root/.dotnet/tools:/usr/local/bin:/usr/bin:/bin && dotnet tool restore && (/root/.dotnet/tools/csharpier check . || dotnet tool run csharpier -- check .)",
            ])
            // build the solution to catch compilation errors
            .WithExec(["dotnet", "build", "Brew.slnx", "-c", "Release"]);
    }

    /// <summary>
    /// Run only CSharpier in check mode as a named Dagger check.
    /// Usage: `dagger call CSharpierCheck --path .`
    /// </summary>
    [Function]
    [Check]
    public Container CSharpierCheck([DefaultPath("/")] Directory directoryArg)
    {
        var cache = Dag.CacheVolume("nuget");

        return Dag.Container()
            .From("mcr.microsoft.com/dotnet/sdk:10.0")
            .WithMountedCache("/root/.nuget/packages", cache)
            .WithMountedDirectory("/mnt", directoryArg)
            .WithWorkdir("/mnt")
            .WithExec([
                "sh",
                "-c",
                "export PATH=/root/.dotnet/tools:/usr/local/bin:/usr/bin:/bin && dotnet tool restore && (/root/.dotnet/tools/csharpier check . || dotnet tool run csharpier -- check .)",
            ]);
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
        var brewListOutput = await listContainer.Stdout();

        // Parse the brew names from the output (format: "- BrewName: Description")
        var brewNames = brewListOutput
            .Split('\n')
            .Where(line => line.Trim().StartsWith('-'))
            .Select(line => line.Trim().Substring(2).Split(':')[0].Trim())
            .Where(name => name.Length > 0)
            .ToList();

        // Workflow 1: CI Build (for README badge and PR checks)
        var buildJob = Dag.Gha().Job("build", "build", runner: ["ubuntu-latest"]);
        var buildWorkflow = Dag.Gha().Workflow("ci").WithJob(buildJob);

        // Workflow 2: List all available brews
        var listJob = Dag.Gha().Job("list", "list", runner: ["ubuntu-latest"]);
        var listWorkflow = Dag.Gha().Workflow("list-brews").WithJob(listJob);

        // Workflow 3: Run all brews in parallel
        var runAllJob = Dag.Gha().Job("run-all", "run", runner: ["ubuntu-latest"]);
        var runAllWorkflow = Dag.Gha().Workflow("run-all-brews").WithJob(runAllJob);

        // Workflow 4: Run individual brews (demonstrates running a specific brew)
        // Pick the first brew as an example
        var exampleBrewName = brewNames.Count > 0 ? brewNames[0] : "Brew.Features.Builders.Simple";
        var runOneJob = Dag.Gha()
            .Job(
                "run-one",
                $"run-brew --brew-name=\"{exampleBrewName}\"",
                runner: ["ubuntu-latest"]
            );
        var runOneWorkflow = Dag.Gha().Workflow("run-one-brew").WithJob(runOneJob);

        // Workflow 5: Run dagger checks (formats/builds/tests via `dagger check`)
        var daggerCheckJob = Dag.Gha().Job("dagger-check", "dagger check", runner: ["ubuntu-latest"]);
        var daggerCheckWorkflow = Dag.Gha().Workflow("dagger-check").WithJob(daggerCheckJob);

        return Dag.Gha()
            .WithWorkflow(buildWorkflow)
            .WithWorkflow(listWorkflow)
            .WithWorkflow(runAllWorkflow)
            .WithWorkflow(runOneWorkflow)
            .WithWorkflow(daggerCheckWorkflow)
            .Generate();
    }
}
