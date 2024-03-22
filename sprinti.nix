{ buildDotnetModule, dotnetCorePackages, opencv, cvsharp }:

buildDotnetModule rec {
  pname = "sprinti";
  version = "0.1";

  src = ./.;

  projectFile = "src/Sprinti.Api/Sprinti.Api.csproj";
  testProjectFile = "src/Sprinti.Tests/Sprinti.Tests.csproj";
  nugetDeps = ./deps.nix; # nix build .\#sprinti.passthru.fetch-deps

  passthru.updateScript = ./updater.sh;

  doCheck = true;

  dotnet-sdk = dotnetCorePackages.dotnet_8.sdk;
  selfContainedBuild = true;
  executables = [
    "Sprinti.Api"
  ]; # This wraps "$out/lib/$pname/Sprinti.Api" to `$out/bin/Sprinti.Api`.

  runtimeDeps = [
    opencv
    cvsharp
  ]; # This will wrap opencv's library path into `LD_LIBRARY_PATH`.
}
