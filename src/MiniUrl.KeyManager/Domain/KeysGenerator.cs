using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain
{
    public class KeysGenerator
    {
        public long Iteration { get; private set; }
        public long MaxValue { get; private set; }
        public long Step { get; private set; }

        public KeysGenerator(long maxValue, long step, long iteration = 0)
        {
            Iteration = iteration;
            MaxValue = maxValue;
            Step = step;
        }

        public IList<long> Generate()
        {
            IList<long> keys = new List<long>();
            long current = ++Iteration;

            while (current < MaxValue)
            {
                keys.Add(current);
                current += Step;
            }

            return keys;
        }
    }
}
