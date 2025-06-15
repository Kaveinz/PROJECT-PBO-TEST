# 🍽️ MBOK WO RESERVE - Aplikasi Manajemen Reservasi Kursi

## 📋 Deskripsi Projek
Aplikasi desktop untuk manajemen reservasi kursi restoran yang dibangun menggunakan C# WinForms dengan implementasi lengkap prinsip-prinsip Object-Oriented Programming (OOP).

## 🎯 Fitur Utama

### 👨‍💼 Fitur Admin:
- ✅ Edit status reservasi pelanggan
- ✅ Batalkan reservasi 
- ✅ Cetak laporan berdasarkan status (selesai, pending, dibatalkan)
- ✅ Kelola data meja dan kapasitas
- ✅ Export laporan dalam berbagai format (TXT, CSV, HTML)

### 👤 Fitur Pengguna:
- ✅ Registrasi & login dengan validasi lengkap
- ✅ Buat reservasi kursi dengan pemilihan meja interaktif
- ✅ Lihat daftar reservasi pribadi
- ✅ Batalkan reservasi (dengan batasan waktu)
- ✅ Dashboard setelah login & logout yang aman

## 🏗️ Arsitektur Folder

### 📁 Struktur Folder (MVC Pattern)
```
📦 project/
├── 📂 models/          # Data Models & Business Logic
│   ├── User.cs         # Model pengguna dengan inheritance
│   ├── Reservation.cs  # Model reservasi dengan polymorphism
│   ├── Table.cs        # Model meja dengan abstraction
│   └── DatabaseHelper.cs # Database operations dengan singleton
├── 📂 view/            # User Interface (WinForms)
│   ├── auth_form.cs    # Login/Authentication
│   ├── form_register.cs # Registration
│   ├── layout-admin.cs # Admin dashboard
│   ├── layout-pelanggan.cs # Customer dashboard
│   ├── FormReservasiBaru.cs # Create reservation
│   ├── FormEditReservasi.cs # Edit reservation (admin)
│   ├── FormBatalReservasi.cs # Cancel reservation
│   ├── FormPilihKursi.cs # Table selection
│   ├── CetakLaporan.cs # Report printing
│   └── lihat reservasi.cs # View reservations
└── 📂 controller/      # Business Logic Controllers
    ├── AuthController.cs # Authentication logic
    ├── ReservationController.cs # Reservation management
    └── LaporanController.cs # Report generation
```

```
## 🎓 Konsep OOP yang Dipelajari

### 1. **Encapsulation Examples**
- Private fields dengan public properties
- Data validation dalam setter methods
- Method visibility control (private, protected, public)

### 2. **Inheritance Examples**
- `User` extends `Person` 
- `Table` extends `BaseTable`
- Controller hierarchies dengan base classes

### 3. **Polymorphism Examples**
- Method overriding dalam report controllers
- Interface implementations
- Virtual method dengan berbagai implementasi

### 4. **Abstraction Examples**
- Abstract classes untuk base functionality
- Interfaces untuk contract definition
- Hiding implementation details

## 📈 Fitur Lanjutan
- **Validation**: Input validation dengan error handling
- **Security**: Password hashing dengan bcrypt
- **Reporting**: Export ke multiple formats (TXT, CSV, HTML)
- **Transaction**: Database transaction untuk data consistency
- **Logging**: Login attempt tracking untuk security

## 🎯 Learning Outcomes
Proyek ini mendemonstrasikan:
- ✅ Implementasi lengkap 4 pilar OOP
- ✅ Design patterns (Singleton, Factory, Strategy)
- ✅ MVC architectural pattern
- ✅ Database integration dengan ORM-like patterns
- ✅ Error handling dan validation
- ✅ User interface design dengan WinForms
- ✅ Code organization dan maintainability

## 👨‍💻 Struktur Code untuk Mahasiswa
Code dirancang untuk mudah dipahami oleh mahasiswa semester awal dengan:
- Komentar yang jelas dan informatif
- Penamaan variabel yang deskriptif
- Struktur folder yang terorganisir
- Implementasi bertahap dari simple ke complex
- Best practices untuk maintainability

---
**Dibuat untuk tugas akhir OOP - Semester 2 Informatika** 🎓
