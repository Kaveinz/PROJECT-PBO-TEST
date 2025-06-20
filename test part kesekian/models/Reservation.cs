using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    public interface IReservable
    {
       
        string GetDeskripsiStatus();
        bool ValidateReservasi();
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

        public abstract bool ValidateReservasi();
    
       

        public virtual string GetDeskripsiStatus()
        {
            return Status switch
            {
                ReservationStatus.Menunggu => "Menunggu reservasi terlaksana",
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
                    throw new ArgumentException("Waktu Reservasi Tidak Boleh Di masa Lalu");
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


        public Reservation() : base() { }

        public Reservation(int userId, string nomorHP, DateTime reservationTime, int jumlahOrang, string tableNumber) : base()
        {
            UserId = userId;
            NomorHP = nomorHP;
            ReservationTime = reservationTime;
            JumlahOrang = jumlahOrang;
            TableNumber = tableNumber;
        }

        public override bool ValidateReservasi()
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
        Selesai,
        Dibatalkan,
        Tidak_Datang
    }
}
