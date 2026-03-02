# HoloRed - Persistencia Políglota (Redis + Cassandra + Neo4j)

Trabajo AD - API REST en C# con arquitectura multicapa (Controllers).


# Tecnologías utilizadas

- .NET 9 (Web API)
- Docker & Docker Compose
- Redis (caché + TTL)
- Neo4j (grafo)
- Cassandra (NoSQL distribuido)
- Swagger (documentación API)

---

# Infraestructura

El entorno se levanta mediante Docker Compose.

Contenedores utilizados:

- Redis
- Neo4j
- Cassandra

Para arrancar el entorno:

```bash
docker compose up -d