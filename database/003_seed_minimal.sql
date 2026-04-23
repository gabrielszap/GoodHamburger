begin;

insert into "product" (
    id,
    description,
    type,
    price
)
values (
    'e6405b80-bddc-4cc2-b4f0-2fb2acd596c7',
    'X Burger',
    'Sanduiche',
    5.00
)
on conflict (id) do nothing;

insert into "product" (
    id,
    description,
    type,
    price
)
values (
    'cc1327c9-4256-4398-954b-71031131193e',
    'X Egg',
    'Sanduiche',
    4.50
)
on conflict (id) do nothing;

insert into "product" (
    id,
    description,
    type,
    price
)
values (
    'e9daa86f-2ae4-48ff-b791-f7a054e8e242',
    'X Bacon',
    'Sanduiche',
    7.00
)
on conflict (id) do nothing;

insert into "product" (
    id,
    description,
    type,
    price
)
values (
    'c0b8c83c-9e8e-4504-a85e-064ee1f90b71',
    'Batata Frita',
    'Acompanhamento',
    2.00
)
on conflict (id) do nothing;

insert into "product" (
    id,
    description,
    type,
    price
)
values (
    '87bf672e-a750-41c9-9dfa-38f07e9e30bf',
    'Refrigerante',
    'Bebida',
    2.50
)
on conflict (id) do nothing;

commit;
