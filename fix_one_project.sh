#!/bin/bash
file=$1
sed -i 's|../../SharpCanvas/|../../../|g' "$file"
sed -i 's|../SharpCanvas/|../../|g' "$file"
