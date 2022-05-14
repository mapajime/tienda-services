using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(Customer value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Usuarios]
                                                          ([Id]
                                                          ,[NombreUsuario]
                                                          ,[NumeroDocumento]
                                                          ,[Email]
                                                          ,[Telefono]
                                                          ,[Activo]
                                                          ,[FechaNacimiento]
                                                          ,[FechaCreacion]
                                                          ,[FechaModificacion]
                                                          ,[FkCiudad]
                                                          ,[FkTipoDocumento])
                                                        VALUES ( @Id, @NombreUsuario, @NumeroDocumento,@Email, @Telefono, @Activo, @FechaNacimiento, @ @FechaCreacion, @FechaModificacion, @FkCiudad, @FkTipoDocumento", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", Guid.NewGuid());
                sqlCommand.Parameters.AddWithValue("NombreUsuario", value.Name);
                sqlCommand.Parameters.AddWithValue("NumeroDocumento", value.DocumentNumber);
                sqlCommand.Parameters.AddWithValue("Email", value.Email);
                sqlCommand.Parameters.AddWithValue("Telefono", value.Phone);
                sqlCommand.Parameters.AddWithValue("Activo", value.Active);
                sqlCommand.Parameters.AddWithValue("FechaNacimiento", value.DateBirth);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("FkCiudad", value.FKCountry);
                sqlCommand.Parameters.AddWithValue("FkTipoDocumento", value.FKDocument);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Usuarios] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                          ,[NombreUsuario]
                                                          ,[NumeroDocumento]
                                                          ,[Email]
                                                          ,[Telefono]
                                                          ,[Activo]
                                                          ,[FechaNacimiento]
                                                          ,[FechaCreacion]
                                                          ,[FechaModificacion]
                                                          ,[FkCiudad]
                                                          ,[FkTipoDocumento]
                                                       FROM [dbo].[Usuarios]", sqlConnection);

                using var result = await sqlCommand.ExecuteReaderAsync();
                List<Customer> customers = new List<Customer>();

                while (await result.ReadAsync())
                {
                    customers.Add(new Customer
                    {
                        Id = result.GetGuid(0),
                        Name = result["NombreUsuario"].ToString(),
                        DocumentNumber = result.GetString(2),
                        Email = result.GetString(3),
                        Phone = result.GetString(4),
                        Active = result.GetBoolean(5),
                        DateBirth = result.GetDateTime(6),
                        CreationDate = result.GetDateTime(7),
                        ModificationDate = result.GetDateTime(8),
                        FKCountry = result.GetGuid(9),
                        FKDocument = result.GetGuid(10)
                    });
                }
                return customers;
            }
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT
                                                           [Id]
                                                          ,[NombreUsuario]
                                                          ,[NumeroDocumento]
                                                          ,[Email]
                                                          ,[Telefono]
                                                          ,[Activo]
                                                          ,[FechaNacimiento]
                                                          ,[FechaCreacion]
                                                          ,[FechaModificacion]
                                                          ,[FkCiudad]
                                                          ,[FkTipoDocumento]
                                                       FROM [dbo].[Usuarios] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var customer = new Customer();
                    while (reader.Read())
                    {
                        customer.Id = reader.GetGuid(0);
                        customer.Name = reader.GetString(1);
                        customer.DocumentNumber = reader.GetString(2);
                        customer.Email = reader.GetString(3);
                        customer.Phone = reader.GetString(4);
                        customer.Active = reader.GetBoolean(5);
                        customer.DateBirth = reader.GetDateTime(6);
                        customer.CreationDate = reader.GetDateTime(7);
                        customer.ModificationDate = reader.GetDateTime(8);
                        customer.FKCountry = reader.GetGuid(9);
                        customer.FKDocument = reader.GetGuid(10);
                    }
                    return customer;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Usuarios]", sqlConnection);
                var count = (int)await sqlCommand.ExecuteScalarAsync();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(Customer value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Usuarios]
                                                        SET [NombreUsuario] = @NombreUsuario
                                                          ,[NumeroDocumento] =@NumeroDocumento
                                                          ,[Email] = @Email
                                                          ,[Telefono] = @Telefono
                                                          ,[Activo] =@Activo
                                                          ,[FechaNacimiento] = @FechaNacimiento
                                                          ,[FechaModificacion] = @FechaModificacion
                                                          ,[FkCiudad] = @FkCiudad
                                                          ,[FkTipoDocumento] = @FkTipoDocumento
                                                        WHERE Id = @id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@NombreUsuario", value.Name);
                sqlCommand.Parameters.AddWithValue("@NumeroDocumento", value.DocumentNumber);
                sqlCommand.Parameters.AddWithValue("@Email", value.Email);
                sqlCommand.Parameters.AddWithValue("@Telefono", value.Phone);
                sqlCommand.Parameters.AddWithValue("@Activo", value.Active);
                sqlCommand.Parameters.AddWithValue("@FechaNacimiento", value.DateBirth);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("@FkCiudad", value.FKCountry);
                sqlCommand.Parameters.AddWithValue("@FkTipoDocumento", value.FKDocument);

                var updateCustomer = await sqlCommand.ExecuteNonQueryAsync();
                return updateCustomer == 1;
            }
        }
    }
}