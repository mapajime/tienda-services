using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class CityRepository : ICityRepository
    {
        private readonly string _connectionString;

        public CityRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(City value)
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
                sqlCommand.Parameters.AddWithValue("Id", Guid.NewGuid());
                sqlCommand.Parameters.AddWithValue("Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("FechaCreacion", DateTime.UtcNow);
                sqlCommand.Parameters.AddWithValue("FechaModificacion", DateTime.UtcNow);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Ciudades] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<IEnumerable<City>> GetAllAsync()
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

                using var result = await sqlCommand.ExecuteReaderAsync();
                List<City> countries = new List<City>();

                while (await result.ReadAsync())
                {
                    countries.Add(new City
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

        public async Task<City> GetByIdAsync(Guid id)
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
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var county = new City();
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
                var count = (int)await sqlCommand.ExecuteScalarAsync();
                return count;
            }
        }

        public async Task<City> GetByNameAsync(string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT[Id]
                                                            ,[Nombre]
                                                            ,[Descripcion]
                                                            ,[FechaModificacion]
                                                            ,[FechaCreacion]
                                                        FROM[dbo].[Ciudades] WHERE Nombre = @name", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Name", name);

                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (!reader.HasRows) return null;

                    City city = new City();
                    while (reader.Read())
                    {
                        city.Id = reader.GetGuid(0);
                        city.Name = reader.GetString(1);
                        city.Description = reader.GetString(2);
                        city.CreationDate = reader.GetDateTime(3);
                        city.ModificationDate = reader.GetDateTime(4);
                    }
                    return city;
                }
            }
        }

        public async Task<bool> UpdateAsync(City value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Ciudades]
                                                        SET [Nombre] = @Nombre
                                                      ,[Descripcion] =@Descripcion
                                                      ,[FechaModificacion] = @FechaModificacion
                                                       WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("@Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", DateTime.UtcNow);

                var updateCity = await sqlCommand.ExecuteNonQueryAsync();
                return updateCity == 1;
            }
        }
    }
}