{ buildDotnetModule, dotnetCorePackages, opencv, gcc }:

buildDotnetModule rec {
  pname = "sprinti";
  version = "0.1";

  src = ./.;

  projectFile = "src/Sprinti.Api/Sprinti.Api.csproj";
  nugetDeps = ./deps.nix; # nix build .\#sprinti.passthru.fetch-deps

  passthru.updateScript = ./updater.sh;

  dotnet-sdk = dotnetCorePackages.dotnet_8.sdk;
  selfContainedBuild = true;
  #  executables = [ "foo" ]; # This wraps "$out/lib/$pname/foo" to `$out/bin/foo`.
  executables = [ "Sprinti.Api" ]; # Don't install any executables.
  nativeBuildInputs = [ gcc ];

  #  packNupkg = true; # This packs the project as "foo-0.1.nupkg" at `$out/share`.
  dotnetFlags = [
    #        "-p:PublishTrimmed=true"
    #    "--self-contained"
    #"--runtime linux-arm64 --self-contained"
  ];
  runtimeDeps = [
    opencv
    dotnetCorePackages.dotnet_8.runtime
  ]; # This will wrap ffmpeg's library path into `LD_LIBRARY_PATH`.
}

#https://github.com/NixOS/nixpkgs/blob/c4df2f03974c8d19df0fb5625211238692dcc0ef/pkgs/by-name/ry/ryujinx/package.nix
