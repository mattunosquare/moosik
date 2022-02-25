CREATE TABLE IF NOT EXISTS public.thread_types
(
    id serial constraint thread_type_pk primary key,
    description text
);

CREATE TABLE IF NOT EXISTS public.resource_types
(
    id  serial constraint resource_type_pk primary key,
    description text
);

CREATE TABLE IF NOT EXISTS public.users
(
    id  serial constraint user_pk primary key,
    username text,
    email text,
    active boolean
);

CREATE TABLE IF NOT EXISTS public.threads
(
    id  serial constraint thread_pk primary key,
    title text,
    created_date timestamp,
    active boolean
);

ALTER TABLE public.threads ADD COLUMN thread_type_id INTEGER;
ALTER TABLE public.threads
    ADD CONSTRAINT thread_type_fk
    FOREIGN KEY (thread_type_id)
    REFERENCES public.thread_types(id);

ALTER TABLE public.threads add column user_id INTEGER;
ALTER TABLE public.threads
    add constraint user_fk
    foreign key (user_id)
    references public.users(id);

CREATE TABLE IF NOT EXISTS public.posts
(
    id serial constraint post_pk primary key,
    description text,
    created_date timestamp,
    active boolean
);

ALTER TABLE public.posts add column user_id INTEGER;
alter Table public.posts
    add constraint user_fk
    foreign key (user_id)
    references public.users(id);

alter table public.posts add column thread_id INTEGER;
alter table public.posts
    add constraint thread_fk
    foreign key (thread_id)
    references public.threads(id);



CREATE TABLE IF NOT EXISTS public.post_resources
(
    id serial constraint post_resources_pk primary key,
    title text,
    value text
);

alter table public.post_resources add column post_id INTEGER;
alter table public.post_resources
    add constraint post_fk
    foreign key (post_id)
    references public.posts(id);

alter table public.post_resources add column resource_type_id INTEGER;
alter table public.post_resources
    add constraint resource_type_fk
    foreign key (resource_type_id)
    references public.resource_types(id);    

