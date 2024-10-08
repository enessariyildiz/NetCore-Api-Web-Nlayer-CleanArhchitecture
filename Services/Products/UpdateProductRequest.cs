namespace App.Services.Productsi;

public record UpdateProductRequest(int Id, string Name, decimal Price, int Stock);
