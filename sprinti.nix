{ buildDotnetModule, dotnetCorePackages, opencv, cvsharp }:

buildDotnetModule rec {
  pname = "sprinti";
  version = "0.1";

  src = ./.;

  projectFile = "src/Sprinti/Sprinti.csproj";
  testProjectFile = "src/Tests/Tests.csproj";
  # nix build .\#sprinti.passthru.fetch-deps
  # ./result deps.nix
  nugetDeps = ./deps.nix;

  doCheck = false;

  dotnet-sdk = dotnetCorePackages.dotnet_8.sdk;
  selfContainedBuild = true;
  executables = [
    "sprinti"
  ]; # This wraps "$out/lib/$pname/Sprinti.Api" to `$out/bin/Sprinti.Api`.

  runtimeDeps = [
    opencv
    cvsharp
  ]; # This will wrap opencv's library path into `LD_LIBRARY_PATH`.
}
