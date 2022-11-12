using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public  async Task<bool> CreateDiscount(Coupon coupon)
        {

            using var connection = new NpgsqlConnection
                          (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon(Productname, Description, Amount) VALUES(@Productname, @Description, @Amount)", 
                new { ProductName = coupon.ProductName, Amount = coupon.Amount, Description = coupon.Description });

            if (affected == 0)
            {
                return false;
            }
            return true ;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {

            using var connection = new NpgsqlConnection
                                      (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("DELETE  FROM Coupon WHERE ProductName = @Productname",
                new { ProductName = productName });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection
                (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName= @ProductName", new { ProductName = productName });

            if (coupon == null)
            {
                return new Coupon() { ProductName ="No Discount" , Amount=0,Description="No discount description"};
            }
            return coupon;
        }

        public async  Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection
                                      (_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET Productname = @Productname, Description = @Description, Amount=@Amount  WHERE Id = @Id",
                new { ProductName = coupon.ProductName, Amount = coupon.Amount, Description = coupon.Description ,ID=coupon.Id });

            if (affected == 0)
            {
                return false;
            }
            return true;
        }
    }
}
