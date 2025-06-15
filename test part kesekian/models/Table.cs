using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    // Antarmuka untuk operasi tabel (Abstraksi)
    public interface ITableOperations
    {
        bool IsAvailable();
        bool CanAccommodate(int numberOfPeople);
        void Reserve();
        void Release();
    }

    public abstract class BaseTable : ITableOperations
    {
        // Enkapsulasi: Field yang dilindungi
        protected string _tableNumber;
        protected int _capacity;
        protected TableStatus _status;
        protected TableType _tableType;

        // Properti dengan validasi
        public string TableNumber 
        { 
            get => _tableNumber; 
            set => _tableNumber = value?.Trim().ToUpper(); 
        }

        public int Capacity 
        { 
            get => _capacity; 
            set => _capacity = value > 0 ? value : throw new ArgumentException("Capacity must be positive"); 
        }

        public TableStatus Status 
        { 
            get => _status; 
            set => _status = value; 
        }

        public TableType TableType 
        { 
            get => _tableType; 
            set => _tableType = value; 
        }

        protected BaseTable(string tableNumber, int capacity, TableType tableType)
        {
            TableNumber = tableNumber;
            Capacity = capacity;
            TableType = tableType;
            Status = TableStatus.Available;
        }

        public abstract decimal GetHourlyRate();

        public virtual bool IsAvailable()
        {
            return Status == TableStatus.Available;
        }

        public virtual bool CanAccommodate(int numberOfPeople)
        {
            return numberOfPeople > 0 && numberOfPeople <= Capacity && IsAvailable();
        }

        public virtual void Reserve()
        {
            if (IsAvailable())
                Status = TableStatus.Reserved;
            else
                throw new InvalidOperationException($"Table {TableNumber} is not available for reservation");
        }

        public virtual void Release()
        {
            if (Status == TableStatus.Reserved || Status == TableStatus.Occupied)
                Status = TableStatus.Available;
        }

        // Metode untuk mendapatkan deskripsi tabel
        public virtual string GetDescription()
        {
            return $"Table {TableNumber} - {TableType} (Capacity: {Capacity})";
        }
    }

    // Kelas Tabel konkret yang mewarisi dari BaseTable
    public class Table : BaseTable
    {
        private string _location;
        private bool _hasView;
        private DateTime _lastCleanedAt;

        public string Location 
        { 
            get => _location; 
            set => _location = value?.Trim(); 
        }

        public bool HasView 
        { 
            get => _hasView; 
            set => _hasView = value; 
        }

        public DateTime LastCleanedAt 
        { 
            get => _lastCleanedAt; 
            set => _lastCleanedAt = value; 
        }

        public Table() : base("", 1, TableType.Regular) 
        {
            _lastCleanedAt = DateTime.Now;
        }

        public Table(string tableNumber, int capacity, TableType tableType = TableType.Regular) 
            : base(tableNumber, capacity, tableType)
        {
            _lastCleanedAt = DateTime.Now;
        }

        public Table(string tableNumber, int capacity, TableType tableType, string location, bool hasView) 
            : this(tableNumber, capacity, tableType)
        {
            Location = location;
            HasView = hasView;
        }

        // Implementasi metode abstrak dengan logika harga spesifik (Polimorfisme)
        public override decimal GetHourlyRate()
        {
            decimal baseRate = TableType switch
            {
                TableType.Regular => 25000,
                TableType.VIP => 50000,
                TableType.Private => 75000,
                _ => 25000
            };

            // Tambahkan premium untuk meja dengan pemandangan
            if (HasView)
                baseRate += 10000;

            return baseRate;
        }

        public override bool CanAccommodate(int numberOfPeople)
        {
            // Pemeriksaan tambahan untuk meja VIP
            if (TableType == TableType.VIP && numberOfPeople < 2)
                return false;

            return base.CanAccommodate(numberOfPeople);
        }

        public override string GetDescription()
        {
            string description = base.GetDescription();
            
            if (!string.IsNullOrEmpty(Location))
                description += $" - {Location}";
                
            if (HasView)
                description += " (With View)";

            return description;
        }

        // Untuk memeriksa meja apakah perlu dibersihkan
        public bool NeedsCleaning()
        {
            return DateTime.Now.Subtract(LastCleanedAt).TotalHours >= 4;
        }

        // Tandai Meja
        public void MarkAsCleaned()
        {
            LastCleanedAt = DateTime.Now;
        }

        // Total Biaya berdasar Durasi
        public decimal CalculateCost(TimeSpan duration)
        {
            double hours = Math.Ceiling(duration.TotalHours);
            return (decimal)hours * GetHourlyRate();
        }

        // Objek Tabel
        public static Table CreateTable(string tableNumber, int capacity)
        {
            TableType type = capacity switch
            {
                <= 2 => TableType.Regular,
                <= 4 => TableType.Regular,
                <= 6 => TableType.VIP,
                _ => TableType.Private
            };

            return new Table(tableNumber, capacity, type);
        }
    }

    public enum TableStatus
    {
        Available,
        Reserved,
        Occupied,
        OutOfOrder,
        Cleaning
    }

    public enum TableType
    {
        Regular,
        VIP,
        Private
    }
}
