# Set current working directory to this script.
cd "${0%/*}"

./../Drake.Core/rebuild.sh

dotnet clean && dotnet build

rm ./drake.db
cp ../Drake.Core/drake.db .

dotnet run https://github.com/loic-sharma/Drake.git
dotnet run https://github.com/tensorflow/tensorflow.git
