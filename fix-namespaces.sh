#!/bin/bash

# Function to fix a file's namespace
fix_file() {
    file=$1
    echo "Fixing $file"
    
    # Get the line with namespace declaration
    namespace_line=$(grep -n "namespace Bi5\.Net" "$file" | head -1)
    
    if [[ "$namespace_line" == *";"* ]]; then
        # Extract line number and namespace
        line_num=$(echo "$namespace_line" | cut -d ':' -f 1)
        namespace=$(echo "$namespace_line" | cut -d ':' -f 2- | sed 's/;$//')
        
        # Replace the file-scoped namespace with traditional namespace
        sed -i "${line_num}s/${namespace};/${namespace}\n{/" "$file"
        
        # Add closing brace at the end of the file
        echo "}" >> "$file"
        
        echo "  Fixed namespace in $file"
    else
        echo "  No file-scoped namespace found in $file"
    fi
}

# Get all CS files in the Bi5.Net project
files=$(find /mnt/c/Users/User/RiderProjects/Dukas.Net/Bi5.Net -name "*.cs")

# Fix each file
for file in $files; do
    fix_file "$file"
done

echo "All done!"