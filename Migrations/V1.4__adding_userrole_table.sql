CREATE TABLE IF NOT EXISTS public.user_roles
(
    id serial constraint user_role_pk primary key,
    description text
);

ALTER TABLE public.users add column role_id INTEGER;
ALTER TABLE public.users
    add constraint role_fk
    foreign key (role_id)
    references public.user_roles(id);