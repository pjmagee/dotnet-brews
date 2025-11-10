/**
 * A Dagger module for building and running the dotnet-brews solution
 *
 * This module provides functions to build, run, and generate CI workflows
 * for the dotnet-brews project using Dagger.
 */
import { 
  dag,
  Container, 
  Directory, 
  object, 
  func, 
  GhaJobOpts, 
  argument, 
  GhaJob} from "@dagger.io/dagger"

@object()
export class DotnetBrews {

  /**
   * A container that will build the solution in the given directory.
   */
  @func()
  build(@argument({ defaultPath: "/" }) directoryArg: Directory): Container {
    const cache = dag.cacheVolume("nuget")
    
    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/sdk:10.0")
      .withMountedCache("/root/.nuget/packages", cache)
      .withMountedDirectory("/mnt", directoryArg)
      .withWorkdir("/mnt")
      .withExec(["dotnet", "build", "Brew.slnx", "-c", "Release", "-o", "/app"])
  }

  /**
   * Run the built solution in the given directory.
   */
  @func()
  run(@argument({ defaultPath: "/" }) directoryArg: Directory): Container {
    const build = this.build(directoryArg)
    
    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/runtime:10.0")
      .withMountedDirectory("/app", build.directory("/app"))
      .withWorkdir("/app")
      .withExec(["./Brew.Console"])
  }

  /**
   * List all available brews
   */
  @func()
  list(@argument({ defaultPath: "/" }) directoryArg: Directory): Container {
    const build = this.build(directoryArg)
    
    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/runtime:10.0")
      .withMountedDirectory("/app", build.directory("/app"))
      .withWorkdir("/app")
      .withExec(["./Brew.Console", "list"])
  }

  /**
   * Run a specific brew by name
   */
  @func()
  runBrew(brewName: string, @argument({ defaultPath: "/" }) directoryArg: Directory): Container {
    const build = this.build(directoryArg)
    
    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/runtime:10.0")
      .withMountedDirectory("/app", build.directory("/app"))
      .withWorkdir("/app")
      .withExec(["./Brew.Console", "run", brewName])
  }

  /**
   * Generate GitHub Actions workflows
   * Usage: dagger call ci export --path .
   */
  @func()
  async ci(@argument({ defaultPath: "/" }) directoryArg: Directory): Promise<Directory> {

    // Get the list of available brews by executing the list command
    const listContainer = this.list(directoryArg)
    const brewListOutput = await listContainer.stdout()
    
    // Parse the brew names from the output (format: "- BrewName: Description")
    const brewNames = brewListOutput
      .split('\n')
      .filter(line => line.trim().startsWith('-'))
      .map(line => line.trim().substring(2).split(':')[0].trim())
      .filter(name => name.length > 0)

    // Workflow 1: List all available brews
    const listJobOpts: GhaJobOpts = {
      runner: ["ubuntu-latest"],     
    }
    const listJob: GhaJob = dag.gha().job("list", "list", listJobOpts)
    const listWorkflow = dag.gha().workflow("list-brews").withJob(listJob)

    // Workflow 2: Run all brews in parallel
    const runAllJobOpts: GhaJobOpts = {
      runner: ["ubuntu-latest"],     
    }
    const runAllJob: GhaJob = dag.gha().job("run-all", "run", runAllJobOpts)
    const runAllWorkflow = dag.gha().workflow("run-all-brews").withJob(runAllJob)

    // Workflow 3: Run individual brews (demonstrates running a specific brew)
    // Pick the first brew as an example
    const exampleBrewName = brewNames.length > 0 ? brewNames[0] : "Brew.Features.Builders.Simple"
    const runOneJobOpts: GhaJobOpts = {
      runner: ["ubuntu-latest"],
    }
    const runOneJob: GhaJob = dag.gha().job("run-one", `run-brew --brew-name="${exampleBrewName}"`, runOneJobOpts)
    const runOneWorkflow = dag.gha().workflow("run-one-brew").withJob(runOneJob)

    return dag
      .gha()
      .withWorkflow(listWorkflow)
      .withWorkflow(runAllWorkflow)
      .withWorkflow(runOneWorkflow)
      .generate()
  }
}
