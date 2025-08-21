#!/bin/bash

dotnet clean ./../discipline.sln && dotnet build ./../discipline.sln

echo "Going to discipline.centre project"
cd ./../discipline.centre/
projects=$(find . -name "*.unittests.csproj")

if [ -z "$projects" ]; then
    echo "Not tests projects found"
    exit 1
fi

for proj in $projects; do
    echo "=================================="
    echo "Running tests for $(basename $proj)"
    dotnet test "$proj" --no-build \
        --logger:"console;verbosity=minimal" 
done