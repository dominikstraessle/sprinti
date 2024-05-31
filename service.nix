{ mkUserService, sprinti }:
mkUserService "sprinti" {
  description = "Sprinti Server";
  path = [ sprinti ];
  serviceConfig = {
    Type = "simple";
    ExecStart = "${sprinti}/bin/sprinti --contentRoot $SPRINTI_HOME";
    Environment = "SPRINTI_HOME=/home/sprinti/sprinti";
  };
}
