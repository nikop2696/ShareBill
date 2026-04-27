create table public.bill_groups_members (
  group_id uuid not null,
  user_id uuid not null,
  user_role text null default 'member'::text,
  member_status text null default 'active'::text,
  joined_at timestamp without time zone null default now(),
  updated_at timestamp without time zone null default now(),
  constraint bill_groups_members_pkey primary key (group_id, user_id),
  constraint bill_groups_members_group_id_fkey foreign KEY (group_id) references bill_groups (id) on delete CASCADE,
  constraint bill_groups_members_user_id_fkey foreign KEY (user_id) references users (id)
) TABLESPACE pg_default;

create index IF not exists idx_bill_group_members_group_id on public.bill_groups_members using btree (group_id) TABLESPACE pg_default;

create index IF not exists idx_bill_group_members_user_id on public.bill_groups_members using btree (user_id) TABLESPACE pg_default;

create trigger trg_group_members_updated_at BEFORE
update on bill_groups_members for EACH row
execute FUNCTION set_updated_at ();
