{
  description = "A very basic flake";

  inputs = { nixpkgs.url = "nixpkgs"; };

  outputs = { self, nixpkgs }: {

    packages.x86_64-linux.cvsharp =
      nixpkgs.legacyPackages.x86_64-linux.callPackage ./cvsharp.nix { };
    packages.x86_64-linux.sprinti =
      nixpkgs.legacyPackages.x86_64-linux.callPackage ./sprinti.nix { };

    packages.x86_64-linux.default = self.packages.x86_64-linux.sprinti;

  };
}
