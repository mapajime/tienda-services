using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;

namespace TiendaServices.DataAccess.Implementations
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly string _connectionString;

        public DocumentTypeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> CreateAsync(DocumentType value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[TipoDocumentos]
                                                           ([Id]
                                                           ,[Nombre]
                                                           ,[FechaCreacion]
                                                           ,[FechaModificacion])
                                                        VALUES ( @Id, @Nombre, @FechaCreacion, @FechaModificacion", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", Guid.NewGuid());
                sqlCommand.Parameters.AddWithValue("Nombre", value.Name);
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
                SqlCommand sqlCommand = new SqlCommand(@"DELETE FROM [dbo].[TipoDocumentos] WHERE Id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                var result = await sqlCommand.ExecuteNonQueryAsync();
                return result == 1;
            }
        }

        public async Task<IEnumerable<DocumentType>> GetAllAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[Nombre]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[TipoDocumentos]", sqlConnection);

                using var result = await sqlCommand.ExecuteReaderAsync();
                List<DocumentType> documentType = new List<DocumentType>();

                while (await result.ReadAsync())
                {
                    documentType.Add(new DocumentType
                    {
                        Id = result.GetGuid(0),
                        Name = result["Nombre"].ToString(),
                        CreationDate = result.GetDateTime(3),
                        ModificationDate = result.GetDateTime(4)
                    });
                }
                return documentType;
            }
        }

        public async Task<DocumentType> GetByIdAsync(Guid id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"SELECT [Id]
                                                      ,[Nombre]
                                                      ,[FechaCreacion]
                                                      ,[FechaModificacion]
                                                       FROM [dbo].[TipoDocumentos] WHERE Id = @Id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("Id", id);
                using (var reader = await sqlCommand.ExecuteReaderAsync())
                {
                    var documentType = new DocumentType();
                    while (reader.Read())
                    {
                        documentType.Id = reader.GetGuid(0);
                        documentType.Name = reader.GetString(1);
                        documentType.CreationDate = reader.GetDateTime(3);
                        documentType.ModificationDate = reader.GetDateTime(4);
                    }
                    return documentType;
                }
            }
        }

        public async Task<int> GetCountAsync()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand("SELECT COUNT(*) FROM [dbo].[TipoDocumentos]", sqlConnection);
                var count = (int) await sqlCommand.ExecuteScalarAsync();
                return count;
            }
        }

        public async Task<bool> UpdateAsync(DocumentType value)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                await sqlConnection.OpenAsync();
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[TipoDocumentos]
                                                           SET [Nombre] = @Nombre
                                                          ,[FechaModificacion] = @FechaModificacion
                                                           WHERE Id = @id ", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Nombre", value.Name);
                sqlCommand.Parameters.AddWithValue("@FechaModificacion", DateTime.UtcNow);

                var updateDocumenttype = await sqlCommand.ExecuteNonQueryAsync();
                return updateDocumenttype == 1;
            }
        }
    }
}