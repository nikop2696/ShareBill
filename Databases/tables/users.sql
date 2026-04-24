create table public.users (
  id uuid not null,
  username text null,
  is_active boolean null default false,
  created_at timestamp without time zone null default now(),
  updated_at timestamp without time zone null default now(),
  deleted_at timestamp without time zone null,
  constraint users_pkey primary key (id),
  constraint users_username_key unique (username),
  constraint users_id_fkey foreign KEY (id) references auth.users (id)
) TABLESPACE pg_default;

create trigger set_users_updated_at BEFORE
update on users for EACH row
execute FUNCTION set_updated_at ();
