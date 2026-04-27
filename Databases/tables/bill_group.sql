create table public.bill_groups (
  id uuid not null default gen_random_uuid (),
  name text not null,
  created_by uuid null,
  status text null default 'active'::text,
  created_at timestamp with time zone null default now(),
  constraint bill_groups_pkey primary key (id),
  constraint bill_groups_created_by_fkey foreign KEY (created_by) references users (id) on delete set null
) TABLESPACE pg_default;

create index IF not exists idx_bill_groups_created_by on public.bill_groups using btree (created_by) TABLESPACE pg_default;
