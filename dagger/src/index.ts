/**
 * A generated module for Brew functions
 *
 * This module has been generated via dagger init and serves as a reference to
 * basic module structure as you get started with Dagger.
 *
 * Two functions have been pre-created. You can modify, delete, or add to them,
 * as needed. They demonstrate usage of arguments and return types using simple
 * echo and grep commands. The functions can be called from the dagger CLI or
 * from one of the SDKs.
 *
 * The first line in this comment block is a short description line and the
 * rest is a long description with more detail on the module's purpose or usage,
 * if appropriate. All modules should have a short description.
 */
import { dag, Container, Directory, object, func } from "@dagger.io/dagger"


@object()
class Brew {
  

  /**
   * Builds the Brew dotnet solution
   */
  @func()
  async build(directoryArg: Directory): Promise<Container> {
    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/sdk:8.0")
      .withMountedDirectory("/mnt", directoryArg)
      .withWorkdir("/mnt")
      .withExec(["dotnet", "build", "Brew.sln", "-c", "Release", "-o", "/app"])
  }

  /**
   * Runs the Brew dotnet solution
   */
  @func()
  async run(directoryArg: Directory): Promise<Container> {

    let build = await this.build(directoryArg)

    return dag
      .container()
      .from("mcr.microsoft.com/dotnet/runtime:8.0")
      .withMountedDirectory("/app", build.directory("/app"))
      .withWorkdir("/app")
      .withExec(["./Brew.Console"])
  }

  @func()
  async ci(): Promise<Directory> {
    
    return dag
      .gha()
      .withPipeline(
        "build and run features", 
        "run --directory-arg .", {
            onPushBranches: ["main"]
      })
      .config()      
    
  }

}


