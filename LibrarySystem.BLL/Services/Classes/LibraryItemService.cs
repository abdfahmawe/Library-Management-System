using LibrarySystem.BLL.DTOs.Request.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Request.LibraryItem.NewsPaper;
using LibrarySystem.BLL.DTOs.Response.LibraryItem;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Book;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.Magazine;
using LibrarySystem.BLL.DTOs.Response.LibraryItem.NewsPaper;
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
    public class LibraryItemService : ILibraryItemService
    {
        private readonly ILibraryItemRepository _libraryItemRepository;

        public LibraryItemService(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        public async Task<BookResponse> AddBookAsync(AddBookRequest request)
        {
           Book book = request.Adapt<Book>();
             await _libraryItemRepository.AddAsync(book);
            await _libraryItemRepository.SaveChangesAsync();
            return book.Adapt<BookResponse>();
        }

        public async Task<MagazineResponse> AddMagazineAsync(AddMagazineRequest request)
        {
            Magazine magazine = request.Adapt<Magazine>();
            await _libraryItemRepository.AddAsync(magazine);
            await _libraryItemRepository.SaveChangesAsync();
            return magazine.Adapt<MagazineResponse>();
        }

        public async Task<NewspaperResponse> AddNewsPaperAsync(AddNewspaperRequest request)
        {
           Newspaper newspaper = request.Adapt<Newspaper>();
            await _libraryItemRepository.AddAsync(newspaper);
            await _libraryItemRepository.SaveChangesAsync();
            return newspaper.Adapt<NewspaperResponse>();
        }

        public async Task<DeleteLibraryItemResult> DeleteLibraryItemAsync(string id)
        {
             LibraryItem libraryItem = await _libraryItemRepository.GetByIdAsync(id);
            if (libraryItem is null)
            {
                return DeleteLibraryItemResult.NotFound;
            }
            bool hasBorrowTransactionsHistory = await _libraryItemRepository.HasBorrowTransactionsAsync(id);
            if (hasBorrowTransactionsHistory)
            {
                return DeleteLibraryItemResult.HasBorrowingHistory;
            }
            else
            {
                _libraryItemRepository.Delete(libraryItem);
                await _libraryItemRepository.SaveChangesAsync();
                return DeleteLibraryItemResult.Deleted;
            }
        }

        public async Task<IEnumerable<LibraryItemResponse>> GetAllLibraryItemsAsync()
        {
           IEnumerable<LibraryItem> libraryItems = await _libraryItemRepository.GetAllAsync();
            return libraryItems.Select(item => new LibraryItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                Author = item.Author,
                YearOfPublication = item.YearOfPublication,
                IsAvailable = item.IsAvailable,
                Type = item.GetType().Name // book , magazine , newspaper
            });
        }

        public async Task<LibraryItemResponse?> GetLibraryItemByIdAsync(string id)
        {
           LibraryItem? libraryItem = await _libraryItemRepository.GetByIdAsync(id);
            if (libraryItem is null)
            {
                return null;
            }
            LibraryItemResponse response = new LibraryItemResponse
            {
                Author = libraryItem.Author,
                Id = libraryItem.Id,
                Title = libraryItem.Title,
                IsAvailable = libraryItem.IsAvailable,
                Type = libraryItem.GetType().Name,
                YearOfPublication = libraryItem.YearOfPublication
            };
            return response;

        }

        public async Task<BookResponse?> UpdateBookAsync(string id, UpdateBookRequest request)
        {
            LibraryItem? libraryItem = await _libraryItemRepository.GetByIdAsync(id);
            if(libraryItem is not Book book)
            {
                return null;
            }
             request.Adapt(book); // adaptation of the request on book  
            _libraryItemRepository.Update(book);
            await _libraryItemRepository.SaveChangesAsync();
            return book.Adapt<BookResponse>();
        }

        public async Task<MagazineResponse?> UpdateMagazineAsync(string id, UpdateMagazineRequest request)
        {
            LibraryItem? libraryItem =await _libraryItemRepository.GetByIdAsync(id);
            if (libraryItem is not Magazine magazine)
            {
                return null;
            }
            // requist(Dto) => magazine(Entity)
            request.Adapt(magazine); // adaptation of the request on magazine
            _libraryItemRepository.Update(magazine);
            await _libraryItemRepository.SaveChangesAsync();
            // magazine(Entity) => magazineResponse(Dto)
            return magazine.Adapt<MagazineResponse>();
        }

        public async Task<NewspaperResponse?> UpdateNewsPaperAsync(string id, UpdateNewspaperRequest request)
        {
            LibraryItem? libraryItem = await _libraryItemRepository.GetByIdAsync(id);
            if (libraryItem is not Newspaper newspaper)
            {
                return null;
            }
            // requist(Dto) => newspaper(Entity)
            request.Adapt(newspaper); // adaptation of the request on magazine
            _libraryItemRepository.Update(newspaper);
            await _libraryItemRepository.SaveChangesAsync();
            // newspaper(Entity) => NewspaperResponse(Dto)
            return newspaper.Adapt<NewspaperResponse>();
        }
    }
}
