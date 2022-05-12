using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(Country value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Ciudades]
                                                           ([Id]
                                                           ,[Nombre]
                                                           ,[Descripcion]
                                                           ,[FechaCreacion]
                                                           ,[FechaModificacion])
                                                        VALUES ( @Id, @Nombre, @Descripcion, @FechaCreacion, @FechaModificacion", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", value.ModificationDate);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Ciudades] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                await sqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[Nombre]
                                                      ,[Descripcion]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[Ciudades]", sqlConnection);

                using var result = sqlCommand.ExecuteReader();
                List<Country> countries = new List<Country>();

                while (await result.ReadAsync())
                {
                    countries.Add(new Country
                    {
                        Id = result.GetGuid(0),
                        Name = result["Nombre"].ToString(),
                        Description = result.GetString(2),
                        CreationDate = result.GetDateTime(3),
                        ModificationDate = result.GetDateTime(4)
                    });
                }
                return countries;
            }
        }

        public async Task<Country> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT[Id]
                                                      ,[Nombre]
                                                      ,[Descripcion]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[Ciudades] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = sqlCommand.ExecuteReader())
                {
                    var county = new Country();
                    while (reader.Read())
                    {
                        county.Id = reader.GetGuid(0);
                        county.Name = reader.GetString(1);
                        county.Description = reader.GetString(2);
                        county.CreationDate = reader.GetDateTime(3);
                        county.ModificationDate = reader.GetDateTime(4);
                    }
                    return county;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Ciudades]", sqlConnection);
                var count = (int)sqlCommand.ExecuteScalar();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(Country value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Ciudades]
                                                        SET [Nombre] = @Nombre
                                                      ,[Descripcion] =@Descripcion
                                                      ,[FechaCreacion] = @FechaCreacion
                                                      ,[FechaModificacion] = @FechaModificacion
                                                       WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", value.Id);
                sqlCommand.Parameters.AddWithValue("@Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("@Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("@FechaCreacion", value.CreationDate);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", value.ModificationDate);

                var updateCountry = sqlCommand.ExecuteNonQuery();
                return updateCountry == 1;
            }
        }
    }
}