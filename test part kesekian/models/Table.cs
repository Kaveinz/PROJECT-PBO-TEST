using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    // Interface for table operations (Abstraction)
    public interface ITableOperations
    {
        bool IsAvailable();
        bool CanAccommodate(int numberOfPeople);
        void Reserve();
        void Release();
    }

    // Abstract base class demonstrating Abstraction and Inheritance
    public abstract class BaseTable : ITableOperations
    {
        // Encapsulation: Protected fields
        protected string _tableNumber;
        protected int _capacity;
        protected TableStatus _status;
        protected TableType _tableType;

        // Properties with validation
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

        // Constructor
        protected BaseTable(string tableNumber, int capacity, TableType tableType)
        {
            TableNumber = tableNumber;
            Capacity = capacity;
            TableType = tableType;
            Status = TableStatus.Available;
        }

        // Abstract method for derived classes
        public abstract decimal GetHourlyRate();

        // Interface implementation with virtual methods (Polymorphism)
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

        // Method to get table description
        public virtual string GetDescription()
        {
            return $"Table {TableNumber} - {TableType} (Capacity: {Capacity})";
        }
    }

    // Concrete Table class inheriting from BaseTable
    public class Table : BaseTable
    {
        // Additional properties specific to regular tables
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

        // Constructors
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

        // Implementation of abstract method with specific pricing logic (Polymorphism)
        public override decimal GetHourlyRate()
        {
            decimal baseRate = TableType switch
            {
                TableType.Regular => 25000,
                TableType.VIP => 50000,
                TableType.Private => 75000,
                _ => 25000
            };

            // Add premium for tables with view
            if (HasView)
                baseRate += 10000;

            return baseRate;
        }

        // Override methods with specific logic (Polymorphism)
        public override bool CanAccommodate(int numberOfPeople)
        {
            // Additional check for VIP tables
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

        // Method to check if table needs cleaning
        public bool NeedsCleaning()
        {
            return DateTime.Now.Subtract(LastCleanedAt).TotalHours >= 4;
        }

        // Method to mark table as cleaned
        public void MarkAsCleaned()
        {
            LastCleanedAt = DateTime.Now;
        }

        // Method to calculate total cost for duration
        public decimal CalculateCost(TimeSpan duration)
        {
            double hours = Math.Ceiling(duration.TotalHours);
            return (decimal)hours * GetHourlyRate();
        }

        // Static method to create table based on capacity
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

    // Enums for better type safety
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
