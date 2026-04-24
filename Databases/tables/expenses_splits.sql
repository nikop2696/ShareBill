create table public.expenses_splits (
  id uuid not null default gen_random_uuid (),
  expenses_id uuid null,
  user_id uuid null,
  amount numeric not null,
  created_at timestamp without time zone null default now(),
  constraint expenses_splits_pkey primary key (id),
  constraint expenses_splits_expenses_id_fkey foreign KEY (expenses_id) references expenses (id) on delete CASCADE,
  constraint expenses_splits_user_id_fkey foreign KEY (user_id) references users (id)
) TABLESPACE pg_default;

create index IF not exists idx_expenses_splits_expenses_id on public.expenses_splits using btree (expenses_id) TABLESPACE pg_default;

create index IF not exists idx_expenses_splits_user_id on public.expenses_splits using btree (user_id) TABLESPACE pg_default;
