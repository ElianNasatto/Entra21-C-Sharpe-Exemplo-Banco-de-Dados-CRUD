drop table carro;
CREATE TABLE carro(
 id INT PRIMARY KEY IDENTITY(1,1),
 cor VARCHAR(50),
 modelo VARCHAR(80),
 preco DECIMAL(10,2),
 ano INT
);

insert into carro (cor, modelo, preco, ano ) values ('Amarelo','palio',2000.00,2010);
insert into carro (cor, modelo, preco, ano ) values ('dasd','ads',454.00,2010);

select * from carro;

update c