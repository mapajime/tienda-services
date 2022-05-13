using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    internal class PurchaseRepository : IPurchaseRepository
    {
        private readonly string _connectionString;

        public PurchaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(Purchase value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Compras]
                                                            ([Id]
                                                            ,[FechaCompra]
                                                            ,[FechaCreacion]
                                                            ,[FechaModificacion]
                                                            ,[FkCliente])
                                                        VALUES ( @Id, @FechaCompra, @FechaCreacion, @FechaModificacion, @FkCliente", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", Guid.NewGuid());
                sqlCommand.Parameters.AddWithValue("FechaCompra", value.PurchaseDate);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("FkCliente", value.FKCustomer);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Compras] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<IEnumerable<Purchase>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                        ,[FechaCompra]
                                                        ,[FechaCreacion]
                                                        ,[FechaModificacion]
                                                        ,[FkCliente]
                                                       FROM [dbo].[Compras]", sqlConnection);

                using var result = await sqlCommand.ExecuteReaderAsync();
                List<Purchase> purchases = new List<Purchase>();

                while (await result.ReadAsync())
                {
                    purchases.Add(new Purchase
                    {
                        Id = result.GetGuid(0),
                        PurchaseDate = result.GetDateTime(1),
                        CreationDate = result.GetDateTime(2),
                        ModificationDate = result.GetDateTime(3),
                        FKCustomer = result.GetGuid(4)
                    });
                }
                return purchases;
            }
        }

        public async Task<Purchase> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[FechaCompra]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                      ,[FkCliente]
                                                       FROM [dbo].[Compras] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var purchase = new Purchase();
                    while (reader.Read())
                    {
                        purchase.Id = reader.GetGuid(0);
                        purchase.PurchaseDate = reader.GetDateTime(1);
                        purchase.CreationDate = reader.GetDateTime(3);
                        purchase.ModificationDate = reader.GetDateTime(4);
                        purchase.FKCustomer = reader.GetGuid(5);
                    }
                    return purchase;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Compras]", sqlConnection);
                var count = (int)await sqlCommand.ExecuteScalarAsync();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(Purchase value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Compras]
                                                        SET [FechaCompra] = @FechaCompra
                                                          ,[FechaModificacion] = @FechaModificacion
                                                          ,[FkCliente] = @FkCliente
                                                        WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@FechaCompra", value.PurchaseDate);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("@FkCliente", value.FKCustomer);

                var updatePurchase = await sqlCommand.ExecuteNonQueryAsync();
                return updatePurchase == 1;
            }
        }
    }
}