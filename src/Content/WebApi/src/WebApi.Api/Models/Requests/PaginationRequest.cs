using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Models.Requests
{
    [Serializable]
    public class PaginationRequest : IValidatableObject
    {
        internal const string PageParameterName = "_page";
        internal const string SizeParameterName = "_size";
        internal const int MinOffset = 1;
        internal const int MinRecordsPerPage = 1;
        internal const int MaxRecordsPerPage = 50;

        /// <summary>Request page number.</summary>
        /// <value>1</value>
        [Required(ErrorMessage = "The field _page is required")]
        [FromQuery(Name = PageParameterName)]
        public int? PageNumber { get; init; }

        /// <summary>Max register count per page.</summary>
        /// <value>10</value>
        [Required(ErrorMessage = "The field _size is required")]
        [FromQuery(Name = SizeParameterName)]
        public int? RecordPerPage { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber < MinOffset)
            {
                yield return new ValidationResult(
                    $"Parameter {PageParameterName} must be greather or equal than {MinOffset}",
                    new[] { nameof(PageNumber) });
            }

            if (RecordPerPage > MaxRecordsPerPage || RecordPerPage < MinRecordsPerPage)
            {
                yield return new ValidationResult(
                    $"Parameter {SizeParameterName} must be specified between {MinRecordsPerPage} and {MaxRecordsPerPage}.",
                    new[] { nameof(RecordPerPage) });
            }
        }
    }
}
