/* 
 * Name: Alexandre
 * Last Name: Da silva
 */

using System;
using System.Linq;

namespace ThermometerTest
{
    /// <summary>
    /// A thermometer that read temperature values from console input and shows alerts if the temperature
    /// reaches determined threshold ( must be in the fluctuation range only)
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {

            // This is the defaults values for thresholds
            var thermometer = new ThermometerModel
            {
                TemperatureType = "F",
                Boiling = 212.0,
                Freezing = 32.0,
                Fluctuation = 3.6
            };

            // The default values can be modified in the lines above or during the execution
            // by writing the name of the variable and a new value
            // Examples: Boiling 98 C
            // Fluctuation: 0.5 C
            // by changing the threshold values, all alerts are reseted

            //flag if the alert was shown
            bool alerted = false;
            // flag the direction of the last input, false = freezing, true = boiling, null = middle
            bool? lastDirection = null;

            //input (console only)
            Console.WriteLine("Type 'defaults' to see the initialized values");
            Console.WriteLine("Press Ctrl+Break ou Ctrl+C to exit");

            while (true)
            {
                var inputValues = Console.ReadLine();

                Interpreter(thermometer, inputValues);

                if (thermometer.TriggerAlert())
                {
                    if (!alerted && thermometer.Direction != lastDirection)
                        ShowAlert();
                    else
                        alerted = false;
                }

                lastDirection = thermometer.Direction;
            }
        }

        /// <summary>
        /// Do something related to the alert
        /// </summary>
        static void ShowAlert()
        {
            Console.WriteLine("Alert");
        }

        /// <summary>
        /// Set the defaults or a value for the temperature
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input_string"></param>
        static void Interpreter(ThermometerModel model, string input_string)
        {
            var inputList = input_string.Split(' ');
            double degree = 0;

            if (input_string.ToLower().Equals("defaults"))
                ShowDefaults(model);

            model.ValidEntry = false;
            model.TemperatureType = inputList.Last().ToUpper();
           
            //If the temperature type was not indicated, exit the method
            if (!model.ValidTypes.ContainsKey(model.TemperatureType)) return;

            string[] keywords = { "Boiling", "Freezing", "Fluctuation" };

            if (keywords.Contains(inputList.First()))
            {
                // save the last value before, in case of conversion errors
                var lastValue = (double)model.GetType().GetProperty(inputList.First()).GetValue(model, null);

                // write the new value
                degree = Double.TryParse(inputList[1], out degree) ? degree : lastValue;
                model.GetType().GetProperty(inputList.First()).SetValue(model, degree);

                // reset alerts
                model.Direction = null;
                // show defaults
                ShowDefaults(model);
            }
            else
            {
                model.ValidEntry = Double.TryParse(inputList.First(), out degree);
                model.Temperature = degree;
            }
        }

        /// <summary>
        /// Show the defaults values in the screen
        /// </summary>
        /// <param name="model"></param>
        static void ShowDefaults(ThermometerModel model)
        {
            Console.WriteLine("Boiling value is set to: {0}° F", model.Boiling);
            Console.WriteLine("Freezing value is set to: {0}° F", model.Freezing);
            Console.WriteLine("Fluctuation value is set to: {0}° F", model.Fluctuation);
        }
    }
}
