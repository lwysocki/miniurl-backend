using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Domain.Models
{
    public class Key
    {
        public long Id { get; set; }
        public KeyState State {get; set;}

        public Key(long id)
        {
            Id = id;
            State = KeyState.New;
        }
    }
}
