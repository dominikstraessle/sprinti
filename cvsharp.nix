{ stdenv, fetchFromGitHub, opencv, cmake }:

stdenv.mkDerivation rec {
  pname = "opencvsharp";
  version = "4.9.0.20240106";

  nativeBuildInputs = [ cmake ];

  src = fetchFromGitHub {
    owner = "shimat";
    repo = pname;
    rev = version;
    sha256 = "sha256-jXE8WLQ6C0SOjz/m01xn0Znw0lZA56OW0odL5BxpokY=";
  };
  configurePhase = ''
    cmake -D CMAKE_INSTALL_PREFIX=${opencv} $src/src
  '';
  buildPhase = ''
    make -j
  '';
  installPhase = ''
    mkdir -p $out/lib
    cp OpenCvSharpExtern/libOpenCvSharpExtern.so $out/lib
  '';
  runtimeDeps = [ opencv ];
}
