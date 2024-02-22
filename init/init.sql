CREATE UNLOGGED TABLE clientes (
	id SERIAL PRIMARY KEY,
	limite INTEGER NOT NULL,
	saldo INTEGER NOT NULL DEFAULT 0
);

CREATE UNLOGGED TABLE transacoes (
	id SERIAL PRIMARY KEY,
	cliente_id INTEGER NOT NULL,
	valor INTEGER NOT NULL,
	tipo CHAR(1) NOT NULL,
	descricao VARCHAR(255) NOT NULL,
	realizada_em TIMESTAMP NOT NULL DEFAULT NOW(),
	CONSTRAINT fk_clientes_transacoes_id
		FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

INSERT INTO clientes (limite) VALUES
	(100000),
	(80000),
	(1000000),
	(10000000),
	(500000);

CREATE EXTENSION IF NOT EXISTS pg_prewarm;
SELECT pg_prewarm('clientes');
SELECT pg_prewarm('transacoes');