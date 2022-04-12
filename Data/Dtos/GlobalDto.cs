using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Dtos
{
    public class FilterDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int ValueId { get; set; }
    }

    public class PaginatorDto<T>
    {
        public PaginatorDto()
        {
            Items = new List<T>();
            Filters = new List<FilterDto>();
        }
        public int Length { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; }
        public bool HasFilter { get; set; }
        public List<FilterDto> Filters { get; set; }
    }
    public class RepositoryResult<T>
    {
       
        public RepositoryResult(int statusCode, T data, string message)
        {
            this.Data = data;
            this.StatusCode = statusCode;
            this.Message = message;
        }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class RepositoryResult
    {

        public RepositoryResult(int statusCode,string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }

}
