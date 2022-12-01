using System;
using Newtonsoft.Json;

namespace Infura.SDK
{
    public class Attribute
    {
        [JsonProperty("trait_type")]
        public string TraitType { get; set; }
        
        [JsonProperty("value")]
        public object Value { get; set; }
        
        [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayType { get; set; }
        
        [JsonProperty("max_value", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxValue { get; set; }

        public Attribute(string traitType, int value, DisplayType? displayType = null, int? max_value = null)
        {
            DisplayType = displayType != null ? DisplayTypeToString((DisplayType) displayType) : null;
            TraitType = traitType;
            Value = value;
            MaxValue = max_value;
        }
        
        public Attribute(string traitType, DateTime date)
        {
            DisplayType = DisplayTypeToString(SDK.DisplayType.Date);
            TraitType = traitType;
            Value = ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        public Attribute(string traitType, string value)
        {
            TraitType = traitType;
            Value = value;
        }

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