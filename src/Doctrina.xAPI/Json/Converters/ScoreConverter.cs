﻿using Doctrina.xAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Doctrina.xAPI.Json.Converters
{
    public class ScoreConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            var score = new Score();
            serializer.Populate(reader, score);

            if (score.Raw < score.Min)
            {
                throw new JsonSerializationException($"Result 'Raw' cannot be less than 'Min'.");
            }

            if (score.Raw > score.Max)
            {
                throw new JsonSerializationException($"Result 'Raw' cannot be greather than 'Max'.");
            }

            if (score.Min > score.Max)
            {
                throw new JsonSerializationException($"Result 'Min' cannot be greather than 'Max'.");
            }

            return score;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Score);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}