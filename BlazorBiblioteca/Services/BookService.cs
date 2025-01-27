using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BlazorBiblioteca.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            try
            {
                var books = await _httpClient.GetFromJsonAsync<List<Book>>("books");
                return books ?? new List<Book>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener la lista de libros: {ex.Message}");
                throw new ApplicationException("Error al obtener los libros. Intente nuevamente.");
            }
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            try
            {
                var book = await _httpClient.GetFromJsonAsync<Book>($"books/{id}");
                if (book == null)
                    throw new KeyNotFoundException($"No se encontró un libro con el ID {id}.");

                return book;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener el libro con ID {id}: {ex.Message}");
                throw new ApplicationException($"No se pudo obtener el libro con ID {id}. Verifique la conexión y vuelva a intentarlo.");
            }
        }

        public async Task CreateBookAsync(Book book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || string.IsNullOrWhiteSpace(book.Author))
                throw new ArgumentException("El título y el autor son campos obligatorios.");

            try
            {
                var response = await _httpClient.PostAsJsonAsync("books", book);
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Error al crear el libro: {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el libro: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateBookAsync(Book book)
        {
            if (book.Id <= 0)
                throw new ArgumentException("El ID del libro no es válido.");

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"books/{book.Id}", book);
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Error al actualizar el libro: {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el libro: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del libro no es válido.");

            try
            {
                var response = await _httpClient.DeleteAsync($"books/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Error al eliminar el libro: {errorDetails}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el libro con ID {id}: {ex.Message}");
                throw;
            }
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Pages { get; set; }
        public DateTime PublishDate { get; set; }
    }
}