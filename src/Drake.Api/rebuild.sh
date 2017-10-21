# Set current working directory to this script.
cd "${0%/*}"

./../Drake.Indexer/rebuild.sh

dotnet clean && dotnet build

rm ./drake.db
cp ../Drake.Indexer/drake.db .
