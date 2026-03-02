USE holored;

CREATE TABLE IF NOT EXISTS impactos (
  sector_id text,
  fecha date,
  ts timestamp,
  nave_atacante text,
  nave_objetivo text,
  dano_escudos int,
  PRIMARY KEY ((sector_id, fecha), ts)
) WITH CLUSTERING ORDER BY (ts DESC);