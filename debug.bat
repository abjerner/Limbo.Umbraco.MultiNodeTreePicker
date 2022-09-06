@echo off
dotnet build src/Limbo.Umbraco.MultiNodeTreePicker --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=C:/nuget