using DataSimulator.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSimulator.Api.Services.DataGenerator
{
    public class WaveFormGenerator : DataGenerator
    {
        private readonly WaveForm _waveForm;
        private readonly NumericScale _scale;
        private readonly Random _random = new Random();

        public WaveFormGenerator(WaveForm waveForm, NumericScale scale)
        {
            _waveForm = waveForm;
            _scale = scale;
        }

        protected override object GetValueAtTime(DateTime date)
        {
            return GetValue(date);
        }

        internal float GetValue(DateTime date)
        {
            return GetValue(date, 60);
        }

        // Adapted from:
        // https://www.codeproject.com/Articles/30180/Simple-Signal-Generator
        // References:
        // http://en.wikipedia.org/wiki/Waveform
        // http://en.wikipedia.org/wiki/White_noise
        internal float GetValue(DateTime date, int secondsPerCycle)
        {
            double t = (1f / secondsPerCycle) * (double)(date.Ticks / TimeSpan.TicksPerSecond);
            double value = 0f;
            switch (_waveForm)
            {
                case WaveForm.Sine:         // sin( 2 * pi * t )
                    value = Math.Sin(2f * Math.PI * t);
                    break;

                case WaveForm.Square:       // sign( sin( 2 * pi * t ) )
                    value = Math.Sign(Math.Sin(2f * Math.PI * t));
                    break;

                case WaveForm.Triangle:     // 2 * abs( t - 2 * floor( t / 2 ) - 1 ) - 1
                    value = 1f - 4f * Math.Abs(Math.Round(t - 0.25f) - (t - 0.25f));
                    break;

                case WaveForm.Sawtooth:     // 2 * ( t/a - floor( t/a + 1/2 ) )
                    value = 2f * (t - Math.Floor(t + 0.5f));
                    break;

                case WaveForm.WhiteNoise:
                    value = 2f * (double)_random.Next(int.MaxValue) / int.MaxValue - 1f;
                    break;
            }

            return (float)(_scale.Min + ((value + 1) / 2) * (_scale.Max - _scale.Min));
        }
    }
}
