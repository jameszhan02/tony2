/*
 * Project:         Project2 Go fish
 * Author:          ShengZhan, Stephan, Slav
 * Date:            2020/04/13
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
    public class CallbackInfo
    {
        [DataMember]
        public int NumCards { get; private set; }   // # of undealt cards left in the Shoe
        [DataMember] 
        public int NumDecks { get; private set; }   // # of decks used by the Shoe
        [DataMember] 
        public bool EmptyTheHand { get; private set; }  // true means client should clear-out the hand (a listbox)

        public CallbackInfo(int c, int d, bool e)
        {
            NumCards = c;
            NumDecks = d;
            EmptyTheHand = e;
        }
    }
}
