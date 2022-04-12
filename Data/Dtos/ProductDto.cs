using System.ComponentModel.DataAnnotations;

namespace MyRentalWebService.Data.Dtos
{
    

    public class ProductItemDto{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int RentPrice { get; set; }
        public int RentUnit { get; set; }
        public string RentUnitText { get; set; }
        public int MinUnitForRent { get; set; }
        public int MaxUnitForRent { get; set; }
    }
    public class CreateProductDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int RentPrice { get; set; }
        public int RentUnit { get; set; }
    }
    public class UpdateProductItemDto{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int RentPrice { get; set; }
        public int RentUnit { get; set; }
    }

     public class ChangeProductActivationDto{
        public int Id { get; set; }
    }
}