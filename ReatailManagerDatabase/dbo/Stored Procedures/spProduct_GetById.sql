CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id INT
AS
begin
	set nocount on;

	SELECT p.Id, p.ProductName, p.Description, p.RetailPrice, p.QuantityInStock, p.IsTaxable
	FROM [dbo].[Product] p
	WHERE p.Id = @Id;
end
