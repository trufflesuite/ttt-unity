using System;
using System.Collections.Generic;
using System.Linq;
using Infura.SDK.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Attribute = Infura.SDK.Common.Attribute;

namespace Infura.SDK
{
    /// <summary>
    /// A simple Metadata class that represents the Metadata of an NFT. This
    /// class follows the ERC721 Metadata standard as well as includes OpenSea
    /// attributes and traits. This class can be used for most NFTs.
    /// Any metadata fields that don't fit into a field in this class can
    /// still be obtained using the <see cref="GetExtraData{T}"/> function. Extra
    /// fields can also be added to this class (and will be serialized into proper JSON)
    /// using the <see cref="SetExtraData{T}"/> function.
    /// </summary>
    public class Metadata : IMetadata
    {
        /// <summary>
        /// The name of this NFT
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// The description for this NFT
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The attributes for this NFT
        /// </summary>
        [JsonProperty("attributes")]
        public Attribute[] Attributes { get; set; }
        
        /// <summary>
        /// The Image URL for this NFT
        /// </summary>
        [JsonProperty("image")]
        public string ImageUrl { get; set; }
        
        /// <summary>
        /// The cover image URL for this NFT
        /// </summary>
        [JsonProperty("coverImage")]
        public string CoverImageUrl { get; set; }
        
        [JsonExtensionData]
#pragma warning disable CS0649
        private IDictionary<string, JToken> _extraStuff;
#pragma warning restore CS0649

        /// <summary>
        /// Create a new Metadata instance with the given name and description. You may
        /// also optionally provide an array of attributes.
        /// </summary>
        /// <param name="name">The name of this NFT</param>
        /// <param name="description">The description of this NFT</param>
        /// <param name="attributes">An array of attributes for this NFT (optional)</param>
        public Metadata(string name, string description, Attribute[] attributes = null)
        {
            if (attributes == null)
                attributes = Array.Empty<Attribute>();
            
            Name = name;
            Description = description;
            Attributes = attributes;
        }

        /// <summary>
        /// Add an attribute. This will append to the attribute array
        /// </summary>
        /// <param name="attribute">The new attribute to add to this NFT</param>
        public void AddAttribute(Attribute attribute)
        {
            if (Attributes == null)
                Attributes = Array.Empty<Attribute>();
            
            Attributes = Attributes.Append(attribute).ToArray();
        }
        
        /// <summary>
        /// Get extra data that was not included in the standard metadata fields. This
        /// can be any JSON field that was not included in the standard fields inside this class.
        /// </summary>
        /// <param name="key">The JSON field name to grab</param>
        /// <typeparam name="T">The type of value for this field</typeparam>
        /// <returns>The value as type T, or null/default if the key doesn't exist (or no extra fields exist)</returns>
        public T GetExtraData<T>(string key)
        {
            if (_extraStuff == null || !_extraStuff.ContainsKey(key))
                return default;
            
            return _extraStuff[key].ToObject<T>();
        }

        /// <summary>
        /// Set extra data. This will add a new field to the JSON metadata that is not part of the standard
        /// fields in this class. This can be used to add any extra fields that are not part of the standard fields.
        /// When this class is serialized into JSON, the extra fields will be added as normal fields
        /// </summary>
        /// <param name="key">The field name of the extra data to add</param>
        /// <param name="value">The field value of the extra data</param>
        /// <typeparam name="T">The type of the value to store</typeparam>
        public void SetExtraData<T>(string key, T value)
        {
            _extraStuff ??= new Dictionary<string, JToken>();

            if (_extraStuff.ContainsKey(key))
                _extraStuff.Remove(key);
            
            _extraStuff.Add(key, JToken.FromObject(value));
        }
    }
}