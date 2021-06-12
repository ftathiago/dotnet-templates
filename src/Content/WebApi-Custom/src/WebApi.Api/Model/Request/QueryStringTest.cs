#nullable enable
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Api.Model.Request
{
    public class QueryStringTest
    {
        /// <summary>
        /// This is just a example of what you can do documenting objects.
        /// </summary>
        /// <example>b3ff45e3-1da2-4a18-8333-4566796b4696</example>
        public Guid? Id { get; set; }

        /// <summary>
        /// It a example of validation.
        /// </summary>
        /// <example>9</example>
        [Range(1, 10)]
        public int QueryNumber { get; set; }

        /// <summary>
        /// Unfortunately you can not generate examples dynamically. For this, you should use another example mechanism.
        /// </summary>
        /// <example>A required field should be nullable.</example>
        [Required]
        public string? RequiredField { get; set; }
    }
}
