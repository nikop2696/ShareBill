create table public.bill_groups_invites (
  id uuid not null default gen_random_uuid (),
  group_id uuid not null,
  invited_user_id uuid not null,
  invited_by_id uuid not null,
  status text not null default 'pending'::text,
  created_at timestamp without time zone null default now(),
  updated_at timestamp without time zone null default now(),
  constraint bill_groups_invites_pkey primary key (id),
  constraint bill_groups_invites_group_id_fkey foreign KEY (group_id) references bill_groups (id) on delete CASCADE,
  constraint bill_groups_invites_invited_by_id_fkey foreign KEY (invited_by_id) references users (id),
  constraint bill_groups_invites_invited_user_id_fkey foreign KEY (invited_user_id) references users (id),
  constraint bill_groups_invites_status_check check (
    (
      status = any (
        array[
          'pending'::text,
          'accepted'::text,
          'rejected'::text
        ]
      )
    )
  ),
  constraint no_self_invites check ((invited_user_id <> invited_by_id))
) TABLESPACE pg_default;

create unique INDEX IF not exists bill_groups_invites_unique_invites on public.bill_groups_invites using btree (group_id, invited_user_id) TABLESPACE pg_default
where
  (status = 'pending'::text);

create index IF not exists bill_groups_invites_invited_user_idx on public.bill_groups_invites using btree (invited_user_id) TABLESPACE pg_default;

create index IF not exists bill_groups_invites_invited_by on public.bill_groups_invites using btree (invited_by_id) TABLESPACE pg_default;

create trigger trg_bill_groups_invites_updated_at BEFORE
update on bill_groups_invites for EACH row
execute FUNCTION set_updated_at ();

create trigger trg_invite_accept
after
update on bill_groups_invites for EACH row
execute FUNCTION accept_invite_to_group ();
