using System.Collections.Generic;

namespace ThermometerTest
{
    /// <summary>
    /// Thermometer representation
    /// </summary>
    public class ThermometerModel
    {
        /// <summary>
        /// Used to check if the last input was in a valid temperature format
        /// </summary>
        public bool ValidEntry { get; set; }
        public string TemperatureType { get; set; }
        public Dictionary<string, double> ValidTypes = new Dictionary<string, double> { { "F", 0 }, { "C", 32 }, { "K", 0 } };

        private double _temperature { get; set; }
        public double Temperature
        {
            get
            {
                return this._temperature;
            }
            set { this._temperature = this.converter(value); }
        }

        public bool? Direction { get; set; }

        private double _freezing { get; set; }
        public double Freezing
        {
            get { return this._freezing; }
            set
            {
                this._freezing = converter(value);
            }
        }

        private double _fluctuation { get; set; }
        public double Fluctuation
        {
            get { return this._fluctuation; }
            set
            {
                this._fluctuation = converter(value, ValidTypes[this.TemperatureType]);
            }
        }

        private double _boiling { get; set; }
        public double Boiling
        {
            get { return this._boiling; }
            set
            {
                this._boiling = converter(value);
            }
        }

        /// <summary>
        /// Convert celsius to farenheit
        /// this program only store values in farenheit but can work with celsius as well
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private double converter(double v, double coefficient = 0)
        {
            switch (this.TemperatureType)
            {
                // to Farenheit
                case "C":
                    return v * 1.8 + 32 - coefficient;
                // to Kelvin
                case "K":
                    return v * 1.8 - 459.67;
                default:
                    return v;
            }
        }

        /// <summary>
        /// Indicate if the temperature reached the threshold limited by the fluctuation range
        /// and sets the direction of the temperature: false = freezing limits, true = boiling limits, null = out of thresholds
        /// </summary>
        /// <returns></returns>
        public bool TriggerAlert()
        {
            if (!ValidEntry) return false;

            bool result = false;
            this.Direction = null;

            if ((this.Temperature >= this.Freezing - this.Fluctuation) &&
                    (this.Temperature <= this.Freezing + this.Fluctuation))
            {
                this.Direction = false;
                result = true;
            }
            else if ((this.Temperature >= this.Boiling - this.Fluctuation) &&
                (this.Temperature <= this.Boiling + this.Fluctuation))
            {
                this.Direction = true;
                result = true;
            }

            return result;
        }
    }
}
