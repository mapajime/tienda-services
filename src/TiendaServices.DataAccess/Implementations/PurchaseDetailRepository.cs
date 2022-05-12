using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class PurchaseDetailRepository : IPurchaseDetailRepository
    {
        private readonly string _connectionString;

        public PurchaseDetailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(PurchaseDetail value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[DetalleCompras]
                                                           ([Id]
                                                          ,[Cantidad]
                                                          ,[Precio]
                                                          ,[FechaCreacion]
                                                          ,[FechaModificacion]
                                                          ,[FkProducto]
                                                          ,[FkCompra])
                                                        VALUES ( @Id, @Cantidad, @Precio, @FechaCreacion, @FechaModificacion, @FkProducto, @FkCompra", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("Cantidad", value.Quantity);
                sqlCommand.Parameters.AddWithValue("Precio", value.Price);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", value.ModificationDate);
                sqlCommand.Parameters.AddWithValue("FkProducto", value.FKProduct);
                sqlCommand.Parameters.AddWithValue("FkCompra", value.FKPurchase);

                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[DetalleCompras] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<PurchaseDetail>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[Cantidad]
                                                      ,[Precio]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                      ,[FkProducto]
                                                      ,[FkCompra]
                                                       FROM [dbo].[DetalleCompras]", sqlConnection);

                using var result = sqlCommand.ExecuteReader();
                List<PurchaseDetail> purchaseDetails = new List<PurchaseDetail>();

                while (await result.ReadAsync())
                {
                    purchaseDetails.Add(new PurchaseDetail
                    {
                        Id = result.GetGuid(0),
                        Quantity = result.GetInt32(1),
                        Price = result.GetFloat(2),
                        CreationDate = result.GetDateTime(3),
                        ModificationDate = result.GetDateTime(4),
                        FKProduct = result.GetGuid(5),
                        FKPurchase = result.GetGuid(6)
                    });
                }
                return purchaseDetails;
            }
        }

        public async Task<PurchaseDetail> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                          ,[Cantidad]
                                                          ,[Precio]
                                                          ,[FechaCreacion]
                                                          ,[FechaModificacion]
                                                          ,[FkProducto]
                                                          ,[FkCompra]
                                                       FROM [dbo].[DetalleCompras] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var purchaseDetail = new PurchaseDetail();
                    while (reader.Read())
                    {
                        purchaseDetail.Id = reader.GetGuid(0);
                        purchaseDetail.Quantity = reader.GetInt32(1);
                        purchaseDetail.Price = reader.GetFloat(2);
                        purchaseDetail.CreationDate = reader.GetDateTime(3);
                        purchaseDetail.ModificationDate = reader.GetDateTime(4);
                        purchaseDetail.FKProduct = reader.GetGuid(5);
                        purchaseDetail.FKPurchase = reader.GetGuid(6);
                    }
                    return purchaseDetail;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[DetalleCompras]", sqlConnection);
                var count = (int)sqlCommand.ExecuteScalar();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(PurchaseDetail value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[DetalleCompras]
                                                        SET [Cantidad] = @Cantidad
                                                         ,[Precio] =@Precio
                                                         ,[FechaCreacion] = @FechaCreacion
                                                         ,[FechaModificacion] = @FechaModificacion
                                                         ,[FkProducto] = @FkProducto
                                                         ,[FkCompra] = @FkCompra
                                                       WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("@Cantidad", value.Quantity);
                sqlCommand.Parameters.AddWithValue("@Precio", value.Price);
                sqlCommand.Parameters.AddWithValue("@FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", value.ModificationDate);
                sqlCommand.Parameters.AddWithValue("@FkProducto", value.FKProduct);
                sqlCommand.Parameters.AddWithValue("@FkCompra", value.FKPurchase);

                var updateCategory = sqlCommand.ExecuteNonQuery();
                return updateCategory == 1;
            }
        }
    }
}