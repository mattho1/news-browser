# news-browser

# Dataset
W celu pobrania datasetu konieczne jest zarejestrowanie się na stronie.
Adres datasetu: https://webhose.io/free-datasets/english-news-articles/

# uruchomienie
**Należy uzupełnić ścieżkę do katalogu z dokumentami w docker-compose.yml, linia 10**
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
