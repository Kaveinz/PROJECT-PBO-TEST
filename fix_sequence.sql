-- Fix for Duplicate Key Error in Reservations Table
-- The sequence is not synchronized with existing data

-- Connect to your database
\c "Mbok Wo Reserved";

-- Check current sequence value and max ID
SELECT 'Current sequence value:' as info, last_value, is_called FROM reservations_id_seq;
SELECT 'Max reservation ID:' as info, MAX(id) FROM reservations;

-- Fix the sequence to start from the correct value
-- Set it to the max ID + 1
SELECT setval('reservations_id_seq', (SELECT MAX(id) FROM reservations) + 1);

-- Verify the fix
SELECT 'New sequence value:' as info, last_value, is_called FROM reservations_id_seq;

-- Test insertion (this should work now)
-- INSERT INTO reservations (user_id, nomor_hp, reservation_time, jumlah_orang, table_number, status) 
-- VALUES (20, '081234567890', NOW(), 2, 'A1', 'Menunggu');
