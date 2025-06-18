using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    public interface IReservable
    {
        bool CanBeCancelled();
        bool CanBeModified();
        string GetStatusDescription();
    }

    public abstract class BaseReservation : IReservable
    {
        protected int _id;
        protected DateTime _createdAt;
        protected ReservationStatus _status;

        public int Id 
        { 
            get => _id; 
            set => _id = value >= 0 ? value : throw new ArgumentException("ID Tidak Boleh Negatif!"); 
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

        protected BaseReservation()
        {
            _createdAt = DateTime.Now;
            _status = ReservationStatus.Menunggu;
        }

        public abstract bool ValidateReservation();
    
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
                _ => "Status Tidak Dikenal/invalid"
            };
        }
    }

    public class Reservation : BaseReservation
    {
        private int _userId;
        private string _nomorHP;
        private DateTime _reservationTime;
        private int _jumlahOrang;
        private string _tableNumber;
        private string _catatan;

        public int UserId 
        { 
            get => _userId; 
            set => _userId = value > 0 ? value : throw new ArgumentException("User ID Harus Positif"); 
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
                    throw new ArgumentException("Waktu Reservasi Tidak Boleh Sebelum Sekarang");
                _reservationTime = value; 
            } 
        }

        public int JumlahOrang 
        { 
            get => _jumlahOrang; 
            set => _jumlahOrang = value > 0 ? value : throw new ArgumentException("Jumlah Orang Harus Positif"); 
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

        public Reservation() : base() { }

        public Reservation(int userId, string nomorHP, DateTime reservationTime, int jumlahOrang, string tableNumber) : base()
        {
            UserId = userId;
            NomorHP = nomorHP;
            ReservationTime = reservationTime;
            JumlahOrang = jumlahOrang;
            TableNumber = tableNumber;
        }

        public override bool ValidateReservation()
        {
            return UserId > 0 && 
                   !string.IsNullOrWhiteSpace(NomorHP) &&
                   !string.IsNullOrWhiteSpace(TableNumber) &&
                   JumlahOrang > 0 &&
                   ReservationTime > DateTime.Now;
        }
    
    }

    public enum ReservationStatus
    {
        Menunggu,
        Dikonfirmasi,
        Selesai,
        Dibatalkan
    }
}
