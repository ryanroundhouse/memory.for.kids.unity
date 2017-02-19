using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public static class Utility
    {
        public static NumCards NumberOfCards = NumCards.Six;
        public static Random Random = new Random(DateTime.Now.Millisecond);
    }

    public enum NumCards
    {
        Six = 6,
        Eight = 8
    }
}
