{ stdenv, fetchFromGitHub, opencv, cmake }:
let opencvgtk = opencv.override { enableGtk2 = true; };
in
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
  # https://github.com/shimat/opencvsharp/issues/1193#issuecomment-769812308
  configurePhase = ''
    cmake -D CMAKE_INSTALL_PREFIX=${opencvgtk} -D WITH_GTK=ON $src/src
  '';
  buildPhase = ''
    make -j
  '';
  installPhase = ''
    mkdir -p $out/lib
    cp OpenCvSharpExtern/libOpenCvSharpExtern.so $out/lib
  '';
  runtimeDeps = [ opencvgtk ];
}
