@echo off
dotnet build src/Skybrud.Umbraco.MultiNodeTreePicker --configuration Debug /t:rebuild /t:pack -p:BuildTools=1 -p:PackageOutputPath=C:/nuget