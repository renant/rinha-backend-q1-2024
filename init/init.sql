SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

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


CREATE INDEX IF NOT EXISTS idx_cliente_id ON clientes(id);
CREATE INDEX IF NOT EXISTS idx_transacao_id_cliente_realizada_em_desc ON transacoes(cliente_id, realizada_em DESC);

INSERT INTO clientes (limite) VALUES
	(100000),
	(80000),
	(1000000),
	(10000000),
	(500000);

CREATE OR REPLACE FUNCTION adicionar_transacao(id_cliente INT, tipo CHAR(1), valor NUMERIC, descricao VARCHAR)
	RETURNS TABLE (novo_saldo NUMERIC, novo_limite NUMERIC, validation_error BOOLEAN) AS $$
DECLARE
    diff INT;
    limite_cliente NUMERIC;
    saldo_cliente NUMERIC;
BEGIN
		IF tipo = 'd' THEN
        diff := valor * -1;
    ELSE
        diff := valor;
    END IF;

		UPDATE clientes 
		SET saldo = saldo + diff 
		WHERE id = id_cliente
		RETURNING saldo, limite INTO saldo_cliente, limite_cliente;

		IF (saldo_cliente + diff) < (-1 * limite_cliente) THEN
				RAISE 'LIMITE_INDISPONIVEL';
		ELSE 
			INSERT INTO transacoes (cliente_id, valor, tipo, descricao) VALUES (id_cliente, valor, tipo, descricao);

			RETURN QUERY SELECT saldo_cliente as novo_saldo, limite_cliente as novo_limite, false as validation_error;
		END IF;
		
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION extrato(p_id integer)
RETURNS json AS $$
DECLARE
    result json;
    row_count integer;
    v_saldo numeric;
    v_limite numeric;
BEGIN
    SELECT saldo, limite
    INTO v_saldo, v_limite
    FROM clientes
    WHERE id = p_id;

    SELECT json_build_object(
        'saldo', json_build_object(
            'total', v_saldo,
            'data_extrato', TO_CHAR(NOW(), 'YYYY-MM-DD"T"HH24:MI:SS.MS"Z"'),
            'limite', v_limite
        ),
        'ultimas_transacoes', COALESCE((
            SELECT json_agg(row_to_json(t)) FROM (
                SELECT valor, tipo, descricao, TO_CHAR(realizada_em, 'YYYY-MM-DD"T"HH24:MI:SS.MS"Z"') as realizada_em
                FROM transacoes
                WHERE cliente_id = p_id
                ORDER BY realizada_em DESC
                LIMIT 10
            ) t
        ), '[]')
    ) INTO result;

    RETURN result;
END;
$$ LANGUAGE plpgsql;