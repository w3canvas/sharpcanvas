#!/bin/bash
find . -name "*.csproj" -print0 | while IFS= read -r -d $'\0' file; do
    sed -i 's|Include="..\\..\\SharpCanvas\\Browser\\|Include="..\\..\\SharpCanvas\\Legacy\\Browser\\|g' "$file"
    sed -i 's|Include="..\\Browser\\|Include="..\\Legacy\\Browser\\|g' "$file"
    sed -i 's|Include="..\\..\\SharpCanvas.Drawing\\|Include="..\\..\\SharpCanvas\\Legacy\\Drawing\\|g' "$file"
    sed -i 's|Include="..\\SharpCanvas.Drawing\\|Include="..\\SharpCanvas\\Legacy\\Drawing\\|g' "$file"
    sed -i 's|Include="..\\..\\SharpCanvas/|Include="..\\..\\..\\|g' "$file"
    sed -i 's|Include="..\\SharpCanvas/|Include="..\\..\\|g' "$file"
done

find . -name "*.sh" -print0 | while IFS= read -r -d $'\0' file; do
    sed -i 's|SharpCanvas/Legacy/Browser/|SharpCanvas/Legacy/Browser/|g' "$file"
    sed -i 's|SharpCanvas/Legacy/Drawing/|SharpCanvas/Legacy/Drawing/|g' "$file"
    sed -i 's|SharpCanvas/Legacy/Drawing/|SharpCanvas/Legacy/Drawing/|g' "$file"
done

find . -name "*.sln" -print0 | while IFS= read -r -d $'\0' file; do
    sed -i 's|Browser\\SharpCanvas.Browser.csproj|Legacy\\Browser\\SharpCanvas.Browser.csproj|g' "$file"
    sed -i 's|SharpCanvas.Drawing\\|SharpCanvas\\Legacy\\Drawing\\|g' "$file"
done
