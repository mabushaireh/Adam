#tool "nuget:?package=GitReleaseNotes"
#tool nuget:?package=GitVersion.CommandLine

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "netcoreapp2.0");
var outputDir = "./artifacts/";
var webProject = "./src/Web/i2fam.Web.csproj";
var isAppVeyor = BuildSystem.IsRunningOnAppVeyor;
var isWindows = IsRunningOnWindows();


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////
////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

// Clean Folders
Task("Clean")
    .Does(() =>
    {
        if (DirectoryExists(outputDir))
        {
            DeleteDirectory(outputDir, recursive:true);
        }       
    });

// Restore Missing Nuget and Npm packages.
Task("Restore")
    .Does(() => {
        DotNetCoreRestore("./i2fam.sln", new DotNetCoreRestoreSettings{
            Verbosity = DotNetCoreVerbosity.Minimal,
        });
    });


GitVersion versionInfo = null;
DotNetCoreMSBuildSettings msBuildSettings = null;

Task("Version")
    .Does(() => {
        GitVersion(new GitVersionSettings{
            UpdateAssemblyInfo = false,
            OutputType = GitVersionOutput.BuildServer
        });
        
        versionInfo = GitVersion(new GitVersionSettings{ OutputType = GitVersionOutput.Json });
        
        msBuildSettings = new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", versionInfo.NuGetVersion)
                            .WithProperty("AssemblyVersion", versionInfo.AssemblySemVer)
                            .WithProperty("FileVersion", versionInfo.AssemblySemVer);
    });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Version")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild("./i2fam.sln", new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            OutputDirectory = outputDir,
            MSBuildSettings = msBuildSettings
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        //DotNetCoreTool("./src/Shouldly.Tests/Shouldly.Tests.csproj", "xunit", "-configuration Debug");
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCorePack(webProject, new DotNetCorePackSettings
        {
            Configuration = configuration,
            OutputDirectory = outputDir,
            MSBuildSettings = msBuildSettings
        });

        if (!isWindows) return;

        // TODO not sure why this isn't working
        // GitReleaseNotes("outputDir/releasenotes.md", new GitReleaseNotesSettings {
        //     WorkingDirectory         = ".",
        //     AllTags                  = false
        // });

        var releaseNotesExitCode = StartProcess(
            @"tools\GitReleaseNotes\tools\gitreleasenotes.exe", 
            new ProcessSettings { Arguments = ". /o artifacts/releasenotes.md" });
        if (string.IsNullOrEmpty(System.IO.File.ReadAllText("./artifacts/releasenotes.md")))
            System.IO.File.WriteAllText("./artifacts/releasenotes.md", "No issues closed since last release");

        if (releaseNotesExitCode != 0) throw new Exception("Failed to generate release notes");

        System.IO.File.WriteAllLines(outputDir + "artifacts", new[]{
            "nuget:Web." + versionInfo.NuGetVersion + ".nupkg",
            "nugetSymbols:Shouldly." + versionInfo.NuGetVersion + ".symbols.nupkg",
            "releaseNotes:releasenotes.md"
        });

        if (isAppVeyor)
        {
            foreach (var file in GetFiles(outputDir + "**/*"))
                AppVeyor.UploadArtifact(file.FullPath);
        }
    });

Task("Default")
    .IsDependentOn("Version");

RunTarget(target);


RunTarget(target);