using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner.Control
{

    /// <summary>
    /// The KeyboardAdapter handles all input from the keyboard and allows information from it to be retrieved in a simple manner.
    /// It implements the singleton design pattern.
    /// </summary>
    class KeyboardAdapter
    {

        private static KeyboardAdapter instance = null;

        private KeyboardAdapter()
        {
            
        }

        public static KeyboardAdapter GetInstance()
        {
            if (instance == null)
                instance = new KeyboardAdapter();

            return instance;
        }
    }

    enum Key { W, A, S, D, F, Up, Down, Left, Right, Space }
}
