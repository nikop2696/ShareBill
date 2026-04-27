create table public.settlements (
  id uuid not null default gen_random_uuid (),
  group_id uuid null,
  from_user uuid null,
  to_user uuid null,
  amount numeric not null,
  created_at timestamp without time zone null default now(),
  constraint settlements_pkey primary key (id),
  constraint settlements_from_user_fkey foreign KEY (from_user) references users (id),
  constraint settlements_group_id_fkey foreign KEY (group_id) references bill_groups (id) on delete CASCADE,
  constraint settlements_to_user_fkey foreign KEY (to_user) references users (id)
) TABLESPACE pg_default;

create index IF not exists idx_settlements_groups_id on public.settlements using btree (group_id) TABLESPACE pg_default;

create index IF not exists idx_settlements_from_user on public.settlements using btree (from_user) TABLESPACE pg_default;

create index IF not exists idx_settlements_to_user on public.settlements using btree (to_user) TABLESPACE pg_default;
