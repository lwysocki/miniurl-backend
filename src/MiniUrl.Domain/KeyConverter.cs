using System;

namespace MiniUrl.Domain
{
    public class KeyConverter : IKeyConverter
    {
        private static readonly string _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_";

        public int AlphabetSize
        {
            get { return _alphabet.Length; }
        }

        private readonly long _offset;

        public KeyConverter(int keyOrder)
        {
            _offset = (long)Math.Pow(AlphabetSize, keyOrder - 1);
        }

        public string Encode(long number)
        {
            number += _offset;

            var temp = number;
            var size = 0;

            do
            {
                number /= AlphabetSize;
                size++;
            }
            while (number > 0);

            char[] buffer = new char[size];

            do
            {
                int idx = (int)(temp % AlphabetSize);
                char c = _alphabet[idx];

                buffer[--size] = c;
                temp /= AlphabetSize;
            }
            while (temp > 0);

            return new string(buffer);
        }
    }
}
