#!/bin/sh

# INDEX_NAME='sample500000'  # TODO: pass it from docker-compose as env variable
cd /home/data

if [ ! -d documents ]; then
    echo "ERROR: Dataset expected in /home/data/documents, but not found."
    exit 1
fi

echo "Checking Index ..."
./import $PWD/documents "$INDEX_NAME"

echo "Starting app ..."
cd /home/install/src/NewsBrowser && dotnet run
