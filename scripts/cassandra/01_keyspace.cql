-- Keyspace para desarrollo (1 nodo)
CREATE KEYSPACE IF NOT EXISTS holored
WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};

USE holored;
