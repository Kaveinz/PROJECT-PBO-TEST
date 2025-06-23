using System;

namespace test_part_kesekian.models
{
    public class Table
    {
        public string? TableNumber { get; set; }
        public int Capacity
        {
            get => _capacity;
            set => _capacity = value > 0 ? value : throw new ArgumentException("Kapasitas harus lebih besar dari 0");
        }
        public TableStatus Status { get; set; }

        private int _capacity;

        public Table()
        {
            TableNumber = "";
            Capacity = 1;
            Status = TableStatus.Available;
        
        }

        public Table(string tableNumber, int capacity)
        {
            TableNumber = tableNumber?.Trim().ToUpper();
            Capacity = capacity;
            Status = TableStatus.Available;
        }
    }

    public enum TableStatus
    {
        Available,
        Reserved,
    }
}
