//-----------------------------------------------------------------------
// <copyright file="Vector2Converter.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace OpenTK.SpriteManager
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OpenTK;

    /// <summary>
    /// Json converter for <see cref="Vector2"/>.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class Vector2Converter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Vector2);
        }

        /// <summary>
        /// Reads the json for the <see cref="Vector2" type/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value.</param>
        /// <param name="serializer">The serializer.</param>
        /// <returns>The <see cref="Vector2"/>.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            var result = new Vector2();

            if (token["X"] != null)
                result.X = (float)token["X"];
            if (token["Y"] != null)
                result.Y = (float)token["Y"];

            return result;
        }

        /// <summary>
        /// Writes the json for the <see cref="Vector2" type/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var vector = (Vector2)value;

            writer.WriteStartObject();

            writer.WritePropertyName("X");
            writer.WriteValue(vector.X);

            writer.WritePropertyName("Y");
            writer.WriteValue(vector.Y);

            writer.WriteEndObject();
        }
    }
}
