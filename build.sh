#!/bin/bash

echo "Đang build file cho Windows..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

echo "Đang build file cho Linux..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true

echo ""
echo "Build hoàn tất!"
echo "File Windows: bin/Release/net*/win-x64/publish/"
echo "File Linux:   bin/Release/net*/linux-x64/publish/"
