using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount" };
            //LoggInformation
            logger.LogInformation("Discount Service: GetDiscount: {ProductName} - {Amount}", coupon.ProductName, coupon.Amount);

            //Transforma el objeto Coupon para CouponModel
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }


        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid required coupon"));

            await dbContext.Coupons.AddAsync(coupon);
            var success = await dbContext.SaveChangesAsync();
            if (success == 0)
                throw new RpcException(new Status(StatusCode.Internal, "Error creating coupon"));
            //LoggInformation
            logger.LogInformation("Discount Service: CreateDiscount: {ProductName} - {Amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid required coupon"));

            dbContext.Coupons.Update(coupon);
            var success = await dbContext.SaveChangesAsync();
            if (success== 0)
                throw new RpcException(new Status(StatusCode.Internal, "Error updating coupon"));

            //LoggInformation
            logger.LogInformation("Discount Service: UpdateDiscount: {ProductName} - {Amount}", coupon.ProductName, coupon.Amount);

            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;

        }


        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Coupon with name {request.ProductName}, not Found"));

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            //LoggInformation
            logger.LogInformation("Discount Service: DeleteDiscount: {ProductName} Deleted", coupon.ProductName);

            return new DeleteDiscountResponse { Success = true };

        }




    }

}
