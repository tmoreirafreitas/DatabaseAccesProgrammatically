-- Seta o uso do banco de dados Master
USE[master]

-- Se o banco SVDB existir, apaga o mesmo,
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SVDB')
DROP DATABASE [SVDB];

-- Se o banco não existir, cria.
IF  NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'SVDB')
CREATE DATABASE [SVDB];

-- Seta o uso do banco de dados
USE [SVDB]
GO

-- Cria as Tabelas do banco de dados
create table [Socio](
id bigint not null,
nome varchar(80) not null,
aniversario date null,
rg char(10) null,
cpf char(12) null,
email varchar(255) not null,
constraint pk_socio primary key(id),
constraint uk_email unique(email)
);

create table [Endereco](
id int identity(1,1) not null,
idsocio bigint not null,
rua varchar(255) not null,
numero int null,
complemento text null,
cep char(9) null,
bairro varchar(255) null,
cidade varchar(255) null,
estado varchar(100) null,
constraint pk_Endereco primary key(id),
constraint fk_endereco_socio foreign key(idsocio) references Socio(id)
);

create table [Ator](
id int identity(1,1) not null,
nome varchar(100),
constraint pk_ator primary key(id)
);

create table [Genero](
id int identity(1,1) not null,
descricao varchar(255),
constraint pk_genero primary key(id)
);

create table [Categoria](
id int identity(1,1) not null,
descricao varchar(255) not null,
valor_locacao numeric(4,2) not null,
constraint pk_categoria primary key(id)
);

create table [Filme](
id int identity(1,1) not null,
idgenero int not null,
idcategoria int not null,
titulo varchar(255) not null,
duracao varchar(10) not null,
constraint pk_filme primary key(id),
constraint fk_categoria_filme foreign key(idcategoria) references Categoria(id),
constraint fk_genero_filme foreign key(idgenero) references Genero(id)
);

create table [Atua](
idator int not null,
idfilme int not null,
papel varchar(255) not null,
constraint pk_atua primary key(idator,idfilme),
constraint fk_atua_ator foreign key(idator) references Ator(id),
constraint fk_atua_filme foreign key(idfilme) references Filme(id)
);

create table [Locacao](
id bigint identity(1,1) not null,
idsocio bigint not null,
data_locacao date not null,
data_devolucao date null,
status bit null,
constraint pk_Locacao primary key(id),
constraint fk_locacao_socio foreign key(idsocio) references Socio(id)
);

create table [Copia](
id int identity(1,1) not null,
idfilme int not null,
datacopia date null,
situacao_copia bit null,
constraint pk_copia primary key(id),
constraint fk_copia_filme foreign key(idfilme) references Filme(id)
);

create table [Item_Locacao](
idlocacao bigint not null,
idcopia int not null,
valor_locacao numeric(8,2) not null,
constraint pk_item_locacao primary key(idlocacao, idcopia),
constraint fk_item_locacao foreign key(idlocacao) references Locacao(id),
constraint fk_item_locacao_copia foreign key(idcopia) references Copia(id)
);

create table [Telefone](
id bigint identity(1,1) not null,
idsocio bigint not null,
ddd char(2) not null,
numero char(9),
constraint pk_telefone primary key(id),
constraint uk_telefone unique(ddd,numero),
constraint fk_telefone_socio foreign key(idsocio) references Socio(id)
);


