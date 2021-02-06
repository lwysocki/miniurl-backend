using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain
{
    public class KeysGenerator : IKeysGenerator
    {
        public long Iteration { get; private set; }
        private long _limit;
        private long _step;

        public KeysGenerator(long limit, long step, long iteration = 0)
        {
            Iteration = iteration;
            _limit = limit;
            _step = step;
        }

        public IList<long> Generate()
        {
            IList<long> keys = new List<long>();
            long current = ++Iteration;

            while (current < _limit)
            {
                keys.Add(current);
                current += _step;
            }

            return keys;
        }
    }
}
