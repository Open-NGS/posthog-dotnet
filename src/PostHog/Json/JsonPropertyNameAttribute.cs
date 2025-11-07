using System;
using System.Collections.Generic;
using System.Text;

namespace PostHog.Json
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class JsonPropertyNameAttribute : Attribute
    {
        //
        // 摘要:
        //     Gets the name of the property.
        //
        // 返回结果:
        //     The name of the property.
        public string Name { get; }

        //
        // 摘要:
        //     Initializes a new instance of System.Text.Json.Serialization.JsonPropertyNameAttribute
        //     with the specified property name.
        //
        // 参数:
        //   name:
        //     The name of the property.
        public JsonPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}
