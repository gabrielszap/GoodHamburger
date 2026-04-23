begin;

create unique index if not exists ux_product_description
    on "product" (description);

commit;