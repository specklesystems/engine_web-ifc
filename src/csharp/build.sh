#!/usr/bin/env bash
set -euo pipefail

dotnet run --project src/csharp/build/build.csproj -- "$@"
