# Define the source and destination directories
srcdir="/home/dominik/aworkspace/study/pren/sprinti/src/Sprinti/img"
dstdir="/home/dominik/aworkspace/study/pren/sprinti/src/Tests/Stream/Images/Detection"

# Define the array of filenames
declare -a filenames=("20240419091756.png" "20240419091430.png" "20240419091435.png"
                      "20240419091440.png" "20240419091445.png" "20240419091450.png" "20240419091455.png" "20240419091500.png"
                      "20240419091505.png" "20240419091510.png" "20240419091515.png" "20240419092601.png" "20240419091605.png"
                      "20240419092631.png" "20240419091736.png" "20240419091741.png" "20240419092551.png")

# Iterate through each filename
for filename in "${filenames[@]}"; do
  # Copy the file if it exists
  if [[ -e "$srcdir/$filename" ]]; then
    cp "$srcdir/$filename" "$dstdir/"
  else
    echo "File $filename does not exist in $srcdir"
  fi
done