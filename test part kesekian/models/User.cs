using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_part_kesekian.models
{
    public abstract class Person
    {
        private string namaLengkap;
        private string email;
        private string nomorHp;

        public string NamaLengkap 
        { 
            get => namaLengkap; 
            set => namaLengkap= value?.Trim(); 
        }
        
        public string Email 
        { 
            get => email; 
            set => email = value?.Trim().ToLower(); 
        }
        
        public string NomorHp 
        { 
            get => nomorHp; 
            set => nomorHp = value?.Trim(); 
        }

        

        public virtual bool ValidateData()
        {
            return !string.IsNullOrWhiteSpace(NamaLengkap) && 
                   !string.IsNullOrWhiteSpace(Email) && 
                   !string.IsNullOrWhiteSpace(NomorHp);
        }
    }

    public class User : Person
    {
        private int id;
        private string username;
        private string password;
        private UserRole role;
        private DateTime createdAt;
        private UserStatus status;

        public int Id 
        { 
            get => id; 
            set => id = value >= 0 ? value : throw new ArgumentException("ID Tidak Boleh Negatif!"); 
        }

        public string Username 
        { 
            get => username; 
            set => username = value?.Trim().ToLower(); 
        }

        public string Password 
        { 
            get => password; 
            set => password = value; 
        }

        public UserRole Role 
        { 
            get => role; 
            set => role = value; 
        }

        public DateTime CreatedAt 
        { 
            get => createdAt; 
            set => createdAt = value; 
        }

        public UserStatus Status 
        { 
            get => status; 
            set => status = value; 
        }

        public User()
        {
            createdAt = DateTime.Now;
            status = UserStatus.Aktif;
            role = UserRole.Pengguna;
        }

        public User(string username, string password, string namaLengkap, string email, string nomorHp) : this()
        {
            Username = username;
            Password = password;
            NamaLengkap = namaLengkap;
            Email = email;
            NomorHp = nomorHp;
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
