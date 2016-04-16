create database AadharDB

use AadharDB
go

create table AadharMaster
(
	id int primary key identity not null,
	-- Setting Unique key --
	aadharNo numeric(12,0) unique,
	firstName nvarchar(50),
	lastName nvarchar(50),
	gender int check(gender >= 1 and gender <= 3),
	dob date,
	bloodGroup int check(bloodGroup >=1 and bloodGroup <= 8),
	isGaurded bit not null,
);

create table ContectMaster
(
	id int foreign key references dbo.AadharMaster(id) not null,
	phoneNumber numeric(10),
	emailId nvarchar(50),
	isPrimary bit not null,
);

create table ConnectedAccountMaster
(
	id int foreign key references dbo.AadharMaster(id) not null,
	licenseNumber nvarchar(15),
);

-- Change table name --
-- exec sp_rename 'ConnectedAccount', 'ConnectedAccountMaster' --

create table GardianMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.AadharMaster(id) not null,
	typeGardian int check(typeGardian >= 1 and typeGardian <=3),
	gFirstName nvarchar(25),
	gMiddleName nvarchar(25),
	gLastName nvarchar(25),
	gAadharNo numeric(12,0) foreign key references dbo.AadharMaster(aadharNo),
);

create table DigitalInfoMaster
(
	id int foreign key references dbo.AadharMaster(id) not null,
	photo varbinary(max),
	lThumb varbinary(max),
	rThumb varbinary(max),
	lFingers varbinary(max),
	rFingers varbinary(max),
	signature varbinary(max),
);

create table PermentAddressMaster
(
	id int foreign key references dbo.AadharMaster(id) not null,
	mainAddress nvarchar(50),
	nearByAddress nvarchar(50),
	optionalAddress nvarchar(50),
	city nvarchar(25),
	pincode numeric(6),
	IsLiveInPermentAddress bit not null,
);

create table PersentAddressMaster
(
	id int foreign key references dbo.AadharMaster(id) not null,
	mainAddress nvarchar(50),
	nearByAddress nvarchar(50),
	optionalAddress nvarchar(50),
	city nvarchar(25),
	pincode numeric(6),
	duration int,
);

create table IntigrationMaster
(
	requestId int primary key identity(1,1) not null,
	id int foreign key references dbo.AadharMaster(id) not null,
	aadharNo numeric(12,0) foreign key references dbo.AadharMaster(aadharNo) not null,
	externalUniqueId nvarchar(15) unique,
	clientType nvarchar(15),
	requestDateTime datetime,
);

create table OTPMaster
(
	id int primary key identity(1,1) not null,
	requestId int foreign key references dbo.IntigrationMaster(requestId) not null,
	mOTP numeric(4) not null,
	eOTP numeric(6) not null,
);