using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DicountProtoService.DicountProtoServiceClient _dicountProtoServiceClient;

        public DiscountGrpcService(DicountProtoService.DicountProtoServiceClient dicountProtoServiceClient)
        {
            _dicountProtoServiceClient = dicountProtoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName) 
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName};
            return await _dicountProtoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
