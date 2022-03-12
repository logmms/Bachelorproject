using System;

namespace DataStrucs
{
    public class KeyValPair<TKey, TValue> : IEquatable<KeyValPair<TKey, TValue>>
    {
        public TKey Key;
        public TValue Value;
        
        public KeyValPair(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }

        public bool Equals(KeyValPair<TKey, TValue> other)
        {
            if (other == null) return false;
            return this.Key.Equals(other.Key) && this.Value.Equals(other.Value);
        }
    }
}