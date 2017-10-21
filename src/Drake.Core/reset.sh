# Set current working directory to this script.
cd "${0%/*}"

rm ../../drake.db

dotnet clean && dotnet build
dotnet ef database update