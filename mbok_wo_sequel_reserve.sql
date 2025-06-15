--
-- PostgreSQL database dump
--

-- Dumped from database version 17.4
-- Dumped by pg_dump version 17.4

-- Started on 2025-06-14 01:33:25

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 2 (class 3079 OID 25247)
-- Name: pgcrypto; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pgcrypto WITH SCHEMA public;


--
-- TOC entry 4914 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pgcrypto; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgcrypto IS 'cryptographic functions';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 222 (class 1259 OID 25303)
-- Name: reservations; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.reservations (
    id integer NOT NULL,
    user_id integer NOT NULL,
    nomor_hp character varying(20) NOT NULL,
    reservation_time timestamp without time zone NOT NULL,
    jumlah_orang integer NOT NULL,
    table_number character varying(20) NOT NULL,
    status character varying(20) DEFAULT 'Menunggu'::character varying NOT NULL
);


ALTER TABLE public.reservations OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 25302)
-- Name: reservations_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.reservations_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.reservations_id_seq OWNER TO postgres;

--
-- TOC entry 4915 (class 0 OID 0)
-- Dependencies: 221
-- Name: reservations_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.reservations_id_seq OWNED BY public.reservations.id;


--
-- TOC entry 220 (class 1259 OID 25296)
-- Name: tables; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tables (
    table_number character varying(20) NOT NULL,
    capacity integer NOT NULL,
    status character varying(20) DEFAULT 'Available'::character varying NOT NULL
);


ALTER TABLE public.tables OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 25285)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(50) NOT NULL,
    password_hash text NOT NULL,
    role character varying(20) NOT NULL,
    nama_lengkap character varying(100) DEFAULT 'User'::character varying,
    email character varying(100),
    nomor_hp character varying(20),
    alamat text,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    status character varying(20) DEFAULT 'Aktif'::character varying,
    CONSTRAINT users_role_check CHECK (((role)::text = ANY ((ARRAY['pengguna'::character varying, 'admin'::character varying, 'superadmin'::character varying])::text[])))
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 25284)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 4916 (class 0 OID 0)
-- Dependencies: 218
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4746 (class 2604 OID 25306)
-- Name: reservations id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reservations ALTER COLUMN id SET DEFAULT nextval('public.reservations_id_seq'::regclass);


--
-- TOC entry 4741 (class 2604 OID 25288)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 4908 (class 0 OID 25303)
-- Dependencies: 222
-- Data for Name: reservations; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.reservations (id, user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status) FROM stdin;
1	20	081211112222	2025-01-02 19:00:00	3	A1	Menunggu
2	20	081211112223	2025-01-04 13:30:00	2	A3	Selesai
3	20	081211112224	2025-01-07 20:00:00	4	A5	Dibatalkan
4	20	081211112225	2025-01-10 18:00:00	1	A7	Selesai
5	20	081211112226	2025-01-14 12:00:00	2	A2	Menunggu
6	20	081211112227	2025-01-18 19:30:00	5	A4	Selesai
7	20	081211112228	2025-01-21 16:00:00	3	A6	Menunggu
8	20	081211112229	2025-01-25 14:00:00	2	A1	Dibatalkan
9	20	081211112230	2025-01-28 21:00:00	4	A3	Selesai
10	20	081211112231	2025-02-01 11:00:00	1	A5	Menunggu
11	20	081211112232	2025-02-04 18:30:00	3	A7	Menunggu
12	20	081211112233	2025-02-08 17:00:00	2	A2	Selesai
13	20	081211112234	2025-02-11 19:00:00	5	A4	Menunggu
14	20	081211112235	2025-02-15 13:00:00	2	A6	Dibatalkan
15	20	081211112236	2025-02-18 20:00:00	3	A1	Selesai
16	20	081211112237	2025-02-22 10:00:00	4	A3	Selesai
17	20	081211112238	2025-02-25 15:00:00	1	A5	Menunggu
18	20	081211112239	2025-03-01 16:30:00	2	A7	Selesai
19	20	081211112240	2025-03-04 19:30:00	3	A2	Menunggu
20	20	081211112241	2025-03-08 11:30:00	5	A4	Dibatalkan
21	20	081211112242	2025-03-11 14:00:00	2	A6	Selesai
22	20	081211112243	2025-03-15 18:00:00	4	A1	Selesai
23	20	081211112244	2025-03-18 20:30:00	3	A3	Menunggu
24	20	081211112245	2025-03-22 13:00:00	1	A5	Selesai
25	20	081211112246	2025-03-25 17:00:00	2	A7	Menunggu
26	20	081211112247	2025-03-29 19:00:00	5	A2	Dibatalkan
27	20	081211112248	2025-04-01 12:30:00	3	A4	Selesai
28	20	081211112249	2025-04-05 14:00:00	2	A6	Selesai
29	20	081211112250	2025-04-08 18:00:00	4	A1	Menunggu
30	20	081211112251	2025-04-12 20:00:00	1	A3	Selesai
31	20	081211112252	2025-04-15 11:00:00	3	A5	Menunggu
32	20	081211112253	2025-04-19 15:30:00	2	A7	Dibatalkan
33	20	081211112254	2025-04-22 19:00:00	5	A2	Selesai
34	20	081211112255	2025-04-26 12:00:00	4	A4	Selesai
35	20	081211112256	2025-04-29 16:00:00	3	A6	Menunggu
36	20	081211112257	2025-05-03 18:30:00	2	A1	Selesai
37	20	081211112258	2025-05-06 20:00:00	1	A3	Menunggu
38	20	081211112259	2025-05-10 13:00:00	5	A5	Dibatalkan
39	20	081211112260	2025-05-13 14:00:00	3	A7	Selesai
40	20	081211112261	2025-05-17 10:00:00	2	A2	Selesai
41	20	081211112262	2025-05-20 17:00:00	4	A4	Menunggu
42	20	081211112263	2025-05-24 19:30:00	1	A6	Selesai
43	20	081211112264	2025-05-27 12:00:00	3	A1	Menunggu
44	20	081211112265	2025-05-31 16:00:00	2	A3	Dibatalkan
45	20	081211112266	2025-06-03 18:00:00	5	A5	Selesai
46	20	081211112267	2025-06-07 20:00:00	4	A7	Selesai
47	20	081211112268	2025-06-10 11:30:00	3	A2	Menunggu
48	20	081211112269	2025-06-12 14:00:00	2	A4	Selesai
49	20	081211112270	2025-06-17 15:00:00	1	A6	Dibatalkan
50	20	081211112271	2025-06-21 17:30:00	5	A1	Selesai
51	20	081211112272	2025-06-24 19:00:00	3	A3	Menunggu
52	20	081211112273	2025-06-28 12:00:00	2	A5	Selesai
53	20	081211112274	2025-07-01 20:00:00	4	A7	Menunggu
54	20	081211112275	2025-07-05 13:30:00	1	A2	Dibatalkan
55	20	081211112276	2025-07-08 16:00:00	3	A4	Selesai
56	20	081211112277	2025-07-12 18:00:00	2	A6	Selesai
57	20	081211112278	2025-07-15 19:30:00	5	A1	Menunggu
58	20	081211112279	2025-07-19 11:00:00	4	A3	Selesai
59	20	081211112280	2025-07-22 14:00:00	3	A5	Menunggu
60	20	081211112281	2025-07-26 17:00:00	2	A7	Dibatalkan
61	20	081211112282	2025-07-29 18:30:00	1	A2	Selesai
62	20	081211112283	2025-08-02 20:00:00	5	A4	Selesai
63	20	081211112284	2025-08-05 12:00:00	3	A6	Menunggu
64	20	081211112285	2025-08-09 15:00:00	2	A1	Selesai
65	20	081211112286	2025-08-12 17:00:00	4	A3	Dibatalkan
66	20	081211112287	2025-08-16 19:00:00	1	A5	Selesai
67	20	081211112288	2025-08-19 20:30:00	3	A7	Menunggu
68	20	081211112289	2025-08-23 13:00:00	2	A2	Selesai
69	20	081211112290	2025-08-26 10:00:00	5	A4	Menunggu
70	20	081211112291	2025-08-30 16:30:00	4	A6	Dibatalkan
71	20	081211112292	2025-09-02 18:00:00	3	A1	Selesai
72	20	081211112293	2025-09-06 19:30:00	2	A3	Selesai
73	20	081211112294	2025-09-09 11:00:00	1	A5	Menunggu
74	20	081211112295	2025-09-13 14:00:00	5	A7	Selesai
75	20	081211112296	2025-09-16 15:00:00	3	A2	Dibatalkan
76	20	081211112297	2025-09-20 17:00:00	2	A4	Selesai
77	20	081211112298	2025-09-23 18:30:00	4	A6	Menunggu
78	20	081211112299	2025-09-27 20:00:00	1	A1	Selesai
79	20	081211112300	2025-09-30 12:00:00	3	A3	Menunggu
80	20	081211112301	2025-10-04 15:00:00	2	A5	Dibatalkan
81	20	081211112302	2025-10-07 17:00:00	5	A7	Selesai
82	20	081211112303	2025-10-11 19:30:00	4	A2	Selesai
83	20	081211112304	2025-10-14 11:00:00	3	A4	Menunggu
84	20	081211112305	2025-10-18 14:00:00	2	A6	Selesai
85	20	081211112306	2025-10-21 15:00:00	1	A1	Dibatalkan
86	20	081211112307	2025-10-25 17:30:00	5	A3	Selesai
87	20	081211112308	2025-10-28 19:00:00	3	A5	Menunggu
88	20	081211112309	2025-11-01 12:00:00	2	A7	Selesai
89	20	081211112310	2025-11-04 20:00:00	4	A2	Menunggu
90	20	081211112311	2025-11-08 13:30:00	1	A4	Dibatalkan
91	20	081211112312	2025-11-11 16:00:00	3	A6	Selesai
92	20	081211112313	2025-11-15 18:00:00	2	A1	Selesai
93	20	081211112314	2025-11-18 19:30:00	5	A3	Menunggu
94	20	081211112315	2025-11-22 11:00:00	4	A5	Selesai
95	20	081211112316	2025-11-25 14:00:00	3	A7	Menunggu
96	20	081211112317	2025-11-29 17:00:00	2	A2	Dibatalkan
97	20	081211112318	2025-12-02 18:30:00	1	A4	Selesai
98	20	081211112319	2025-12-06 20:00:00	5	A6	Selesai
99	20	081211112320	2025-12-09 12:00:00	3	A1	Menunggu
100	20	081211112321	2025-12-13 15:00:00	2	A3	Selesai
101	20	081211112322	2025-12-16 17:00:00	4	A5	Dibatalkan
102	20	081211112323	2025-12-20 19:00:00	1	A7	Selesai
103	20	081211112324	2025-12-23 20:30:00	3	A2	Menunggu
104	20	081211112325	2025-12-27 13:00:00	2	A4	Selesai
\.


--
-- TOC entry 4906 (class 0 OID 25296)
-- Dependencies: 220
-- Data for Name: tables; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tables (table_number, capacity, status) FROM stdin;
A5	6	Available
A6	6	Available
A7	8	Available
A4	4	Available
A2	4	Available
A1	4	Reserved
A3	4	Reserved
\.


--
-- TOC entry 4905 (class 0 OID 25285)
-- Dependencies: 219
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, password_hash, role, nama_lengkap, email, nomor_hp, alamat, created_at, status) FROM stdin;
19	admin	$2a$06$dpNU7X5L14jNNgMh35c7rOsyaxfWu1hW.tb6zJhq063tT4cLEnaIa	admin	Admin Gampang	admin@mail.com	0811111111	\N	2025-06-13 01:23:45.910407	Aktif
20	user	$2a$06$dH0rVJ2LRfLgJWjRzyS.G.cBUoqXgV115ZP2FnFkl1KQJj4UDeZoS	pengguna	User Biasa	user@mail.com	0822222222	\N	2025-06-13 01:23:45.910407	Aktif
21	superadmin	$2a$06$4EMsRfCdDZN/PxZZp/qNQuc4ukleHms7hLhlb5BjAqsTxvMWZIvFi	superadmin	Super Admin	super@admin.com	081234567890	\N	2025-06-13 01:29:40.566828	Aktif
22	sefa	$2a$06$KPl/saZgUOmKXgGxZgiyV.z1tiE1ArFwWNl.ok/oUydpakEoOIt7e	pengguna	sefa yoga ganteng	sefaganteng99@gmail.com	09032832098	\N	2025-06-13 01:57:14.934213	Aktif
23	egy	$2a$06$xjyv/KZe8p1oaBILHsGYpuOu2QXrglIbgPSAAosc125LhUoTgjtx6	pengguna	M egy	Megy@gmail.com	09834028394	\N	2025-06-13 02:04:29.878166	Aktif
24	adit	$2a$06$bSob6/Db05iEDJby9th.nu6gcGgHYqslqH4c6URBUdVp6RzELwf.O	pengguna	aditya dwi	adit@gmail.com	3098402	\N	2025-06-13 02:06:32.550831	Aktif
\.


--
-- TOC entry 4917 (class 0 OID 0)
-- Dependencies: 221
-- Name: reservations_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.reservations_id_seq', 1, false);


--
-- TOC entry 4918 (class 0 OID 0)
-- Dependencies: 218
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 24, true);


--
-- TOC entry 4756 (class 2606 OID 25309)
-- Name: reservations reservations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reservations
    ADD CONSTRAINT reservations_pkey PRIMARY KEY (id);


--
-- TOC entry 4754 (class 2606 OID 25301)
-- Name: tables tables_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tables
    ADD CONSTRAINT tables_pkey PRIMARY KEY (table_number);


--
-- TOC entry 4750 (class 2606 OID 25293)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4752 (class 2606 OID 25295)
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- TOC entry 4757 (class 2606 OID 25315)
-- Name: reservations reservations_table_number_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reservations
    ADD CONSTRAINT reservations_table_number_fkey FOREIGN KEY (table_number) REFERENCES public.tables(table_number);


--
-- TOC entry 4758 (class 2606 OID 25310)
-- Name: reservations reservations_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.reservations
    ADD CONSTRAINT reservations_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id);


-- Completed on 2025-06-14 01:33:25

--
-- PostgreSQL database dump complete
--

