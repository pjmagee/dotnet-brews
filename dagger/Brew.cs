using System.Text.Json.Serialization;
using Dagger;
using static Dagger.Alias;

namespace Brew;

public class BrewModule : IJsonOnDeserialized
{
	/// <summary>
	/// A container that will build the solution in the given directory.
	/// </summary>
	public static Container Build([DirectoryFromContext(DefaultPath = ".")] Directory directoryArg)
	{
		var cache = DAG.CacheVolume(key: "nuget");
		
		return DAG
			.Container()
			.From("mcr.microsoft.com/dotnet/sdk:8.0")
			.WithMountedCache("/root/.nuget/packages", cache)
			.WithMountedDirectory("/mnt", directoryArg)
			.WithWorkdir("/mnt")
			.WithExec(["dotnet", "build", "Brew.sln", "-c", "Release", "-o", "/app"]);
	}

	/// <summary>
	/// run the built solution in the given directory.
	/// </summary>
	public static Container Run([DirectoryFromContext(DefaultPath = ".")] Directory directoryArg)
	{
		var build = Build(directoryArg);
		
		return DAG.Container()
			.From("mcr.microsoft.com/dotnet/runtime:8.0")
			.WithMountedDirectory("/app", build.Directory("/app"))
			.WithWorkdir("/app")
			.WithExec(["./Brew.Console"]);
	}

	/// <summary>
	/// dagger call ci export --path .
	/// </summary>
	public static Directory Ci()
	{
		return DAG.Gha().WithWorkflow(
			DAG.Gha().Workflow("build and run features").WithJob(DAG.Gha().Job(
					name: "build and run features",
					command: "run",
					runner: ["ubuntu-latest"]
				)
			)
		).Generate();
	}

	void IJsonOnDeserialized.OnDeserialized()
	{
		
	}
}