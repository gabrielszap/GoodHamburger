begin;

create table if not exists "order" (
    id uuid primary key,
    created_at timestamptz not null default now(),
    is_active boolean not null default true
);

create table if not exists "product" (
    id uuid primary key,
    description varchar(200) not null,
    price numeric(10,2) not null,
    type varchar(50) not null,
    is_active boolean not null default true,
    constraint ck_product_type
        check (type in ('Sanduiche', 'Acompanhamento', 'Bebida'))
);

create table if not exists order_product (
    id uuid primary key,
    order_id uuid not null,
    product_id uuid not null,
    is_active boolean not null default true,
    constraint fk_order_product_order
        foreign key (order_id) references "order"(id),
    constraint fk_order_product_product
        foreign key (product_id) references "product"(id)
);

commit;