/*==============================================================*/
/* Table: greeny.category                                       */
/*==============================================================*/
create table greeny.category (
	id serial4 not null,
	"name" varchar null,
	constraint category_pkey primary key (id)
);

/*==============================================================*/
/* Table: greeny.user                                           */
/*==============================================================*/
create table greeny."user" (
	id serial4 not null,
	"type" int4 not null default 0,
	phone_number varchar null,
	email varchar null,
	first_name varchar null,
	middle_name varchar null,
	last_name varchar null,
	"password" varchar null,
	register_time timestamp not null,
	constraint user_pkey primary key (id)
);



