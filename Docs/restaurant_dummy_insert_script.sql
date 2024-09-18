DECLARE @counter INT = 1;
DECLARE @maxRecords INT = 5000;
DECLARE @OwnerId NVARCHAR(450) = '731bbb9c-0871-44df-ba30-eab9df9e11cc';

WHILE @counter <= @maxRecords
BEGIN
    INSERT INTO [dbo].[Restaurants]
    (
        [Name],
        [Description],
        [Category],
        [HasDelivery],
        [ContactEmail],
        [ContactNumber],
        [Address_City],
        [Address_Street],
        [Address_PostalCode],
        [OwnerId]
    )
    VALUES
    (
        'Restaurant ' + CAST(@counter AS NVARCHAR(10)),          -- Name
        'Delicious food number ' + CAST(@counter AS NVARCHAR(10)),  -- Description
        CASE WHEN @counter % 3 = 0 THEN 'Japanese'               -- Category
             WHEN @counter % 3 = 1 THEN 'Italian'
             ELSE 'Mexican' 
        END,
        CASE WHEN @counter % 2 = 0 THEN 1 ELSE 0 END,            -- HasDelivery (randomly assigns true/false)
        'email' + CAST(@counter AS NVARCHAR(10)) + '@dummy.com',  -- ContactEmail
        '123456' + RIGHT('000' + CAST(@counter AS NVARCHAR(10)), 4),  -- ContactNumber
        'City ' + CAST(@counter AS NVARCHAR(10)),                 -- Address_City
        'Street ' + CAST(@counter AS NVARCHAR(10)),               -- Address_Street
        'PostalCode ' + CAST(@counter AS NVARCHAR(10)),           -- Address_PostalCode
        @OwnerId                                                  -- OwnerId (fixed value)
    );

    SET @counter = @counter + 1;
END;
