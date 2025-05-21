using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);


    //Crea las condiciones para la Validacion
    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }


        internal class StoreBasketCommandHandler(
            IBasketRepository _repository, 
            DiscountProtoService.DiscountProtoServiceClient discountProto
            )
                : ICommandHandler<StoreBasketCommand, StoreBasketResult>
        {
            public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
            {
                //Communicate with Discount.Grpc and calculate latest prices of products with discounts
                await DeductDiscount(command.Cart, command);

                var basket = await _repository.UpdateBasket(command.Cart, cancellationToken);

                return new StoreBasketResult(basket.UserName);

            }

            private async Task DeductDiscount(ShoppingCart cart, StoreBasketCommand command)
            {
                foreach (var item in cart.Items)
                {
                    var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
                    //COMUNICACION GRPC ENTRE MICROSERVICIOS
                    var discountReply = await discountProto.GetDiscountAsync(discountRequest);
                    item.Price -= discountReply.Amount;
                }
            }

        }

    }
}