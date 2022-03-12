using System.Numerics;
using System;
using LoremNET;

namespace DataStrucs
{

    public class HashFunc
    {
        private BigInteger a;
        private BigInteger b;
        public UInt32 m;
        private int q;
        private BigInteger p;



        public HashFunc(UInt32 m) 
        {
            this.q = 61;
            this.p = 2305843009213693951; 
            this.a = Lorem.Number(1, (Int64)p);
            this.b = Lorem.Number(0, (Int64)p);
            this.m = m;
        }

        /// <summary>
        /// Hashes value using multiplyModPrime hashing scheme.
        /// </summary>
        /// <param name="x">Value to hash.</param>
        /// <returns>The hashed value.</returns>
       public UInt32 Run(UInt32 x) 
        {
            BigInteger y = (a*x)+b;
            y = (y & p) + (y >> q);
            y = y >= p ? y-p : y;

            return (UInt32)((UInt64)y % m);
        }
    }
}