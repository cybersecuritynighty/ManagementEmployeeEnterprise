-- Create core tables
CREATE TABLE Departments (
    DepartmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    ManagerId UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE TABLE Employees (
    EmployeeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    IdentityUserId NVARCHAR(450) NOT NULL, -- Links to ASP.NET Core Identity
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    DepartmentId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Departments(DepartmentId),
    RoleId NVARCHAR(50) NOT NULL, -- Admin, HR, Manager, Employee
    BaseLocationLat DECIMAL(9,6) NULL,
    BaseLocationLon DECIMAL(9,6) NULL,
    RegisteredDeviceId NVARCHAR(255) NULL
);

CREATE TABLE Workspaces (
    WorkspaceId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    CostPerSqFt DECIMAL(18,2) NULL,
    IsActive BIT DEFAULT 1
);

CREATE TABLE CheckIns (
    CheckInId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    EmployeeId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Employees(EmployeeId),
    CheckInTime DATETIME2 NOT NULL,
    LocationLat DECIMAL(9,6) NOT NULL,
    LocationLon DECIMAL(9,6) NOT NULL,
    CapturedDeviceId NVARCHAR(255) NOT NULL,
    CheckInMethod NVARCHAR(20) NOT NULL, -- 'QR', 'GPS', 'Manual'
    IsFlagged BIT DEFAULT 0
);

CREATE TABLE FraudAlerts (
    AlertId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    CheckInId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES CheckIns(CheckInId),
    RuleName NVARCHAR(100) NOT NULL,
    SeverityLevel INT NOT NULL, -- 1 (Low) to 3 (Critical)
    Status NVARCHAR(20) DEFAULT 'Pending', -- 'Pending', 'Reviewed', 'Dismissed'
    ReviewedBy UNIQUEIDENTIFIER NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);
