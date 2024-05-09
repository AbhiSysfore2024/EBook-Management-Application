  CREATE TABLE ADOAuthor (
     AuthorID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
     FirstName VARCHAR(100) NOT NULL,
     LastName VARCHAR(100),
     Biography VARCHAR(100),
     BirthDate DATE,
     Country VARCHAR(100),
     CreatedAt DATETIME,
     UpdatedAt DATETIME
 )

 EXEC sp_rename 'ADOAuthor.MiddleName', 'LastName', 'COLUMN';


  CREATE TABLE ADOBook (
     BookID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
     Title VARCHAR(100) NOT NULL,
     Descriptions VARCHAR(100),
     ISBN UNIQUEIDENTIFIER NOT NULL,
     PublicationDate DATETIME,
     Price FLOAT,
     BookLanguage VARCHAR(100),
     Publisher VARCHAR(100),
     PagesCount INT,
     AvgRating FLOAT,
     BookGenre INT,
     FOREIGN KEY (BookGenre) REFERENCES Genre(GenreId),
     IsAvailable BIT,
     CreatedAt DATETIME,
     UpdatedAt DATETIME
 )
 

  CREATE TABLE Genre (
     GenreId INT IDENTITY(1,1) PRIMARY KEY,
     GenreName VARCHAR(255)
 );


 INSERT INTO Genre (GenreName)
 VALUES ('Action'),
        ('Adventure'),
        ('Sci-Fi'),
 	   ('Mystery'),
 	   ('Comedy'),
 	   ('Fiction');


 CREATE TABLE BookAuthor (
    BookID UNIQUEIDENTIFIER,
    AuthorID UNIQUEIDENTIFIER,
    PRIMARY KEY (BookID, AuthorID),
    FOREIGN KEY (BookID) REFERENCES ADOBook(BookID),
    FOREIGN KEY (AuthorID) REFERENCES ADOAuthor(AuthorID)
);


 CREATE PROCEDURE GetBookDetails AS BEGIN SELECT * FROM ADOBook END;
 CREATE PROCEDURE GetAuthorDetails AS BEGIN SELECT * FROM ADOAuthor END;


 CREATE TYPE AuthorIDList AS TABLE (
    AuthorID UNIQUEIDENTIFIER
);


  CREATE PROCEDURE InsertBook ( 
     @BookID UNIQUEIDENTIFIER,
     @Title VARCHAR(100),
     @Descriptions VARCHAR(100),
     @ISBN UNIQUEIDENTIFIER,
     @PublicationDate DATETIME,
     @Price FLOAT,
     @BookLanguage VARCHAR(100),
     @Publisher VARCHAR(100),
     @PagesCount INT,
     @AvgRating FLOAT,
     @BookGenre INT,
     @IsAvailable BIT,
     @CreatedAt DATETIME,
     @UpdatedAt DATETIME,
	 @AuthorIDs AuthorIDList READONLY
 	)
 	AS BEGIN 
 	INSERT INTO ADOBook (BookID, Title, Descriptions, ISBN, PublicationDate, Price, BookLanguage, Publisher, PagesCount, AvgRating, BookGenre, IsAvailable, CreatedAt, UpdatedAt) 
 VALUES ( @BookID, @Title, @Descriptions, @ISBN, @PublicationDate, @Price, @BookLanguage, @Publisher, @PagesCount, @AvgRating, @BookGenre, @IsAvailable, @CreatedAt, @UpdatedAt);


 INSERT INTO BookAuthor (BookID, AuthorID)
    SELECT @BookID, AuthorID
    FROM @AuthorIDs;
 END;



  CREATE PROCEDURE InsertAuthor ( 
     @AuthorID UNIQUEIDENTIFIER,
     @FirstName VARCHAR(100),
     @LastName VARCHAR(100),
     @Biography VARCHAR(100),
     @BirthDate DATE,
     @Country VARCHAR(100),
     @CreatedAt DATETIME,
     @UpdatedAt DATETIME
 	)
 	AS BEGIN 
 	INSERT INTO ADOAuthor (AuthorID, FirstName, LastName, Biography, BirthDate, Country, CreatedAt, UpdatedAt ) 
 VALUES ( @AuthorID, @FirstName, @LastName, @Biography, @BirthDate, @Country, @CreatedAt, @UpdatedAt );
 END;


 CREATE TABLE BookAuthor (
    BookID UNIQUEIDENTIFIER,
    AuthorID UNIQUEIDENTIFIER,
    PRIMARY KEY (BookID, AuthorID),
    FOREIGN KEY (BookID) REFERENCES ADOBook(BookID),
    FOREIGN KEY (AuthorID) REFERENCES ADOAuthor(AuthorID)
);


 CREATE PROCEDURE UpdateBook (
     @BookID UNIQUEIDENTIFIER,
     @Title VARCHAR(100),
     @Descriptions VARCHAR(100),
     @ISBN UNIQUEIDENTIFIER,
     @PublicationDate DATETIME,
     @Price FLOAT,
     @BookLanguage VARCHAR(100),
     @Publisher VARCHAR(100),
     @PagesCount INT,
     @AvgRating FLOAT,
     @BookGenre INT,
     @IsAvailable BIT,
     @UpdatedAt DATETIME,
	 @AuthorIDs AuthorIDList READONLY
 )
 AS BEGIN 
    UPDATE ADOBook SET Title = @Title, Descriptions = @Descriptions, ISBN = @ISBN, PublicationDate = @PublicationDate, 
    Price = @Price, BookLanguage = @BookLanguage, Publisher = @Publisher, PagesCount = @PagesCount, 
    AvgRating = @AvgRating, BookGenre = @BookGenre, IsAvailable = @IsAvailable, UpdatedAt = @UpdatedAt WHERE BookID = @BookID;

	DELETE FROM BookAuthor WHERE BookID = @BookID;

	INSERT INTO BookAuthor (BookID, AuthorID)
    SELECT @BookID, AuthorID FROM @AuthorIDs;
	END


 CREATE PROCEDURE UpdateAuthor (
    @AuthorID UNIQUEIDENTIFIER,
     @FirstName VARCHAR(100),
     @LastName VARCHAR(100),
     @Biography VARCHAR(100),
     @BirthDate DATE,
     @Country VARCHAR(100),
     @UpdatedAt DATETIME
	 ) AS BEGIN UPDATE ADOAuthor SET AuthorID = @AuthorID, FirstName = @FirstName, LastName = @LastName,
	   Biography = @Biography, BirthDate = @BirthDate, Country = @Country, UpdatedAt = @UpdatedAt WHERE AuthorID = @AuthorID; END


 CREATE PROCEDURE DeleteBook(
     @BookID UNIQUEIDENTIFIER ) AS BEGIN 
	 DELETE FROM BookAuthor WHERE BookID = @BookID
	 DELETE FROM ADOBook WHERE BookID = @BookID; 
 	END


 CREATE PROCEDURE DeleteAuthor(
     @AuthorID UNIQUEIDENTIFIER ) AS BEGIN DELETE FROM ADOAuthor WHERE AuthorID = @AuthorID; 
 	END


 CREATE PROCEDURE GetBooksByTitle (@Title VARCHAR(100)) AS BEGIN SELECT * FROM ADOBook WHERE Title LIKE '%' + @Title + '%'; END
 CREATE PROCEDURE GetBooksByGenre (@BookGenre INT) AS BEGIN SELECT * FROM ADOBook WHERE BookGenre = @BookGenre; END

 DROP PROCEDURE GetBooksByTitle

 EXEC  GetBooksByTitle 'twinklestar'


  CREATE PROCEDURE samplequery (@Title VARCHAR) AS BEGIN SELECT * FROM ADOBook WHERE Title = @Title; END


 CREATE PROCEDURE GroupBooksOnGenreName AS BEGIN SELECT ADOBook.BookID, ADOBook.BookGenre, ADOBook.Title, ADOBook.BookLanguage, Genre.GenreName FROM ADOBook JOIN Genre ON ADOBook.BookGenre = Genre.GenreId
 ORDER BY Genre.GenreName; END


  SELECT ADOBook.BookID, ADOBook.BookGenre, ADOBook.Title, ADOBook.BookLanguage FROM ADOBook JOIN Genre ON ADOBook.BookGenre = Genre.GenreId
 ORDER BY Genre.GenreName; 


SELECT ADOBook.AuthorID, ADOBook.BookID, ADOAuthor.FirstName, ADOBook.Title FROM ADOBook
JOIN ADOAuthor ON ADOAuthor.AuthorID = ADOBook.AuthorID WHERE ADOAuthor.FirstName = 'tagore';


CREATE PROCEDURE GetBooksByAuthorName (@FirstName VARCHAR(100)) AS BEGIN
    SELECT ADOBook.BookID, ADOBook.Title, ADOBook.Descriptions FROM ADOBook JOIN BookAuthor ON ADOBook.BookID = BookAuthor.BookID
    JOIN ADOAuthor ON BookAuthor.AuthorID = ADOAuthor.AuthorID
    WHERE ADOAuthor.FirstName = @FirstName; END


CREATE PROCEDURE GetAuthorsOfABook (@Title VARCHAR(100)) AS BEGIN SELECT ADOBook.Title, BookAuthor.AuthorID, ADOAuthor.FirstName FROM ADOBook JOIN BookAuthor ON ADOBook.BookID = BookAuthor.BookID
INNER JOIN ADOAuthor ON ADOAuthor.AuthorID = BookAuthor.AuthorID
WHERE ADOBook.Title = @Title; END


 CREATE PROCEDURE UpdateBook (
     @BookID UNIQUEIDENTIFIER,
     @Title VARCHAR(100),
     @Descriptions VARCHAR(100),
     @ISBN UNIQUEIDENTIFIER,
     @PublicationDate DATETIME,
     @Price FLOAT,
     @BookLanguage VARCHAR(100),
     @Publisher VARCHAR(100),
     @PagesCount INT,
     @AvgRating FLOAT,
     @BookGenre INT,
     @IsAvailable BIT,
     @UpdatedAt DATETIME,
	 @AuthorIDs AuthorIDList READONLY
 )
 AS BEGIN 
    UPDATE ADOBook SET Title = @Title, Descriptions = @Descriptions, ISBN = @ISBN, PublicationDate = @PublicationDate, 
    Price = @Price, BookLanguage = @BookLanguage, Publisher = @Publisher, PagesCount = @PagesCount, 
    AvgRating = @AvgRating, BookGenre = @BookGenre, IsAvailable = @IsAvailable, UpdatedAt = @UpdatedAt WHERE BookID = @BookID;

	DELETE FROM BookAuthor WHERE BookID = @BookID;

	INSERT INTO BookAuthor (BookID, AuthorID)
    SELECT @BookID, AuthorID FROM @AuthorIDs;
	END

	CREATE TABLE ADOAccess(
	   Username VARCHAR(100),
	   Password VARCHAR(100),
	   RoleAssigned VARCHAR(100)
	 )

	 INSERT INTO ADOAccess (Username, Password, RoleAssigned)
	 VALUES ('abhi', 'abhilash', 'Admin'),
	        ('vishnu', 'vishnu', 'User')

	 CREATE PROCEDURE GetRole (
	   @Username VARCHAR(100),
	   @Password VARCHAR (100) ) AS BEGIN SELECT ADOAccess.RoleAssigned FROM ADOAccess WHERE Username = @Username AND Password = @Password; END

	   EXEC GetRole 'abhi', 'abhilash'

	   select * from ADOAccess