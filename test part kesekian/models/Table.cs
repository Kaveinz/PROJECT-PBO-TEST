namespace test_part_kesekian.models
{
    public interface ITableOperations
    {
        bool IsAvailable();
        bool CanAccommodate(int numberOfPeople);
        void Reserve();
        void Release();
    }

    public abstract class BaseTable : ITableOperations
    {
        protected string? _tableNumber;
        protected int _capacity;
        protected TableStatus _status;

        public string? TableNumber 
        { 
            get => _tableNumber; 
            set => _tableNumber = value?.Trim().ToUpper(); 
        }

        public int Capacity 
        { 
            get => _capacity; 
            set => _capacity = value > 0 ? value : throw new ArgumentException("Kapasitas harus lebih besar dari 0"); 
        }

        public TableStatus Status 
        { 
            get => _status; 
            set => _status = value; 
        }

        protected BaseTable(string tableNumber, int capacity)
        {
            TableNumber = tableNumber;
            Capacity = capacity;
            Status = TableStatus.Available;
        }

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
                throw new InvalidOperationException($"Meja {TableNumber} sudah terpakai. ");
        }        public virtual void Release()
        {
            if (Status == TableStatus.Reserved || Status == TableStatus.Occupied)
                Status = TableStatus.Available;
        }

        public virtual string GetDescription()
        {
            return $"Meja {TableNumber} Kapasitas: {Capacity})";        }
    }

    public class Table : BaseTable
    {
        private DateTime _lastCleanedAt;

        public DateTime LastCleanedAt 
        { 
            get => _lastCleanedAt; 
            set => _lastCleanedAt = value; 
        }

        public Table() : base("", 1) 
        {
            _lastCleanedAt = DateTime.Now;
        }

        public Table(string tableNumber, int capacity) 
            : base(tableNumber, capacity)
        {
            _lastCleanedAt = DateTime.Now;
        }        public bool NeedsCleaning()
        {
            return DateTime.Now.Subtract(LastCleanedAt).TotalHours >= 4;
        }

        public void MarkAsCleaned()
        {
            LastCleanedAt = DateTime.Now;
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
}
