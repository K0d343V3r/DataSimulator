using DataSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Helpers
{
    public class DiscreteGenerator : DataGenerator
    {
        private readonly bool _periodic;

        public DiscreteGenerator(bool periodic)
        {
            _periodic = periodic;
        }

        protected override object GetValueAtTime(DateTime time)
        {
            if (_periodic)
            {
                // Reference: https://en.wikipedia.org/wiki/Pulse_wave
                NumericScale scale = new NumericScale(-1, 1);
                var generator = new WaveFormGenerator(WaveForm.Sawtooth, scale);

                // use a 25% duty cycle (25% of 2 = 0.5)
                return generator.GetValue(time, 20) >= 0.5f;
            }
            else
            {
                // Reference: https://en.wikipedia.org/wiki/Pulse-width_modulation
                NumericScale scale = new NumericScale(0, 100);
                var sawToothGenerator = new WaveFormGenerator(WaveForm.Sawtooth, scale);
                var sineGenerator = new WaveFormGenerator(WaveForm.Sine, scale);
                return sawToothGenerator.GetValue(time) < sineGenerator.GetValue(time);
            }
        }
    }
}
