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

using System.ServiceModel;  // WCF types

namespace CardsLibrary
{
    // Define a Callback contract for the client to implement
    //[ServiceContract]
    //public interface ICallback
    //{
    //    [OperationContract(IsOneWay = true)]
    //    void UpdateGui(CallbackInfo info);
    //}


    // Defines a service contract for the Shoe class

    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IShoe
    {
        [OperationContract(IsOneWay = true)]
        void Shuffle();
        [OperationContract] 
        Card Draw();
        int NumCards { [OperationContract] get; }
        [OperationContract(IsOneWay = true)]
        void RegisterForCallbacks();
        [OperationContract(IsOneWay = true)]
        void UnregisterFromCallbacks();
    }

    // Implements the Shoe service

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Shoe : IShoe
    {
        // Private attributes
        private List<Card> cards = null;
        private int cardIdx; // index for cards collection
        private static uint objCount = 0;
        private uint objNum;
        private HashSet<ICallback> callbacks = new HashSet<ICallback>();

        // Constructor
        public Shoe()
        {
            objNum = ++objCount;
            Console.WriteLine($"Creating Shoe object #{objNum}");

            cards = new List<Card>();
            repopulate();
        }

        // Public methods & properties

        public void RegisterForCallbacks()
        {
            // A client calls this method when it's loading!

            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            // Add the client's callback (proxy) object to the collection
            if (!callbacks.Contains(cb))
                callbacks.Add(cb);
        }

        public void UnregisterFromCallbacks()
        {
            // A client calls this method when it's quitting!

            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            // Remove the client's callback object from the collection
            // so that the Shoe object won't try to call a method on a 
            // "dangling reference"
            if (callbacks.Contains(cb))
                callbacks.Remove(cb);
        }

        public void Shuffle()
        {
            Console.WriteLine($"Shoe object #{objNum} Shuffling");

            Random rng = new Random();
            cards = cards.OrderBy(card => rng.Next()).ToList();
            cardIdx = 0;

        }

        public Card Draw()
        {
            if (cardIdx >= cards.Count())
                throw new IndexOutOfRangeException("The shoe is empty.");

            Console.WriteLine($"Shoe object #{objNum} Dealing {cards[cardIdx].ToString()}");

            Card card = cards[cardIdx++];

            // Initiate callbacks

            return card;
        }

       

        public int NumCards
        {
            get
            {
                // Returns the number of cards in the shoe that haven't aready 
                // been dealt via Draw()
                return cards.Count - cardIdx;
            }
        }

        // Helper methods

        private void repopulate()
        {
            Console.WriteLine($"Shoe object #{objNum} Repopulating with ONLY 1 Decks");

            // Clear out the "old" cards
            cards.Clear();

            // Add new "new" cards

                foreach (Card.SuitID s in Enum.GetValues(typeof(Card.SuitID)))
                {
                    foreach (Card.RankID r in Enum.GetValues(typeof(Card.RankID)))
                    {
                        cards.Add(new Card(s, r));
                    }
                }

            // Randomize the collection
            Shuffle();
        }



    } // end class

   

} // end namespace
