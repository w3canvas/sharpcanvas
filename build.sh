#!/bin/bash
./dotnet-install.sh
export DOTNET_ROOT=/home/jules/.dotnet
export PATH=$PATH:$DOTNET_ROOT
dotnet restore SharpCanvas/SharpCanvas.sln
dotnet build SharpCanvas/SharpCanvas.sln --no-restore
