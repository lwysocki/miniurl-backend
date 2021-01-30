using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain
{
    public class KeysGenerator
    {
        public long Iteration { get; private set; }

        public KeysGenerator(long iteration = 0)
        {
            Iteration = iteration;
        }

        public IList<long> Generate(long maxValue, long step)
        {
            IList<long> keys = new List<long>();
            long current = ++Iteration;

            while (current < maxValue)
            {
                keys.Add(current);
                current += step;
            }

            return keys;
        }
    }
}
