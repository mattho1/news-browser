#!/bin/sh

DOC_PATH="$1"
INDEX_NAME="$2"
# INDEX_NAME='sample500000'  # TODO: pass it from docker-compose as env variable

if [ ! -d "$DOC_PATH" ]; then
    echo "ERROR: Dataset expected in /home/data/documents, but not found."
    exit 1
fi

echo "Checking Index ..."
cd /home/data
./import "$DOC_PATH" "$INDEX_NAME"

# hask to avoid maintaining two versions of config file
es_docker_host_name='news-elasticsearch'
app_config_file='/home/install/src/NewsBrowser/appsettings.json'
sed -i'' 's/localhost/'"$es_docker_host_name"'/g' "$app_config_file"

echo "Starting app ..."
cd /home/install/src/NewsBrowser && dotnet run
