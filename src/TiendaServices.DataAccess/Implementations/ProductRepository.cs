using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionstring)
        {
            _connectionString = connectionstring;
        }

        public async Task<bool> CreateAsync(Product value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Produtos]
                                                           ( [Id]
                                                            ,[NombreProducto]
                                                            ,[Precio]
                                                            ,[Marca]
                                                            ,[Descripcion]
                                                            ,[Stock]
                                                            ,[Activo]
                                                            ,[FechaCreacion]
                                                            ,[FechaModificacion]
                                                            ,[FkCategoria])
                                                        VALUES ( @Id, @NombreProducto, @Precio, @Marca, @Descripcion, @Stock, @Activo, @FechaCreacion, @FechaModificacion, @FkCategoria", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("NombreProducto", value.ProductName);
                sqlCommand.Parameters.AddWithValue("Precio", value.Price);
                sqlCommand.Parameters.AddWithValue("Marca", value.Trademark);
                sqlCommand.Parameters.AddWithValue("Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("Stock", value.Stock);
                sqlCommand.Parameters.AddWithValue("Activo", value.Active);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", value.ModificationDate);
                sqlCommand.Parameters.AddWithValue("FkCategoria", value.FKCategory);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Produtos] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[NombreProducto]
                                                      ,[Precio]
                                                      ,[Marca]
                                                      ,[Descripcion]
                                                      ,[Stock]
                                                      ,[Activo]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                      ,[FkCategoria]
                                                       FROM [dbo].[Produtos]", sqlConnection);

                using var result = sqlCommand.ExecuteReader();
                List<Product> products = new List<Product>();

                while (await result.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = result.GetGuid(0),
                        ProductName = result["NombreProducto"].ToString(),
                        Price = result.GetFloat(2),
                        Trademark = result.GetString(3),
                        Description = result.GetString(4),
                        Stock = result.GetInt32(5),
                        Active = result.GetBoolean(6),
                        CreationDate = result.GetDateTime(7),
                        ModificationDate = result.GetDateTime(8),
                        FKCategory = result.GetGuid(9)
                    });
                }
                return products;
            }
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT  [Id]
                                                      ,[NombreProducto]
                                                      ,[Precio]
                                                      ,[Marca]
                                                      ,[Descripcion]
                                                      ,[Stock]
                                                      ,[Activo]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                      ,[FkCategoria]
                                                       FROM [dbo].[Produtos] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var product = new Product();
                    while (reader.Read())
                    {
                        product.Id = reader.GetGuid(0);
                        product.ProductName = reader.GetString(1);
                        product.Price = reader.GetFloat(2);
                        product.Trademark = reader.GetString(3);
                        product.Description = reader.GetString(4);
                        product.Stock = reader.GetInt32(5);
                        product.Active = reader.GetBoolean(6);
                        product.CreationDate = reader.GetDateTime(7);
                        product.ModificationDate = reader.GetDateTime(8);
                        product.FKCategory = reader.GetGuid(9);
                    }
                    return product;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Produtos]", sqlConnection);
                var count = (int)sqlCommand.ExecuteScalar();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(Product value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Produtos]
                                                        SET [NombreProducto] = @NombreProducto
                                                      ,[Precio] =@Precio
                                                      ,[Marca] = @Marca
                                                      ,[Descripcion] = @Descripcion
                                                      ,[Stock] = @Stock
                                                      ,[Activo] = @Activo
                                                      ,[FechaCreacion] = @FechaCreacion
                                                      ,[FechaModificacion] = @FechaModificacion
                                                      ,[FkCategoria] = @FkCategoria
                                                       WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("@NombreProducto", value.ProductName);
                sqlCommand.Parameters.AddWithValue("@Precio", value.Price);
                sqlCommand.Parameters.AddWithValue("@Marca", value.Trademark);
                sqlCommand.Parameters.AddWithValue("@Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("@Stock", value.Description);
                sqlCommand.Parameters.AddWithValue("@Activo", value.Description);
                sqlCommand.Parameters.AddWithValue("@FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", value.ModificationDate);
                sqlCommand.Parameters.AddWithValue("@FkCategoria", value.ModificationDate);

                var updateCategory = sqlCommand.ExecuteNonQuery();
                return updateCategory == 1;
            }
        }
    }
}