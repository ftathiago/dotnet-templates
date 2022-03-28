using System;

namespace WebApi.EfInfraData.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    internal sealed class OrderedByAttribute : Attribute
    {
        public OrderedByAttribute(string jsonPropertyName) =>
            JsonPropertyName = jsonPropertyName;

        public string JsonPropertyName { get; }
    }
}
