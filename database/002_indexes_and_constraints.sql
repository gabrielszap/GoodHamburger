begin;

create unique index if not exists ux_product_description_active
    on "product" (description)
    where is_active = true;

commit;