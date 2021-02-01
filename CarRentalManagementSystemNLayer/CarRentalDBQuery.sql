CREATE TABLE [dbo].[tbl_Companies] (
	[CompanyID] [int] IDENTITY (1,1), 
	[CompanyEmail] [nvarchar](50) NOT NULL,
	[CompanyPassword] [nvarchar] (50) NOT NULL,
	[CompanyName] [nvarchar] (50) NOT NULL,
	[CompanyCity] [nvarchar](50) NOT NULL,
	[CompanyAdress] [nvarchar] (500) NOT NULL,
	[CompanyPoints] [int] 
	CONSTRAINT [PK_tbl_Companies] PRIMARY KEY CLUSTERED(
		[CompanyID] ASC
	)
)
CREATE TABLE [dbo].[tbl_Vehicles](
	[VehicleID] [int] IDENTITY (1,1),
	[VehiclesCompanyID] [int] NOT NULL,
	[VehicleName] [nvarchar] (50) NOT NULL,
	[VehicleModel] [nvarchar] (100) NOT NULL,
	[VehiclesInstantKm] [int] NOT NULL,
	[HasAirbag] [bit] NOT NULL,
	[TrunkVolume] [int] NOT NULL,
	[SeatingCapacity] [int] NOT NULL,
	[DailyRentalPrice] [money] NOT NULL,
	[AgeLimitForDrivingThisCar] [int] NOT NULL,
	[KmLimitPerDay] [int] NOT NULL,
	[RequiredDrivingLicenseAge] [int] NOT NULL,
	CONSTRAINT [PK_tbl_Vehicles] PRIMARY KEY CLUSTERED(
		[VehicleID] ASC
	)
)
CREATE TABLE [dbo].[tbl_Customers](
	[CustomerID] [int] IDENTITY (1,1),
	[CustomerName] [nvarchar] (50) NOT NULL,
	[CustomerSurname] [nvarchar] (50) NOT NULL,
	[CustomerEmail] [nvarchar](50) NOT NULL,
	[CustomerPassword] [nvarchar] (50) NOT NULL,
	[CustomerPhoneNumber] [nvarchar] (10) NOT NULL,
	[CustomerAddress] [nvarchar] (250) NOT NULL,
	[SecondPhoneNumber] [nvarchar] (10) NOT NULL,
	[NationalIDCard] [nvarchar] (50) NOT NULL,
	CONSTRAINT [PK_tbl_Customers] PRIMARY KEY CLUSTERED 
	(
		[CustomerID] ASC
	)
)
CREATE TABLE [dbo].[tbl_RentedVehicles](
	[RentID] [int] IDENTITY (1,1),
	[RentedVehicleID] [int] NOT NULL,
	[SupplierCompanyID] [int] NOT NULL,
	[DriverCustomerID] [int] NOT NULL,
	[PickUpDate] [datetime] NOT NULL,
	[DropOffDate] [datetime] NOT NULL,
	[VehiclesPickUpKm] [int] NOT NULL,
	[VehiclesDropOffKm] [int] NOT NULL,
	[RentalPrice] [money] NOT NULL,
	CONSTRAINT [PK_tbl_RentedVehicles] PRIMARY KEY CLUSTERED 
	(
	[RentID] ASC
	)
)
CREATE TABLE [dbo].[tbl_Employees](
	[EmployeesID] [int] IDENTITY (1,1),
	[EmployeesCompanyID] [int] NOT NULL,
	[EmployeesEmail] [nvarchar] (50) NOT NULL,
	[EmployessPassword] [nvarchar] (50) NOT NULL,
	[EmployeesPhoneNumber] [nvarchar] (10) NOT NULL,
	[EmployeesAddress] [nvarchar] (250) NOT NULL,
	CONSTRAINT [PK_tbl_Employees] PRIMARY KEY CLUSTERED 
	(
	[EmployeesID] ASC
	)
)

CREATE TABLE [dbo].[tbl_RentalRequests](
	[RentalRequestID] [int] IDENTITY (1,1),
	[RentalRequestCustomerID] [int] NOT NULL,
	[RequestedSupplierCompanyID] [int] NOT NULL,
	[RequestedVehicleID] [int] NOT NULL,
	[RequestedPickUpDate] [datetime] NOT NULL,
	[RequestedDropOffDate] [datetime] NOT NULL,
	CONSTRAINT [PK_tbl_RentalRequest] PRIMARY KEY CLUSTERED 
	(
	[RentalRequestID] ASC
	)
)
CREATE TABLE [dbo].[tbl_Log](
	[LogId] [int] IDENTITY (1,1),
	[Date] [datetime] NOT NULL,
	[ExceptionMessage] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_tbl_Log] PRIMARY KEY CLUSTERED 
	(
	[LogId] ASC
	)
)
GO
CREATE PROCEDURE spInsertLog
@ExceptionMessage NVARCHAR(MAX)
AS
BEGIN
	INSERT INTO dbo.tbl_Log([Date], [ExceptionMessage])
	VALUES (GETDATE(), @ExceptionMessage)
END
GO
ALTER TABLE [dbo].[tbl_Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Vehicles_tbl_Companies] FOREIGN KEY([VehiclesCompanyID])
REFERENCES [dbo].[tbl_Companies] ([CompanyID])
GO

ALTER TABLE [dbo].[tbl_RentedVehicles]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentedVehicles_tbl_Companies] FOREIGN KEY([SupplierCompanyID])
REFERENCES [dbo].[tbl_Companies] ([CompanyID])
GO
ALTER TABLE [dbo].[tbl_RentedVehicles]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentedVehicles_tbl_Customers] FOREIGN KEY([DriverCustomerID])
REFERENCES [dbo].[tbl_Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[tbl_RentedVehicles]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentedVehicles_tbl_Vehicles] FOREIGN KEY([RentedVehicleID])
REFERENCES [dbo].[tbl_Vehicles] ([VehicleID])
GO
ALTER TABLE [dbo].[tbl_Employees]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Employees_tbl_Companies] FOREIGN KEY([EmployeesCompanyID])
REFERENCES [dbo].[tbl_Companies] ([CompanyID])
GO
ALTER TABLE [dbo].[tbl_RentalRequests]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentalRequests_tbl_Companies] FOREIGN KEY([RequestedSupplierCompanyID])
REFERENCES [dbo].[tbl_Companies] ([CompanyID])
GO
ALTER TABLE [dbo].[tbl_RentalRequests]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentalRequests_tbl_Customers] FOREIGN KEY([RentalRequestCustomerID])
REFERENCES [dbo].[tbl_Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[tbl_RentalRequests]  WITH CHECK ADD  CONSTRAINT [FK_tbl_RentalRequests_tbl_Vehicles] FOREIGN KEY([RequestedVehicleID])
REFERENCES [dbo].[tbl_Vehicles] ([VehicleID])
GO
