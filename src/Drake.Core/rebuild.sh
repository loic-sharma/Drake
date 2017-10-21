# Set current working directory to this script.
cd "${0%/*}"

rm -r ./Migrations
rm ./drake.db

dotnet clean && dotnet build

dotnet ef migrations add Initial
dotnet ef database update