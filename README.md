# news-browser
Repozytorium zawiera aplikację umożliwiającą wyszukiwanie artykułów prasowych. Aplikacja składa się z napisaną w C# (.NET core) oraz TypeScript (Angular). Aplikacja wykorzystuje silnik Elasticsearch.
Istnieje możliwość łatwego uruchomienia aplikacji z użyciem kontenera Docker.


# Dataset
W celu pobrania datasetu konieczne jest zarejestrowanie się na stronie.
Adres datasetu: https://webhose.io/free-datasets/english-news-articles/

# uruchomienie
**UWAGA: Należy uzupełnić ścieżkę do katalogu z dokumentami w docker/docker-compose.yml:**
**Podmienić '/PATH/TO/DOCUMENTS/DIR' na ścieżkę w swoim systemie do katalogu z dokumentami**
W celu uruchomienia należy wywołać:
```
docker-compose up -d
```
Polecenie uruchomi silnik Elasticsearch oraz aplikację.

Po uruchomieniu silnika i aplikacji (nie dłużej niż 5 minut), aplikacja będzie dostępna pod adresem
**http://localhost:15001**

W celu zatrzymania usług należy wywołać:
```
docker-compose down
```

# przydatne polecenia
1. W przypadku problemów można je zdiagnozować podglądając logi:
* aplikacja: ```docker-compose logs news-webapp```
* elasticsearch: ```docker-compose logs news-webapp```

1. Usunięcie indeksu elasticsearch:
```docker volume rm docker_news_es_volume```
