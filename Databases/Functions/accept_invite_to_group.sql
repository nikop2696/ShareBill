CREATE OR REPLACE FUNCTION public.accept_invite_to_group()
 RETURNS trigger
 LANGUAGE plpgsql
AS $function$
begin
  if new.status = 'accepted' and old.status = 'pending' then
    insert into bill_groups_members (group_id, user_id, user_role, member_status)
    values (new.group_id, new.invited_user_id, 'member','active')
    on conflict do nothing;
  end if;

  return new;
end;
$function$
