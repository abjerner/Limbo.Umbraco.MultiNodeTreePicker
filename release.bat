@echo off
dotnet build src/Limbo.Umbraco.MultiNodeTreePicker --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget