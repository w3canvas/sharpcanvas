#!/bin/bash
find . -name "*.csproj" -print0 | while IFS= read -r -d $'\0' file; do
    sed -i 's|../../SharpCanvas/|../../../|g' "$file"
    sed -i 's|../SharpCanvas/|../../|g' "$file"
done
