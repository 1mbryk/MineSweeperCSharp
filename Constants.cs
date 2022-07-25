using System;
namespace MineSweeper
{
    public struct Pair<T, U>
    {
        public T? first;
        public U? second;
        public Pair()
        {
            first = default;
            second = default;
        }

        public Pair(T first, U second)
        {
            this.first = first;
            this.second = second;
        }
    }
}

