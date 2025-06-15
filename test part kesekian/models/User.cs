using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    public abstract class Person
    {
        private string _namaLengkap;
        private string _email;
        private string _nomorHp;

        public string NamaLengkap 
        { 
            get => _namaLengkap; 
            set => _namaLengkap = value?.Trim(); 
        }
        
        public string Email 
        { 
            get => _email; 
            set => _email = value?.Trim().ToLower(); 
        }
        
        public string NomorHp 
        { 
            get => _nomorHp; 
            set => _nomorHp = value?.Trim(); 
        }

        public abstract string GetDisplayInfo();

        public virtual bool ValidateData()
        {
            return !string.IsNullOrWhiteSpace(NamaLengkap) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(NomorHp);
        }
    }

    public class User : Person
    {
        private int _id;
        private string _username;
        private string _password;
        private UserRole _role;
        private DateTime _createdAt;
        private UserStatus _status;

        public int Id 
        { 
            get => _id; 
            set => _id = value >= 0 ? value : throw new ArgumentException("ID Tidak Boleh Negatif!"); 
        }

        public string Username 
        { 
            get => _username; 
            set => _username = value?.Trim().ToLower(); 
        }

        public string Password 
        { 
            get => _password; 
            set => _password = value; 
        }

        public UserRole Role 
        { 
            get => _role; 
            set => _role = value; 
        }

        public DateTime CreatedAt 
        { 
            get => _createdAt; 
            set => _createdAt = value; 
        }

        public UserStatus Status 
        { 
            get => _status; 
            set => _status = value; 
        }

        public User()
        {
            _createdAt = DateTime.Now;
            _status = UserStatus.Aktif;
            _role = UserRole.Pengguna;
        }

        public User(string username, string password, string namaLengkap, string email, string nomorHp) : this()
        {
            Username = username;
            Password = password;
            NamaLengkap = namaLengkap;
            Email = email;
            NomorHp = nomorHp;
        }

        public override string GetDisplayInfo()
        {
            return $"{NamaLengkap} ({Username}) - {Role}";
        }

        public override bool ValidateData()
        {
            return base.ValidateData() && 
                   !string.IsNullOrWhiteSpace(Username) && 
                   !string.IsNullOrWhiteSpace(Password) &&
                   Username.Length >= 3 &&
                   Password.Length >= 6;
        }

        public bool IsAdmin()
        {
            return Role == UserRole.Admin;
        }

        public bool CanMakeReservation()
        {
            return Status == UserStatus.Aktif && (Role == UserRole.Pengguna || Role == UserRole.Admin);
        }
    }

    public enum UserRole
    {
        Pengguna,
        Admin
    }

    public enum UserStatus
    {
        Aktif,
        Nonaktif,
        Suspended
    }
}
