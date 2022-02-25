/*ThreadTypes Table*/
INSERT INTO public.thread_types (description) VALUES ('Request');
INSERT INTO public.thread_types (description) VALUES ('Recommendation');

/*ResourceTypes Table*/
INSERT INTO public.resource_types (description) VALUES ('Hyperlink');
INSERT INTO public.resource_types (description) VALUES ('Embedded Spotify');

/*Users Table*/
INSERT INTO public.users (username, email,active) VALUES ('pespinosa0', 'jerickson0@webs.com', True);
INSERT INTO public.users (username, email,active) VALUES ('anesbeth1', 'hfrancesco1@discovery.com', True);
INSERT INTO public.users (username, email,active) VALUES ('nalves2', 'cmcelvogue2@altervista.org', True);
INSERT INTO public.users (username, email,active) VALUES ('vreveland3', 'emcnea3@tripod.com', True);
INSERT INTO public.users (username, email,active) VALUES ('dtrodler41', 'ariseame@soup.io', True);
INSERT INTO public.users (username, email,active) VALUES ('hbowcher5', 'fmarcosc@chronoengine.com', True);
INSERT INTO public.users (username, email,active) VALUES ('mirdaleb', 'asifflettq@vimeo.com', True);
INSERT INTO public.users (username, email,active) VALUES ('amorewoodi', 'zarchibouldj@google.com.au', True);
INSERT INTO public.users (username, email,active) VALUES ('laltonp', 'ebootelll@1688.com', True);
INSERT INTO public.users (username, email,active) VALUES ('cmcmeylerr', 'hfrancesco1@discovery.com', True);

/*Threads Table*/
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Help!', 1, 1, '20210116 11:13:00 AM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Gym', 1, 2, '20210119 08:12:00 AM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Working', 2, 4, '20210401 09:11:00 PM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Going Camping', 2, 3, '20210304 06:00:00 PM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Holidays', 1, 5, '20210218 04:10:00 PM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Going to Gym', 1, 6, '20210720 11:15:00 PM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Bored', 2, 7, '20210914 10:20:00 AM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('On the train', 2, 9, '20211215 10:38:00 AM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('Whats Up', 1, 9, '20210125 10:42:00 PM', True);
INSERT INTO public.threads (title, thread_type_id, user_id, created_date,active) VALUES ('NVM', 1, 9, '20210721 10:54:00 AM', True);

/*Posts Table*/
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Need some more Beatles music please', 7, 1 ,'20210116 11:13:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Great beat for gym',4,2,  '20210119 08:12:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Need some music for programming please',3,3,  '20210401 09:11:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Good camping music',9,4,  '20210304 06:00:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Need music for holidays please', 3,5, '20210218 04:10:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Need music for cardio please', 6,6, '20210720 11:15:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('I like this song when I am bored',9,7,  '20210914 10:20:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Good wee song for being on the train',10,8,'20211215 10:38:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Whats up?',2,9, '20210125 10:42:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Nevermind', 1, 10, '20210721 10:54:00 AM', True);

INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Hey, I listened to this song, it sounded great!', 2, 2, '20210119 11:12:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('I love yellow submarine, check it out', 1, 1, '20210116 04:13:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Check out help, great song', 1, 1, '20210119 06:12:00 PM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('Try this some while running at gym', 4, 6, '20210119 08:48:00 AM', True);
INSERT INTO public.posts(description,user_id, thread_id, created_date,active) VALUES ('I also like this song when I am bored', 7, 7, '20210119 08:55:00 AM', True);

/*PostResources Table*/
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (1,1,'Hey Jude', 'youtube.com/heyjude');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (2,1, 'Tiesto', 'youtube.com/adagioforstrings');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (4,2, 'All These Things Ive Done', 'iframecode01');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (5,1, 'Fat Lip', 'youtube.com/fatlip');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (6,2, 'I will wait', 'iframecode02');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (7,2, 'Ho Hey', 'iframecode03');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (8,2, 'Hotline Bling', 'iframecode04');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (11,2, 'Feeling This', 'iframecode05');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (12,1, 'Yellow', 'youtube.com/yellow');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (13,2, 'You Make My Dreams(Come True)', 'iframecode06');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (14,1, 'In The Air Tonight', 'youtube.com/intheairtonight');
INSERT INTO public.post_resources (post_id, resource_type_id, title, value) values (15,2, 'My Sharona', 'iframecode07');

