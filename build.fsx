// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open System
open Fake
open Fake.Testing

// Directories
let buildDir  = "build/"
let deployDir = "deploy/"

// TODO
// Create local git repos.

let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    let objDirs = 
        !! "/**/bin" ++ "/**/obj"
            |> Seq.toList

    CleanDirs ([buildDir; deployDir] @ objDirs)
)

Target "BuildApp" (fun _ ->
    let appReferences =
        !! "src/app/**/*.csproj" ++ "src/app/**/*.fsproj"

    MSBuildDebug buildDir "Build" appReferences
        |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
    let testReferences =
        !! "src/test/**/*.csproj" ++ "src/test/**/*.fsproj"

    MSBuildDebug buildDir "Build" testReferences
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    !! (buildDir + "Test*.dll")
        |> NUnit3 (fun p ->
            { p with
                Labels = LabelsLevel.All
                TimeOut = TimeSpan.FromMinutes 20.
                OutputDir = buildDir + "TestResults.xml" }) // The Property name is wrong. It's a file rather than a dir.
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*") -- "*.zip"
        |> Zip buildDir (deployDir + "ApplicationName." + version + ".zip")
)

"BuildApp" ==> "BuildTest"
"BuildTest" ==> "Test"
"Test" ==> "Deploy"

RunTargetOrDefault "BuildApp"
