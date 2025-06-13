# 🚀 Development Guide - MBOK WO RESERVE

## 📋 Setup Instructions

### 1. Database Setup
1. Install PostgreSQL di komputer Anda
2. Buat database dengan nama `MBOK_WO_RESERVE`
3. Jalankan script `database_setup.sql` untuk create tables dan sample data
4. Update connection string di `models/DatabaseHelper.cs` sesuai setting PostgreSQL Anda

### 2. Project Setup
```bash
cd "PROJECT-PBO-TEST/test part kesekian"
dotnet restore
dotnet build
```

### 3. Default Login Credentials
- **Admin**: username: `admin`, password: `admin123`
- **Customer**: Daftar melalui form registrasi

## 🎯 Key Features Implemented

### ✅ Authentication & Authorization
- [x] User registration dengan validasi lengkap
- [x] Secure login dengan password hashing
- [x] Role-based access (Admin vs Customer)
- [x] Session management
- [x] Login attempt logging

### ✅ Reservation Management
- [x] Create reservation dengan table selection
- [x] View personal reservations (Customer)
- [x] Edit reservation status (Admin only)
- [x] Cancel reservations dengan business rules
- [x] Table availability checking
- [x] Capacity validation

### ✅ Reporting System
- [x] Generate reports by status (Pending, Confirmed, Completed, Cancelled)
- [x] Weekly and monthly reports
- [x] Export to multiple formats (TXT, CSV, HTML)
- [x] Print preview functionality
- [x] Report summary with statistics

### ✅ Admin Features
- [x] Dashboard dengan overview
- [x] Manage all reservations
- [x] Update reservation status
- [x] Generate and export reports
- [x] Table management

### ✅ Customer Features
- [x] Personal dashboard
- [x] Create new reservations
- [x] View personal reservation history
- [x] Cancel own reservations
- [x] Interactive table selection

## 🏗️ Architecture Overview

### MVC Pattern Implementation
```
📁 Models (Data Layer)
├── User.cs - User management dengan inheritance
├── Reservation.cs - Reservation logic dengan validation
├── Table.cs - Table management dengan polymorphism
└── DatabaseHelper.cs - Database operations dengan singleton

📁 Controllers (Business Logic)
├── AuthController.cs - Authentication & authorization
├── ReservationController.cs - Reservation business logic
└── LaporanController.cs - Report generation

📁 Views (Presentation Layer)
├── Authentication Forms (login, register)
├── Dashboard Forms (admin, customer)
├── Reservation Forms (create, edit, cancel, view)
└── Report Forms (generate, preview, export)
```

### OOP Principles Implementation

#### 1. Encapsulation 🔒
```csharp
public class User : Person
{
    private string _username;  // Private field
    
    public string Username     // Public property dengan validation
    { 
        get => _username; 
        set => _username = value?.Trim().ToLower(); 
    }
    
    public bool ValidateData() // Encapsulated validation logic
    {
        return !string.IsNullOrWhiteSpace(Username) && 
               Username.Length >= 3;
    }
}
```

#### 2. Inheritance 🧬
```csharp
// Base class
public abstract class Person
{
    public abstract string GetDisplayInfo();
    public virtual bool ValidateData() { /* default */ }
}

// Derived class
public class User : Person
{
    public override string GetDisplayInfo()
    {
        return $"{NamaLengkap} ({Username}) - {Role}";
    }
}
```

#### 3. Polymorphism 🎭
```csharp
// Base controller
public abstract class LaporanReservasiController
{
    public abstract DataTable AmbilData();
    public virtual void CetakLaporan(PrintPageEventArgs e) { /* base implementation */ }
}

// Specific implementations
public class LaporanMingguanController : LaporanReservasiController
{
    public override DataTable AmbilData() { /* weekly logic */ }
}

public class LaporanBulananController : LaporanReservasiController  
{
    public override DataTable AmbilData() { /* monthly logic */ }
}
```

#### 4. Abstraction 🎨
```csharp
// Interface definition
public interface IReservationService
{
    bool CreateReservation(Reservation reservation);
    bool CancelReservation(int reservationId, int userId);
    List<Reservation> GetUserReservations(int userId);
}

// Abstract implementation
public abstract class BaseReservationController : IReservationService
{
    protected readonly IDatabaseOperations _database;
    
    public abstract bool CreateReservation(Reservation reservation);
    
    public virtual bool CancelReservation(int reservationId, int userId)
    {
        // Default implementation dengan business rules
    }
}
```

## 🛠️ Development Patterns Used

### 1. Singleton Pattern
```csharp
public class AuthController : BaseAuthController
{
    private static AuthController _instance;
    private static readonly object _lock = new object();

    public static AuthController Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new AuthController(DatabaseHelper.Instance);
                }
            }
            return _instance;
        }
    }
}
```

### 2. Factory Pattern
```csharp
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
```

### 3. Strategy Pattern
```csharp
public void ExportToFile(string filePath, ExportFormat format)
{
    switch (format)
    {
        case ExportFormat.Text:
            ExportToTextFile(dt, filePath);
            break;
        case ExportFormat.CSV:
            ExportToCSVFile(dt, filePath);
            break;
        case ExportFormat.HTML:
            ExportToHTMLFile(dt, filePath);
            break;
    }
}
```

## 📊 Database Schema

### Core Tables
- **users**: User authentication dan profile
- **tables**: Table management dengan capacity
- **reservations**: Reservation transactions
- **login_logs**: Security logging

### Key Relationships
- users 1:N reservations (One user can have many reservations)
- tables 1:N reservations (One table can have many reservations over time)

## 🧪 Testing Guidelines

### Manual Testing Scenarios
1. **Authentication Flow**
   - Register new user dengan validation
   - Login dengan credentials yang benar/salah
   - Role-based dashboard access

2. **Reservation Flow**
   - Create reservation dengan table selection
   - Check table availability logic
   - Cancel reservation dengan time restrictions

3. **Admin Functions**
   - Edit reservation status
   - Generate reports dengan different filters
   - Export reports dalam berbagai format

### Error Handling
- Input validation dengan user-friendly messages
- Database connection error handling
- Business rule violations (e.g., double booking)

## 🎓 Learning Objectives Achieved

### OOP Mastery
- ✅ Encapsulation dengan proper access modifiers
- ✅ Inheritance dengan abstract classes dan virtual methods
- ✅ Polymorphism dengan method overriding
- ✅ Abstraction dengan interfaces dan abstract classes

### Design Patterns
- ✅ Singleton untuk global access objects
- ✅ Factory untuk object creation
- ✅ Strategy untuk algorithm selection
- ✅ MVC untuk separation of concerns

### Best Practices
- ✅ SOLID principles implementation
- ✅ Error handling dan input validation
- ✅ Code documentation dan naming conventions
- ✅ Database transaction management
- ✅ Security considerations (password hashing, SQL injection prevention)

## 🚀 Future Enhancements
- [ ] Email notifications untuk reservation confirmations
- [ ] Payment integration
- [ ] Mobile app version
- [ ] Real-time table status updates
- [ ] Advanced reporting dengan charts
- [ ] Multi-language support

---
**Happy Coding! 🎉**
