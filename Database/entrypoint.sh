#!/bin/bash
migrate -path=/app/migrations -database $PSQL_CONN up