begin;

create table if not exists order (
    id uuid primary key,
    createdAt timestamptz default now(),
    isActive boolean default true
);

create table if not exists product (
    id uuid primary key,
    description varchar(200) not null,
    price numeric(10,2) not null,
    type varchar(50) not null,
    isActive boolean default true
    constraint ck_users_status
        check (type in ('Sanduiche', 'Acompanhamento', 'Bebida'))
);

create table if not exists orderProduct (
    id uuid primary key,
    orderId uuid not null,
    productId uuid not null,
    isActive boolean default true,
);

commit;
