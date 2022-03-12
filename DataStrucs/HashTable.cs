using System.Collections.Generic;
using System;

namespace DataStrucs
{
    public class HashTable<TKey, TVal>
    {

        private Random rnd;

        private HashFunc h;

        private LinkedList<KeyValPair<TKey, TVal>>[] SArray;

        private HashFunc[] hArray;

        private KeyValPair<TKey, TVal>[][] keyValPairArray;

        /// <summary>
        /// Calculates the binomial coefficient (nCk) (N items, choose k)
        /// This function has been copied from the user Moop at 
        /// https://stackoverflow.com/questions/12983731/algorithm-for-calculating-binomial-coefficient
        /// </summary>
        /// <param name="n">the number items</param>
        /// <param name="k">the number to choose</param>
        /// <returns>the binomial coefficient</returns>
        private UInt32 BinomCoefficient(UInt32 n, UInt32 k)
        {
            if (k > n) { return 0; }
            if (n == k) { return 1; } // only one way to chose when n == k
            if (k > n - k) { k = n - k; } // Everything is symmetric around n-k, so it is quicker to iterate over a smaller k than a larger one.
            UInt32 c = 1;
            for (UInt32 i = 1; i <= k; i++)
            {
                c *= n--;
                c /= i;
            }
            return c;
        }
        

        /// <summary>
        /// This functio gets the inner hash functions s.t. every element that 
        /// hashes to h_i will get a new hashing function. 
        /// </summary>
        /// <param name="n2_i">This is n_i^2 where n_i is the number of elements that hashes to i. </param>
        /// <param name="i">The hashed value used to compute n2_i.</param>
        /// <returns>True if there is a collision under a new inner hash function. Otherwise false. </returns>
        private bool getInnerHash(UInt32 n2_i, UInt32 i)
        {
            hArray[i] = new HashFunc(n2_i);
            this.keyValPairArray[i] = new KeyValPair<TKey, TVal>[n2_i];
            UInt32 hash;
            foreach (KeyValPair<TKey, TVal> x in this.SArray[i])
            {
                hash = hArray[i].Run((UInt32)x.Key.GetHashCode());
                if (this.keyValPairArray[i][hash] != null) return true;
                this.keyValPairArray[i][hash] = x;
            }

            return false;
        }

        public bool ContainsKey(TKey Key)
        {
            UInt32 i = this.h.Run((UInt32)Key.GetHashCode());
            if (keyValPairArray[i] == null) return false;

            UInt32 h_i = this.hArray[i].Run((UInt32)Key.GetHashCode());
            return keyValPairArray[i][h_i] != null && keyValPairArray[i][h_i].Key.GetHashCode() == Key.GetHashCode();
            
        }

        public TVal GetValue(TKey Key)
        {
            UInt32 i = this.h.Run((UInt32)Key.GetHashCode());
            return keyValPairArray[i][this.hArray[i].Run((UInt32)Key.GetHashCode())].Value;
        }

        public TKey GetKey(TKey Key)
        {
            UInt32 i = this.h.Run((UInt32)Key.GetHashCode());
            return keyValPairArray[i][this.hArray[i].Run((UInt32)Key.GetHashCode())].Key;
        }


        private void connstructTable(LinkedList<KeyValPair<TKey, TVal>> S)
        {
            UInt32 C;
            UInt32 n = (UInt32)S.Count;
            this.SArray = new LinkedList<KeyValPair<TKey, TVal>>[n];
            this.hArray = new HashFunc[n];
            this.keyValPairArray = new KeyValPair<TKey, TVal>[n][];
            UInt32 hash;
            C = n;
            while (C >= n && n != 0)
            {
                C = 0;
                this.h = new HashFunc(n);
                for (UInt32 i = 0; i < n; i++)
                {
                    SArray[i] = new LinkedList<KeyValPair<TKey, TVal>>();
                }
                foreach (KeyValPair<TKey, TVal> x in S)
                {
                    hash = this.h.Run((UInt32)x.Key.GetHashCode());
                    SArray[hash].AddLast(x);
                    C += BinomCoefficient((UInt32)SArray[hash].Count, 2);
                }
            } 
            UInt32 n2_i;
            bool collision;
            for (UInt32 i = 0; i < n; i++)
            {
                collision = true;
                if (SArray[i].Count > 0)
                {
                    n2_i = (UInt32)Math.Pow((double)SArray[i].Count, 2);
                    while (collision)
                    {
                        collision = getInnerHash(n2_i, i);
                    }
                }
            }
        }

        public HashTable(LinkedList<KeyValPair<TKey, TVal>> S)
        {
            rnd = new Random();
            connstructTable(S);
        }
    }
}