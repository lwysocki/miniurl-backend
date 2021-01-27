﻿using System;

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

        public long Decode(string key)
        {
            long number = 0;

            for (int i = 0; i < key.Length; i++)
            {
                number = number * AlphabetSize + _alphabet.IndexOf(key[i]);
            }

            return number - _offset;
        }
    }
}
