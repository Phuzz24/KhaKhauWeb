//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Dapper;
//namespace KhaKhau.Repositories;

////public record TopSoldProductModel(string ProductName, int TotalUnitSold);

//public class ReportRepository : IReportRepository
//{
//    private readonly KhaKhauContext _context;
//    public ReportRepository(KhaKhauContext context)
//    {
//        _context = context;
//    }

//    public async Task<IEnumerable<TopSoldProductModel>> GetTopSoldProductByDate(DateTime startDate, DateTime endDate)
//    {
//        var startDateParam = new SqlParameter("@startDate", startDate);
//        var endDateParam = new SqlParameter("@endDate", endDate);
//        //var topFiveSoldBooks = await _context.Database.
//        //    < TopSoldProductModel > ("exec Usp_GetTopNSellingBooksByDate @startDate,@endDate", startDateParam, endDateParam).ToListAsync();

//        //return topFiveSoldBooks;'

//        var sql = "EXEC Usp_GetTopSellingProByDate @startDate, @endDate";

//        using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
//        {
//            var results = await connection.QueryAsync<TopSoldProductModel>(sql, new { startDate, endDate });
//            Console.WriteLine(results.ToString());
//            return results.ToList();
//        }
//    }

//}

//public interface IReportRepository
//{
//    Task<IEnumerable<TopSoldProductModel>> GetTopSoldProductByDate(DateTime startDate, DateTime endDate);
//}