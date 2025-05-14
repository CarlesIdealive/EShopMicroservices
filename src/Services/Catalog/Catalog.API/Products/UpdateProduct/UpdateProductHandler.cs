using FluentValidation;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, string ImageFile, List<string> Categories)
        : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);


    //CLASE IMPLEMENTS VALIDADOR del tipo UpdateProductCommand
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Identifier is required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }

    }



    internal class UpdateProductCommandHandler(
        IDocumentSession session
    ) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
                if (product is null)
                {
                    throw new ProductNotFoundException(command.Id);
                }
                product.Name = command.Name;
                product.Description = command.Description;
                product.Price = command.Price;
                product.ImageFile = command.ImageFile;
                product.Categories = command.Categories;

                session.Update(product);
                await session.SaveChangesAsync(cancellationToken);
                return new UpdateProductResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while updating product", ex);
            }
        }
    }
}
