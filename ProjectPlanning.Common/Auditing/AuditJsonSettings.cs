using System;
using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProjectPlanning.Common.Auditing
{
    public static class AuditJsonSettings
    {
        private sealed class AuditContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                if (string.Equals(property.PropertyName, "RowVersion", StringComparison.OrdinalIgnoreCase))
                {
                    property.ShouldSerialize = _ => false;
                    return property;
                }

                var propType = property.PropertyType;
                if (propType != null && propType != typeof(string)
                    && (typeof(IEnumerable).IsAssignableFrom(propType) || (propType.IsClass && propType.Namespace != null && propType.Namespace.StartsWith("ProjectPlanning.Entities.Models"))))
                {
                    property.ShouldSerialize = _ => false;
                }

                return property;
            }
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new AuditContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            Formatting = Formatting.None
        };

        public static string Serialize(object value)
        {
            return value == null ? null : JsonConvert.SerializeObject(value, Settings);
        }
    }
}
