using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServices.Business.Implementations;
using TiendaServices.Common.Exceptions;
using TiendaServices.DataAccess.Interfaces;
using TiendaServices.Entities;
using Xunit;

namespace TiendaServices.Business.Tests.Implementations
{
    public class DocumentTypeBusinessTest
    {
        private readonly Mock<IDocumentTypeRepository> _mockDocumentTypeRepository;

        public DocumentTypeBusinessTest()
        {
            _mockDocumentTypeRepository = new Mock<IDocumentTypeRepository>();
        }

        [Fact]
        public async Task CreateDocumentTypeAsync_WhenDocumentTypeIsNull_ShouldThrowDocumentTypeInvalidException()
        {
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.CreateDocumentTypeAsync(null));
            Assert.Contains("The document type is null", exception.Message);
            _mockDocumentTypeRepository.Verify(c => c.CreateAsync(It.IsAny<DocumentType>()), Times.Never);
        }

        [Fact]
        public async Task CreateDocumentTypeAsync_WhenDocumentTypeIsEmpty_ShouldThrowDocumentTypeInvalidException()
        {
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.CreateDocumentTypeAsync(new DocumentType { Name = string.Empty }));
            Assert.Contains("The name document type is empty", exception.Message);
            _mockDocumentTypeRepository.Verify(c => c.CreateAsync(It.IsAny<DocumentType>()), Times.Never);
        }

        [Fact]
        public async Task CreateDocumentTypeAsync_WhenDocumentTypeAlreadyExists_ShouldThrowDocumentTypeInvalidException()
        {
            _mockDocumentTypeRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new DocumentType { Name = "Cedula" });

            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAnyAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.CreateDocumentTypeAsync(new DocumentType { Name = "Cedula" }));
            Assert.Contains("The name document type already exists", exception.Message);
        }

        [Fact]
        public async Task CreateDocumentTypeAsync_WhenDocumentTypeIsOk_ShouldReturnTrue()
        {
            DocumentType documentType = null;
            _mockDocumentTypeRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((DocumentType)null);
            _mockDocumentTypeRepository.Setup(c => c.CreateAsync(It.IsAny<DocumentType>()))
                .Callback<DocumentType>(c => documentType = c)
                .ReturnsAsync(true);
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var result = await documentTypeBusiness.CreateDocumentTypeAsync(new DocumentType { Name = "Tarjeta Identidad" });
            Assert.True(result);
            Assert.NotNull(documentType.Name);
            Assert.Equal("Tarjeta Identidad", documentType.Name);
            _mockDocumentTypeRepository.Verify(c => c.CreateAsync(It.IsAny<DocumentType>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDocumentTypeAsync_WhenIdDocumentTypeIsOk_ShouldReturnTrue()
        {
            _mockDocumentTypeRepository.Setup(c => c.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var result = await documentTypeBusiness.DeleteDocumentTypeAsync(Guid.Empty);
            Assert.True(result);
            _mockDocumentTypeRepository.Verify(c => c.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllDocumentTypeAsync_WhenCountriesExists_ShouldReturnAllCountries()
        {
            _mockDocumentTypeRepository.Setup(c => c.GetAllAsync()).ReturnsAsync(
                new List<DocumentType>
                {
                    new DocumentType
                    {
                        Name = "Cedula"
                    },
                    new DocumentType
                    {
                         Name = "Tarjeta Identidad"
                    }
                });
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var documentsType = await documentTypeBusiness.GetAllDocumentTypeAsync();
            Assert.NotNull(documentsType);
            Assert.Equal("Cedula", documentsType.First().Name);
            Assert.Equal(2, documentsType.Count());
        }

        [Fact]
        public async Task GetDocumentTypeByIdAsync_WhenIdDocumentTypeExist_ShouldReturnDocumentType()
        {
            _mockDocumentTypeRepository.Setup(c => c.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(
                new DocumentType
                {
                    Name = "Pasaporte"
                });
            var documentTeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var result = await documentTeBusiness.GetDocumentTypeByIdAsync(Guid.Empty);
            Assert.NotNull(result);
            Assert.Equal("Pasaporte", result.Name);
            _mockDocumentTypeRepository.Verify(c => c.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDocumentTypeAsync_WhenDocumentTypeIsNull_ShouldThrowDocumentTypeInvalidException()
        {
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.UpdateDocumentTypeAsync(null));
            Assert.Contains("The document type is null", exception.Message);
            _mockDocumentTypeRepository.Verify(c => c.UpdateAsync(It.IsAny<DocumentType>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDocumentTypeAsync_WhenDocumentTypeIsEmpty_ShouldThrowDocumentTypeInvalidException()
        {
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.UpdateDocumentTypeAsync(new DocumentType()));
            Assert.Contains("The name document type is empty", exception.Message);
            _mockDocumentTypeRepository.Verify(c => c.UpdateAsync(It.IsAny<DocumentType>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDocumentTypeAsync_WhenDocumentTypeAlreadyExist_ShouldThrowDocumentTypeInvalidException()
        {
            _mockDocumentTypeRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(new DocumentType());
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var exception = await Assert.ThrowsAsync<DocumentTypeInvalidException>(() => documentTypeBusiness.UpdateDocumentTypeAsync(new DocumentType
            {
                Name = "Cedula"
            }));
            Assert.Contains("The name document type already exists", exception.Message);
            _mockDocumentTypeRepository.Verify(c => c.UpdateAsync(It.IsAny<DocumentType>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDocumentTypeAsync_WhenDocumentTypeIsOk_ShouldReturnTrue()
        {
            _mockDocumentTypeRepository.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((DocumentType)null);
            _mockDocumentTypeRepository.Setup(c => c.UpdateAsync(It.IsAny<DocumentType>())).ReturnsAsync(true);
            var documentTypeBusiness = new DocumentTypeBusiness(_mockDocumentTypeRepository.Object);
            var isDocumentType = await documentTypeBusiness.UpdateDocumentTypeAsync(new DocumentType
            {
                Name = "Pasaporte",
                CreationDate = new DateTime(2010, 12, 3),
                ModificationDate = new DateTime(2011, 2, 4)
            });
            Assert.True(isDocumentType);
            _mockDocumentTypeRepository.Verify(c => c.UpdateAsync(It.IsAny<DocumentType>()), Times.Once);
        }
    }
}