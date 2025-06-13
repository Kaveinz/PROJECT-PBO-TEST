using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    // Interface demonstrating Abstraction
    public interface IReservable
    {
        bool CanBeCancelled();
        bool CanBeModified();
        string GetStatusDescription();
    }

    // Abstract base class for reservations
    public abstract class BaseReservation : IReservable
    {
        // Encapsulation: Protected fields accessible by derived classes
        protected int _id;
        protected DateTime _createdAt;
        protected ReservationStatus _status;

        public int Id 
        { 
            get => _id; 
            set => _id = value >= 0 ? value : throw new ArgumentException("ID cannot be negative"); 
        }

        public DateTime CreatedAt 
        { 
            get => _createdAt; 
            protected set => _createdAt = value; 
        }

        public ReservationStatus Status 
        { 
            get => _status; 
            set => _status = value; 
        }

        // Constructor
        protected BaseReservation()
        {
            _createdAt = DateTime.Now;
            _status = ReservationStatus.Menunggu;
        }

        // Abstract methods for derived classes to implement
        public abstract bool ValidateReservation();
        public abstract decimal CalculateCost();

        // Interface implementations with virtual methods (Polymorphism)
        public virtual bool CanBeCancelled()
        {
            return Status == ReservationStatus.Menunggu || Status == ReservationStatus.Dikonfirmasi;
        }

        public virtual bool CanBeModified()
        {
            return Status == ReservationStatus.Menunggu;
        }

        public virtual string GetStatusDescription()
        {
            return Status switch
            {
                ReservationStatus.Menunggu => "Menunggu Konfirmasi",
                ReservationStatus.Dikonfirmasi => "Reservasi Dikonfirmasi",
                ReservationStatus.Selesai => "Reservasi Selesai",
                ReservationStatus.Dibatalkan => "Reservasi Dibatalkan",
                _ => "Status Tidak Dikenal"
            };
        }
    }

    // Concrete Reservation class inheriting from BaseReservation
    public class Reservation : BaseReservation
    {
        // Encapsulation: Private fields with controlled access
        private int _userId;
        private string _nomorHP;
        private DateTime _reservationTime;
        private int _jumlahOrang;
        private string _tableNumber;
        private string _catatan;

        // Properties with validation
        public int UserId 
        { 
            get => _userId; 
            set => _userId = value > 0 ? value : throw new ArgumentException("User ID must be positive"); 
        }

        public string NomorHP 
        { 
            get => _nomorHP; 
            set => _nomorHP = value?.Trim(); 
        }

        public DateTime ReservationTime 
        { 
            get => _reservationTime; 
            set 
            { 
                if (value < DateTime.Now)
                    throw new ArgumentException("Reservation time cannot be in the past");
                _reservationTime = value; 
            } 
        }

        public int JumlahOrang 
        { 
            get => _jumlahOrang; 
            set => _jumlahOrang = value > 0 ? value : throw new ArgumentException("Number of people must be positive"); 
        }

        public string TableNumber 
        { 
            get => _tableNumber; 
            set => _tableNumber = value?.Trim(); 
        }

        public string Catatan 
        { 
            get => _catatan; 
            set => _catatan = value?.Trim(); 
        }

        // Constructors
        public Reservation() : base() { }

        public Reservation(int userId, string nomorHP, DateTime reservationTime, int jumlahOrang, string tableNumber) : base()
        {
            UserId = userId;
            NomorHP = nomorHP;
            ReservationTime = reservationTime;
            JumlahOrang = jumlahOrang;
            TableNumber = tableNumber;
        }

        // Implementation of abstract methods
        public override bool ValidateReservation()
        {
            return UserId > 0 && 
                   !string.IsNullOrWhiteSpace(NomorHP) &&
                   !string.IsNullOrWhiteSpace(TableNumber) &&
                   JumlahOrang > 0 &&
                   ReservationTime > DateTime.Now;
        }

        public override decimal CalculateCost()
        {
            // Simple cost calculation based on number of people
            decimal baseCost = 50000; // Base cost per person
            return JumlahOrang * baseCost;
        }

        // Override polymorphic methods with specific logic
        public override bool CanBeCancelled()
        {
            // Can only cancel if not yet finished and at least 2 hours before reservation time
            return base.CanBeCancelled() && ReservationTime.Subtract(DateTime.Now).TotalHours >= 2;
        }

        // Method to get reservation duration in hours
        public double GetDurationHours()
        {
            return ReservationTime.Subtract(CreatedAt).TotalHours;
        }

        // Method to check if reservation is for today
        public bool IsToday()
        {
            return ReservationTime.Date == DateTime.Today;
        }

        // Method to get formatted reservation info
        public string GetFormattedInfo()
        {
            return $"Reservasi #{Id} - {TableNumber} untuk {JumlahOrang} orang pada {ReservationTime:dd/MM/yyyy HH:mm}";
        }
    }

    // Enum for reservation status
    public enum ReservationStatus
    {
        Menunggu,
        Dikonfirmasi,
        Selesai,
        Dibatalkan
    }
}
