using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Countries_WPF.Models
{
    public class Country
    {
        /// <summary>
        /// Returns the path of the flag's, if one o more flags files nor exists, show the default one "No Image Available"
        /// </summary>
        [JsonIgnore]
        private String _bandeira;

        [JsonIgnore]
        public String Bandeira
        {
            get
            {
                string flag1 = Directory.GetCurrentDirectory() + "\\Flags\\" + _name.Common + ".png";
                if (File.Exists(flag1))
                {
                    _bandeira = Directory.GetCurrentDirectory() + "\\Flags\\" + _name.Common + ".png";
                    return _bandeira;
                }
                else
                {
                    _bandeira = Directory.GetCurrentDirectory() + "\\Flags\\0_Img_NotAvl.png";
                    return _bandeira;
                }
            }
            set
            {
                _bandeira = value;
            }
        }

        /// <summary>
        /// Convert te Atribute _name to String before the (get; set; ) in Name Property
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("name")]
        private Name _name;

        [JsonIgnore]
        public string Name
        {
            get
            {
                if (_name.Common == null)
                {
                    return "not available";
                }
                else
                {
                    return _name.Common; ;
                }
            }
            set
            {
                if (value != null)
                {
                    _name = new Name { Common = value };
                }
            }
        }

        /// <summary>
        /// get te Atribute _capital withe index 0. to String Property Capital
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("capital")]
        private List<string> _capital;

        [JsonIgnore]
        public string Capital
        {
            get
            {
                if (_capital != null)
                {
                    if (!string.IsNullOrWhiteSpace(_capital[0]))
                    {
                        return _capital[0];
                    }
                    else
                    {
                        return "not available";
                    }
                }
                else
                {
                    return "not available";
                }
            }
            set
            {
                if (value != null)
                {
                    _capital = new List<string>();
                    _capital.Add(value);

                }
            }
        }

        /// <summary>
        /// Get and Set the Atribute _region in Propert Region.
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("region")]
        private String _region;
        [JsonIgnore]
        public String Region
        {
            get
            {
                if (_region == null)
                {
                    return "not available";
                }
                else
                {
                    return _region;
                }
            }
            set
            {
                _region = value;
            }
        }

        /// <summary>
        /// Get and Set the Atribute _subRegion in Property SubRegion.
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("subregion")]
        private String _subregion;
        [JsonIgnore]
        public String Subregion
        {
            get
            {
                if (_subregion == null)
                {
                    return "not available";
                }
                else
                {
                    return _subregion;
                }
            }
            set
            {
                _subregion = value;
            }
        }

        /// <summary>
        /// Get and Set the Atribute _population in Property Population.
        /// Convert to String before send the property to the interface
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("population")]
        private double? _population;
        /// <summary>
        /// Poputation have 0's but no Nulls
        /// </summary>
        [JsonIgnore]
        public string Population
        {
            get
            {
                if (_population == null)
                {
                    return "not available";
                }
                else
                {
                    return _population.ToString();
                }
            }
            set
            {
                _population = Convert.ToUInt32(value);
            }
        }

        /// <summary>
        /// Get and Set the Atribute _gine in Property Gine. From Classe Gini.cs
        /// Convert from double to String before send the property to the interface
        /// And If not Exists send to Interface the value "not available"
        /// </summary>
        [JsonProperty("gini")]
        private Gini _gini;
        // public Gini Gini { get; set; }
        [JsonIgnore]
        public String Gini
        {
            get
            {
                if (_gini != null)
                {
                    return _gini.ToString();
                }
                else
                {
                    return "not available";
                }
            }
            set
            {
                if (value != null)
                {
                    _gini = new Gini { giniYear = Convert.ToDouble(value) };
                }
            }
        }

        /// <summary>
        /// Return the Web path for the Country Flags
        /// </summary>
        public Flags Flags { get; set; }


        /// <summary>
        /// Retun the Overrid to string the name of country
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{_name.Common}";
        }



    }
}
