using System;
using System.Collections.Generic;

namespace MyRentalWebService.Data.Dtos
{
   
    public class RentDto
    {
        public RentDto()
        {
            Items = new List<RentItemDto>();           
        }
        public int Id { get; set; }       
        public string CustomerFullName { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string RentDateTime { get; set; }
        public bool IsTerminated { get; set; }
        public string ReturnDateTime { get; set; }
        public int FinalCost { get; set; }
        public int PredictedCost { get; set; }
        public string Description { get; set; }
        public IEnumerable<RentItemDto> Items { get; set; }

    }
    public class RentItemDto
    {
        public int RentId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int PriceWhenRenting { get; set; }
        public string UnitWhenRenting { get; set; }
    }

    public class RentOperationDto
    {
        public RentOperationDto()
        {
            Items = new List<RentOperationItemDto>();
        }
        public int Id { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string RentDateTime { get; set; }
        public bool IsTerminated { get; set; }
        public string ReturnDateTime { get; set; }
        public int FinalCost { get; set; }
        public int PredictedCost { get; set; }
        public int CalculatedCost { get; set; }
        public string Description { get; set; }
        public IEnumerable<RentOperationItemDto> Items { get; set; }

    }
    public class RentOperationItemDto
    {
        public int RentId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int PriceWhenRenting { get; set; }
        public int RentPrice { get; set; }
        public string UnitWhenRenting { get; set; }
        public string RentUnitText { get; set; }
        public int CalculatedCost { get; set; }
    }
    public class NewRentDto
    {
        public NewRentDto()
        {
            ProductIds = new List<int>();
        }
        public int CustomerId { get; set; }
        public int PredictedCost { get; set; }
        public string Description { get; set; }
        public List<int> ProductIds { get; set; }
    }
    public class CustomerListInNewRentDto { 
    
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
    }

    public class TerminateRentDto
    {
        public int Id { get; set; }
        public int FinalCost { get; set; }
        public bool IsTerminated { get; set; }
    }
}