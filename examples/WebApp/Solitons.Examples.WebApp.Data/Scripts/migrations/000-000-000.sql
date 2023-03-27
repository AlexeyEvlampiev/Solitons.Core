CREATE TABLE public.gc_object
(
	object_id uuid NOT NULL DEFAULT(gen_random_uuid()),
	created_utc timestamp NOT NULL DEFAULT now(),
	deleted_utc timestamp,
	CONSTRAINT gc_object_abstract_base  CHECK(false) NO INHERIT
);
COMMENT ON TABLE public.gc_object IS 'This table serves as an abstract base table for garbage collection objects, which can be deleted based on the deleted_utc timestamp.';
COMMENT ON COLUMN public.gc_object.object_id IS 'Primary key for the garbage collection object, stored as a UUID.';
COMMENT ON COLUMN public.gc_object.created_utc IS 'Timestamp indicating when the garbage collection object was created.';
COMMENT ON COLUMN public.gc_object.deleted_utc IS 'Timestamp indicating when the garbage collection object was deleted.';
COMMENT ON CONSTRAINT gc_object_abstract_base ON public.gc_object IS 'Check constraint that prevents data from being inserted directly into this table.';