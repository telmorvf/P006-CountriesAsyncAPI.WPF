using Newtonsoft.Json;

namespace Countries_WPF.Models
{
    /// <summary>
    /// All the years, where are possible have one with the Gini Value 
    /// </summary>
    public class Gini
    {
        [JsonProperty("2015")]
        public double? _2015 { get; set; }

        [JsonProperty("2018")]
        public double? _2018 { get; set; }

        [JsonProperty("2012")]
        public double? _2012 { get; set; }

        [JsonProperty("2010")]
        public double? _2010 { get; set; }

        [JsonProperty("1998")]
        public double? _1998 { get; set; }

        [JsonProperty("2019")]
        public double? _2019 { get; set; }

        [JsonProperty("2011")]
        public double? _2011 { get; set; }

        [JsonProperty("2017")]
        public double? _2017 { get; set; }

        [JsonProperty("1992")]
        public double? _1992 { get; set; }

        [JsonProperty("2008")]
        public double? _2008 { get; set; }

        [JsonProperty("2014")]
        public double? _2014 { get; set; }

        [JsonProperty("2016")]
        public double? _2016 { get; set; }

        [JsonProperty("2013")]
        public double? _2013 { get; set; }

        [JsonProperty("1999")]
        public double? _1999 { get; set; }

        [JsonProperty("2004")]
        public double? _2004 { get; set; }

        [JsonProperty("2006")]
        public double? _2006 { get; set; }

        [JsonProperty("2003")]
        public double? _2003 { get; set; }

        [JsonProperty("2009")]
        public double? _2009 { get; set; }

        [JsonProperty("2005")]
        public double? _2005 { get; set; }

        [JsonIgnore]
        public double? giniYear { get; set; }


        /// <summary>
        /// If any property of Gini are != null, return the value to the property GiniYear to de Class Country.Gini
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            foreach (var giniYear in this.GetType().GetProperties())
            {
                if (giniYear.GetValue(this) != null)
                {
                    return giniYear.GetValue(this).ToString(); // return Gini._Ano do Proprerty giniYear
                }
            }
            return "not available1";
        }

    }
}
