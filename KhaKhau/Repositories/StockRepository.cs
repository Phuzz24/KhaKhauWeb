using Microsoft.EntityFrameworkCore;

namespace KhaKhau.Repositories
{
    public class StockRepository: IStockRepository
    {
        private readonly KhaKhauContext _context;
        public StockRepository(KhaKhauContext context)
        {
            _context = context;

        }
        public async Task<Stock?> GetStockByProductId(int productId)
       => await _context.Stocks.FirstOrDefaultAsync(s => s.Productid == productId);


        public async Task ManageStock(StockDTO stockToManage)
        {
            // if there is no stock for given book id, then add new record
            // if there is already stock for given book id, update stock's quantity
            var existingStock = await GetStockByProductId(stockToManage.ProductId);
            if (existingStock is null)
            {
                var stock = new Stock { Productid = stockToManage.ProductId, Quantity = stockToManage.Quantity };
                _context.Stocks.Add(stock);
            }
            else
            {
                existingStock.Quantity = stockToManage.Quantity;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from product in _context.Products
                                join stock   in   _context.Stocks
                                on product.Id equals stock.Productid
                                into product_stock
                                from productStock in product_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || product.Name.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    ProductId = product.Id,
                                    ProductName = product.Name,
                                    Quantity = productStock == null ? 0 : productStock.Quantity
                                }
                                ).ToListAsync();
            return stocks;
        }

       
    }

    public interface IStockRepository
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByProductId(int productId);
        Task ManageStock(StockDTO stockToManage);
    }
}