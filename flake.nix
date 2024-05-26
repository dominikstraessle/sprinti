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
          service = pkgs.callPackage ./service.nix {
            inherit (self.packages.${system}) sprinti;
            inherit (systemd-nix.lib.${system}) mkUserService;
          };
          # restish api sync sprinti
          # restish sprinti info-config
          inherit (pkgs) restish;
          # sprinti user needs to be in trusted-users in /etc/nix/nix.conf of the raspberry
          # nix copy --to ssh-ng://sprinti@sprinti.secure.straessle.me .\#packages.aarch64-linux.sprinti
          # or
          # nix copy --no-check-sigs --from ssh-ng://dominik@100.117.127.123 /nix/store/sxzvplbnl0m51qq5zkrc4jm4q4g8n3n2-sprinti-0.1
          default = self.packages.${system}.sprinti;
        });

    };
}
