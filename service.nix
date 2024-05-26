{ mkUserService, sprinti }:
mkUserService "sprinti" {
  description = "Sprinti Server";
  path = [ sprinti ];
  serviceConfig = {
    Type = "simple";
    ExecStart = "bin/sprinti";
  };
}
