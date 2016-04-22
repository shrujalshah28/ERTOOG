create database ERTOOGDB

use ERTOOGDB
go

create table ErtoMaster
(
	id int primary key identity(1,1) not null,
	-- Cann't be nullable --
	uniqueId nvarchar(15) unique not null,
	firstName nvarchar(50) not null,
	lastName nvarchar(50) not null,
	gender int check(gender >= 1 and gender <= 3) not null,
	dob date not null,
	bloodGroup int check(bloodGroup >=1 and bloodGroup <= 8) not null,
	isGaurded bit not null,
);

create table ContectMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	phoneNumber numeric(10),
	emailId nvarchar(50),
	isPrimary bit not null,
);

create table GardianMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	typeGardian int check(typeGardian >= 1 and typeGardian <=3),
	gFirstName nvarchar(25),
	gMiddleName nvarchar(25),
	gLastName nvarchar(25),
);

create table DigitalInfoMaster
(
	pkey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	photo varbinary(max),
	lThumb varbinary(max),
	rThumb varbinary(max),
	lFingers varbinary(max),
	rFingers varbinary(max),
	signature varbinary(max),
);

create table PermentAddressMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	mainAddress nvarchar(50),
	nearByAddress nvarchar(50),
	optionalAddress nvarchar(50),
	city nvarchar(25),
	pincode numeric(6),
	IsLiveInPermentAddress bit not null,
);

create table PersentAddressMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	mainAddress nvarchar(50),
	nearByAddress nvarchar(50),
	optionalAddress nvarchar(50),
	city nvarchar(25),
	pincode numeric(6),
	duration int,
);

create table AuthorizePerson
(
	jobId int primary key identity(1,1) not null,
	destination nvarchar(15) not null,
	-- License number foreign key here --
	id int foreign key references dbo.ErtoMaster(id) not null,
	userName nvarchar(15) unique not null,
	password nvarchar(75) not null,
);

create table VerificationMaster
(
	pkey int primary key identity(1,1) not null,
	verifyWhom int foreign key references dbo.ErtoMaster(id),
	verifyBy int foreign key references dbo.AuthorizePerson(jobId),
	-- GETDATE() return current date --
	verifiedAt date default (GETDATE()),
	verified bit default 'false',
);

create table ExternalIdentityMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	idenityType int not null,
	identityNo nvarchar(20) unique not null,
);

create table ResponceMaster
(
	pKey int primary key identity(1,1) not null,
	aadharNo numeric(12,0) unique not null,
	uniqueId nvarchar(15) unique not null,
	requestId int unique,
	age int,
	phoneNumber numeric(10),
	emailId nvarchar(50),
	mOTP numeric(4),
	eOTP numeric(6),
);

create table LicenseMaster
(
	licenseNo nvarchar(15) primary key not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	licenseType int not null,
	validThrough nvarchar(25) not null default 'India',
	validFrom date not null,
	validTill date not null,
);

create table vanueMaster
(
	pKey int primary key identity(1,1) not null,
	vanueName nvarchar(15) not null,
);

create table AppointmentDateMaster
(
	pKey int primary key identity(1,1) not null,
	date date not null,
	totalCount int default 0,
	vanue int foreign key references dbo.VanueMaster(pKey),
);

create table AppointmentTimeMaster
(
	pkey int primary key identity(1,1) not null,
	date int foreign key references dbo.AppointmentDateMaster(pKey) not null,
	timeSlot int check(timeSlot >= 1 and timeSlot <=15) not null,
	totalCount int default 0,
);

create table AppointmentMaster
(
	appointmentId int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	timeOfAppointment int foreign key references dbo.AppointmentTimeMaster(pKey),
);

create table TestResultMaster
(
	pKey int primary key identity(1,1) not null,
	id int foreign key references dbo.ErtoMaster(id) not null,
	learningLicenseTestResult bit default 'false',
	learningLicenseTestCount int,
	permentLicenseTestResult bit default 'flase',
	permentLicenseTestCount int,
);