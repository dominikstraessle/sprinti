{
  description = "A very basic flake";

  inputs = {
    nixpkgs.url = "nixpkgs";
    systemd-nix = {
      url = "github:serokell/systemd-nix";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = { self, nixpkgs, systemd-nix }:
    let forAllSystems = nixpkgs.lib.genAttrs [ "x86_64-linux" "aarch64-linux" ];
    in {
      packages = forAllSystems (system:
        let pkgs = nixpkgs.legacyPackages.${system};
        in {
          cvsharp = pkgs.callPackage ./cvsharp.nix { };
          sprinti = pkgs.callPackage ./sprinti.nix {
            inherit (self.packages.${system}) cvsharp;
          };
          # restish api sync sprinti
          # restish sprinti info-config
          inherit (pkgs) restish;
          # sprinti user needs to be in trusted-users in /etc/nix/nix.conf of the raspberry
          # nix copy --to ssh-ng://server .\#packages.aarch64-linux.sprinti
          # or
          # nix copy --no-check-sigs --from ssh-ng://server /nix/store/sxzvplbnl0m51qq5zkrc4jm4q4g8n3n2-sprinti-0.1
          default = self.packages.${system}.sprinti;
        });

    };
}
# nix flake update . # if it tries to update/copy whole nix store -> https://github.com/NixOS/nix/issues/4602