using System;
using Newtonsoft.Json;

namespace Infura.SDK.Common
{
    /// <summary>
    /// A class that represents the OpenSea Metadata Standard Attribute
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// The type of trait this is. This also acts as the name of this attribute
        /// </summary>
        [JsonProperty("trait_type")]
        public string TraitType { get; set; }
        
        /// <summary>
        /// The value for this attribute. This can be a string, number, or boolean
        /// </summary>
        [JsonProperty("value")]
        public object Value { get; set; }
        
        /// <summary>
        /// The type indicating how you would like the attribute to be displayed.
        /// For string traits you may keep as null.
        /// </summary>
        [JsonProperty("display_type", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayTypeRaw { get; set; }
        
        /// <summary>
        /// The type indicating how you would like the attribute to be displayed.
        /// For string traits you may keep as null.
        /// </summary>
        [JsonIgnore]
        public DisplayTypes? DisplayType
        {
            get
            {
                if (string.IsNullOrEmpty(DisplayTypeRaw))
                {
                    return null;
                }

                return StringToDisplayType(DisplayTypeRaw);
            }
            set
            {
                DisplayTypeRaw = value != null ? DisplayTypeToString((DisplayTypes) value) : null;
            }
        }
        
        /// <summary>
        /// The maximum value for this attribute. This is only used for number traits.
        /// </summary>
        [JsonProperty("max_value", NullValueHandling = NullValueHandling.Ignore)]
        public double? MaxValue { get; set; }

        /// <summary>
        /// Create a new integer Attribute with a given trait type and value. You may optionally specify a display type and max value.
        /// </summary>
        /// <param name="traitType">The trait type for this attribute</param>
        /// <param name="value">The int value for this attribute</param>
        /// <param name="displayType">The display type for this attribute. This is optional and defaults to null</param>
        /// <param name="max_value">The max value for this attribute. This is optional and defaults to null</param>
        public Attribute(string traitType, int value, DisplayTypes? displayType = null, int? max_value = null)
        {
            DisplayType = displayType;
            TraitType = traitType;
            Value = value;
            MaxValue = max_value;
        }
        
        /// <summary>
        /// Create a new float Attribute with a given trait type and value. You may optionally specify a display type and max value.
        /// </summary>
        /// <param name="traitType">The trait type for this attribute</param>
        /// <param name="value">The float value for this attribute</param>
        /// <param name="displayType">The display type for this attribute. This is optional and defaults to null</param>
        /// <param name="max_value">The max value for this attribute. This is optional and defaults to null</param>
        public Attribute(string traitType, float value, DisplayTypes? displayType = null, float? max_value = null)
        {
            DisplayType = displayType;
            TraitType = traitType;
            Value = value;
            MaxValue = max_value;
        }
        
        /// <summary>
        /// Create a new double Attribute with a given trait type and value. You may optionally specify a display type and max value.
        /// </summary>
        /// <param name="traitType">The trait type for this attribute</param>
        /// <param name="value">The double value for this attribute</param>
        /// <param name="displayType">The display type for this attribute. This is optional and defaults to null</param>
        /// <param name="max_value">The max value for this attribute. This is optional and defaults to null</param>
        public Attribute(string traitType, double value, DisplayTypes? displayType = null, double? max_value = null)
        {
            DisplayType = displayType;
            TraitType = traitType;
            Value = value;
            MaxValue = max_value;
        }
        
        /// <summary>
        /// Create a new date Attribute with a given trait type and date.
        /// </summary>
        /// <param name="traitType">The trait type for this attribute</param>
        /// <param name="date">The date value for this attribute</param>
        public Attribute(string traitType, DateTime date)
        {
            DisplayType = DisplayTypes.Date;
            TraitType = traitType;
            Value = ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        
        /// <summary>
        /// Create a new string Attribute with a given trait type and string value.
        /// </summary>
        /// <param name="traitType">The trait type for this attribute</param>
        /// <param name="value">The string value for this attribute</param>
        public Attribute(string traitType, string value)
        {
            TraitType = traitType;
            Value = value;
        }

        private string DisplayTypeToString(DisplayTypes displayType)
        {
            switch (displayType)
            {
                case DisplayTypes.Date:
                    return "date";
                case DisplayTypes.BoostNumber:
                    return "boost_number";
                case DisplayTypes.BoostPercentage:
                    return "boost_percentage";
                default:
                    throw new ArgumentException("Invalid DisplayTypes");
            }
        }
        
        private DisplayTypes StringToDisplayType(string displayType)
        {
            switch (displayType)
            {
                case "date":
                    return DisplayTypes.Date;
                case "boost_number":
                    return DisplayTypes.BoostNumber;
                case "boost_percentage":
                    return DisplayTypes.BoostPercentage;
                default:
                    throw new ArgumentException("Invalid DisplayTypes");
            }
        }
    }
}