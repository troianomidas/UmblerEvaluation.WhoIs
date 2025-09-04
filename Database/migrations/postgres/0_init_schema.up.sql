-- garante uuid aleatório
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE IF NOT EXISTS domains (
  id          UUID          NOT NULL DEFAULT gen_random_uuid(),
  domain_name VARCHAR(255)  NOT NULL,
  ip_address  VARCHAR(45)   NOT NULL,   -- 45 para futuro IPv6
  who_is      TEXT          NOT NULL,
  ttl         INT           NOT NULL,
  hosted_at   VARCHAR(255)  NOT NULL,
  created_at  TIMESTAMPTZ   NOT NULL DEFAULT NOW(),
  updated_at  TIMESTAMPTZ   NULL     DEFAULT NULL,
  PRIMARY KEY (id)
);