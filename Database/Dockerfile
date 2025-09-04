FROM migrate/migrate

WORKDIR /app

COPY ./entrypoint.sh .
COPY /migrations/postgres/ ./migrations

ENTRYPOINT ["sh", "/app/entrypoint.sh"]