using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Backend
{
    public class IgnoreJsonAttributesResolver : DefaultContractResolver
    {
        public Dictionary<string, HashSet<string>> PropertiesToSkip { get; set; }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
            foreach (var prop in props)
            {
                if (!PropertiesToSkip.ContainsKey(type.Name) || !PropertiesToSkip[type.Name].Contains(prop.PropertyName))
                    prop.Ignored = false;   // Ignore [JsonIgnore]
                else
                    prop.Ignored = true;
            }
            return props;
        }
    }
}
