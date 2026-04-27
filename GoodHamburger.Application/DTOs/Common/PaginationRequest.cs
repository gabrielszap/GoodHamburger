//using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace GoodHamburger.Application.DTOs.Common
{
    public sealed class PaginationRequest
    {
        private const int MaxPageSize = 100;

        public int Page { get; init; } = 1;

        public int PageSize { get; init; } = 10;

        //[SwaggerSchema(ReadOnly = true)]
        [JsonIgnore]
        public int Skip => (Page - 1) * PageSize;

        //[SwaggerSchema(ReadOnly = true)]
        [JsonIgnore]
        public int Take => PageSize > MaxPageSize ? MaxPageSize : PageSize;
    }
}
