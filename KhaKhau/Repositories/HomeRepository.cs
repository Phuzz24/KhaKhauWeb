
//namespace KhaKhau.Repositories
//{
//    public class HomeRepository
//    {
//        private readonly KhaKhauContext _context;
//        public HomeRepository(KhaKhauContext context)
//        {
//            _context = context; 
//        }
//        public async Task<IEnumerable<Product>> GetProduct(string sTerm="",int categoryId=0)
//        {
//            sTerm = sTerm.ToLower();
//            IEnumerable<Product> products = await (from product in _context.Products
//                                            join genre in _context.Categories
//                                            on product.CategoryId equals genre.Id
//                                            join stock in _db.Stocks
//                                            on book.Id equals stock.BookId
//                                            into book_stocks
//                                            from bookWithStock in book_stocks.DefaultIfEmpty()
//                                            where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
//                                            select new Book
//                                            {
//                                                Id = book.Id,
//                                                Image = book.Image,
//                                                AuthorName = book.AuthorName,
//                                                BookName = book.BookName,
//                                                GenreId = book.GenreId,
//                                                Price = book.Price,
//                                                GenreName = genre.GenreName,
//                                                Quantity = bookWithStock == null ? 0 : bookWithStock.Quantity
//                                            }
//                         ).ToListAsync();
//            if (genreId > 0)
//            {

//                books = books.Where(a => a.GenreId == genreId).ToList();
//            }
//            return books;
//        }
//    }
//}
