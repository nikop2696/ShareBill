create table public.expenses (
  id uuid not null default gen_random_uuid (),
  group_id uuid not null,
  paid_by uuid not null,
  amount numeric not null,
  currency text null default 'EUR'::text,
  split_type text null default 'equal'::text,
  description text null,
  created_at timestamp without time zone null default now(),
  updated_at timestamp without time zone null default now(),
  deleted_at timestamp without time zone null,
  constraint expenses_pkey primary key (id),
  constraint expenses_group_id_fkey foreign KEY (group_id) references bill_groups (id) on delete CASCADE,
  constraint expenses_paid_by_fkey foreign KEY (paid_by) references users (id)
) TABLESPACE pg_default;

create index IF not exists idx_expenses_group_id on public.expenses using btree (group_id) TABLESPACE pg_default;

create index IF not exists idx_expenses_paid_by on public.expenses using btree (paid_by) TABLESPACE pg_default;
