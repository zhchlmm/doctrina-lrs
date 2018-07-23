using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using xAPI.Core.Converters;

namespace xAPI.Core.Models
{
    [JsonConverter(typeof(ExtensionsConverter))]
    public class Extensions : Dictionary<Uri, object>
    {
        //public void Add(string str, object obj)
        //{
        //    this.Add(new Uri(str), obj);
        //}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson()
        {
            return this.ToString();
        }
    }
}
