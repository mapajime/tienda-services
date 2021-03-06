using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(Category value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Categorias]
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
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[Categorias] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[Nombre]
                                                      ,[Descripcion]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[Categorias]", sqlConnection);

                using var result = await sqlCommand.ExecuteReaderAsync();
                List<Category> categories = new List<Category>();

                while (await result.ReadAsync())
                {
                    categories.Add(new Category
                    {
                        Id = result.GetGuid(0),
                        Name = result["Nombre"].ToString(),
                        Description = result.GetString(2),
                        CreationDate = result.GetDateTime(3),
                        ModificationDate = result.GetDateTime(4)
                    });
                }
                return categories;
            }
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT[Id]
                                                      ,[Nombre]
                                                      ,[Descripcion]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[Categorias] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var category = new Category();
                    while (reader.Read())
                    {
                        category.Id = reader.GetGuid(0);
                        category.Name = reader.GetString(1);
                        category.Description = reader.GetString(2);
                        category.CreationDate = reader.GetDateTime(3);
                        category.ModificationDate = reader.GetDateTime(4);
                    }
                    return category;
                }
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT id, nombre, descripcion, fechaCreacion, fechaModificacion FROM [dbo].[Categorias] WHERE nombre = @name", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Name", name);

                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    if (!reader.HasRows) return null;

                    var category = new Category();
                    while (reader.Read())
                    {
                        category.Id = reader.GetGuid(0);
                        category.Name = reader.GetString(1);
                        category.Description = reader.GetString(2);
                        category.CreationDate = reader.GetDateTime(3);
                        category.ModificationDate = reader.GetDateTime(4);
                    }
                    return category;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Categorias]", sqlConnection);
                var count = (int)await sqlCommand.ExecuteScalarAsync();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(Category value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Categorias]
                                                        SET [Nombre] = @Nombre
                                                      ,[Descripcion] =@Descripcion
                                                      ,[FechaModificacion] = @FechaModificacion
                                                       WHERE Id=@id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("@Descripcion", value.Description);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", DateTime.UtcNow);

                var updateCategory = await sqlCommand.ExecuteNonQueryAsync();
                return updateCategory == 1;
            }
        }
    }
}