using System;
using Random = UnityEngine.Random;

namespace UniOwl
{
    public readonly struct SafeFloat 
    {
        private readonly int _value;
        private readonly int _salt;

        public SafeFloat(float value)
        {
            _salt = Random.Range(int.MinValue / 4, int.MaxValue / 4);
            int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            _value = intValue ^ _salt;
        }

        public override bool Equals(object obj) =>
            (float)this == (float)obj;

        public override int GetHashCode() =>
            base.GetHashCode();

        public override string ToString() =>
            ((float)this).ToString();

        public static implicit operator float(SafeFloat safeFloat) => 
            BitConverter.ToSingle(BitConverter.GetBytes(safeFloat._salt ^ safeFloat._value), 0);
        
        public static implicit operator SafeFloat(float normalFloat) => 
            new SafeFloat(normalFloat);
    }
}
