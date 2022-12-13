using System;
using Newtonsoft.Json;

namespace Infura.SDK
{
    /// <summary>
    /// 
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("trait_type")]
        public string TraitType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayType { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("max_value", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="traitType"></param>
        /// <param name="value"></param>
        /// <param name="displayType"></param>
        /// <param name="max_value"></param>
        public Attribute(string traitType, int value, DisplayType? displayType = null, int? max_value = null)
        {
            DisplayType = displayType != null ? DisplayTypeToString((DisplayType) displayType) : null;
            TraitType = traitType;
            Value = value;
            MaxValue = max_value;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traitType"></param>
        /// <param name="date"></param>
        public Attribute(string traitType, DateTime date)
        {
            DisplayType = DisplayTypeToString(SDK.DisplayType.Date);
            TraitType = traitType;
            Value = ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="traitType"></param>
        /// <param name="value"></param>
        public Attribute(string traitType, string value)
        {
            TraitType = traitType;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string DisplayTypeToString(DisplayType displayType)
        {
            switch (displayType)
            {
                case SDK.DisplayType.Date:
                    return "date";
                case SDK.DisplayType.BoostNumber:
                    return "boost_number";
                case SDK.DisplayType.BoostPercentage:
                    return "boost_percentage";
                default:
                    throw new ArgumentException("Invalid DisplayType");
            }
        }
    }
}