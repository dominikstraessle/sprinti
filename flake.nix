{
  description = "A very basic flake";

  inputs = { nixpkgs.url = "nixpkgs"; };

  outputs = { self, nixpkgs }:
    let forAllSystems = nixpkgs.lib.genAttrs [ "x86_64-linux" "aarch64-linux" ];
    in {
      packages = forAllSystems (system:
        let pkgs = nixpkgs.legacyPackages.${system};
        in {
          cvsharp = pkgs.callPackage ./cvsharp.nix { };
          sprinti = pkgs.callPackage ./sprinti.nix {
            inherit (self.packages.${system}) cvsharp;
          };

          default = self.packages.${system}.sprinti;
        });

    };
}
