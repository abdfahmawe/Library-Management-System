using LibrarySystem.BLL.DTOs.Request.Catalog;
using LibrarySystem.BLL.DTOs.Response.Catalog;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using LibrarySystem.BLL.Services.Interfaces;
using LibrarySystem.DAL.Models;
using LibrarySystem.DAL.Repositories.Interfaces;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.BLL.Services.Classes
{
   public  class MemberCatalogService : IMemberCatalogService
    {
        private readonly ILibraryItemRepository _libraryItemRepository;

        public MemberCatalogService(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        public async Task<IEnumerable<CatalogItemResponse>> GetCatalogAsync(CatalogFilterRequest request)
        {
            IEnumerable<LibraryItem> libraryItems = await _libraryItemRepository.SearchAvailableAsync
                (request.Title, request.Author, request.Year, request.Type);
            return libraryItems.Select(item => new CatalogItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                Author = item.Author,
                YearOfPublication = item.YearOfPublication,
                Type = item.GetType().Name,
                IsAvailable = item.IsAvailable
            });
        }

        public async Task<LibraryItemDetailsResponse?> GetLibraryItemByIdAsync(string id)
        {
          LibraryItem? libraryItem = await _libraryItemRepository.GetByIdAsync(id);
            if(libraryItem is null)
            {
                return null;
            }

            LibraryItemDetailsResponse response = new LibraryItemDetailsResponse
            {
                Id = libraryItem.Id,
                Title = libraryItem.Title,
                Author = libraryItem.Author,
                YearOfPublication = libraryItem.YearOfPublication,
                IsAvailable = libraryItem.IsAvailable,
                Type = libraryItem.GetType().Name
            };
            // if libraryItem is from type Book, set ISBN and NumberOfPages using book properties 
            // libraryItem => book from type Book
            if (libraryItem is Book book)
            {
                response.ISBN = book.ISBN;
                response.NumberOfPages = book.NumberOfPages;
            }
            else if (libraryItem is Magazine magazine)
            {
                response.IssueNumber = magazine.IssueNumber;
                response.Category = magazine.Category;
            }
            else if (libraryItem is Newspaper newspaper)
            {
                response.PublicationDate = newspaper.PublicationDate;
                response.Region = newspaper.Region;
            }

            return response;

        }
    }
}
