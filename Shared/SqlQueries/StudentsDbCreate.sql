use StudentsDb

create table Students(
	[Id] int primary key identity,
	[Name] nvarchar(30) not null,
	Surname nvarchar(30) not null,
	Age int check(Age between 16 and 70),
	Gender nvarchar(7),
	Email nvarchar(40) check(Email like '%@%'),
	Adress nvarchar(50)
)