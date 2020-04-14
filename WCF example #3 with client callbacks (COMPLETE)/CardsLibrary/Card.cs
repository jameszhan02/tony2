/*
 * Program:         CardsLibrary.dll
 * Module:          Card.cs
 * Date:            Mar 23, 2020
 * Author:          T. Haworth
 * Description:     The Card class represents a WCF data contract that represents 
 *                  a standard playing card.
 *                  
 *                  Note that we had to add a reference to the .NET assembly 
 *                  System.Runtime.Serialization to create a DataContract.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace CardsLibrary
{
    [DataContract]
    public class Card
    {
        // Define all possible values for Suit and Rank
        public enum SuitID { Clubs, Diamonds, Hearts, Spades };
        public enum RankID { Ace, King, Queen, Jack, Ten, Nine, Eight, Seven, Six, Five, Four, Three, Two };

        // Public methods & properties
        [DataMember]
        public SuitID Suit { get; private set; }
        [DataMember]
        public RankID Rank { get; private set; }

        public override string ToString()
        {
            return Rank.ToString() + " of " + Suit.ToString();
        }

        // Constructor

        internal Card(SuitID s, RankID r)
        {
            Suit = s;
            Rank = r;
        }

    } // end class

} // end namespace
