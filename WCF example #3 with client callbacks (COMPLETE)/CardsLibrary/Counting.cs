/*
 * Project:         Project2 Go fish
 * Author:          ShengZhan, Stephan, Slav
 * Date:            2020/04/13
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CardsLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Counting : ICounting
    {
        private int count;                                      // Keeps track of the game count for this counting "game"
        const int MAX_COUNT = 10;                               // Game will be over when the count reaches this limit
        private Dictionary<int, ICallback> callbacks = null;    // Stores id and callback for each client
        private int nextClientId;                               // Unique Id to be assigned of next client that "Joins"
        private int clientIndex;                                // Index of client that "counts" next
        private bool gameOver = false;                          // "Game over" flag

        // Constructor method
        public Counting()
        {
            // Initialize the "game" object
            count = nextClientId = 1;
            clientIndex = 0;
            callbacks = new Dictionary<int, ICallback>();
        }

        // ServiceContract method that lets the client "register" for callbacks from the 
        // service and asigns to the client a unique client Id
        public int JoinGame()
        {
            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (callbacks.ContainsValue(cb))
            {
                // This client is already registered, so just return the id that was 
                // assigned previously
                int i = callbacks.Values.ToList().IndexOf(cb);
                return callbacks.Keys.ElementAt(i);
            }

            // Register this client and return a new client id
            callbacks.Add(nextClientId, cb);
            //updateAllClients();
            return nextClientId++;
        }

        // ServiceContract method that lets the client "unregister" from the callbacks 
        // before disconnecting from the service
        public void LeaveGame()
        {
            // Identify which client is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if (callbacks.ContainsValue(cb))
            {
                int i = callbacks.Values.ToList().IndexOf(cb);
                int id = callbacks.ElementAt(i).Key;
                callbacks.Remove(id);
                if (i == clientIndex)
                    // Need to signal another client so that it can count instead of this
                    // client which is exiting the game
                    updateAllClients();
                else if (clientIndex > i)
                    // This prevents a player from being "skipped over" in the turn-taking
                    // of this "game"
                    clientIndex--;

            }
        }

        // ServiceContract method that performs the count (a "move" made by the current 
        // player in this counting "game")
        public void NextCount()
        {
            // Determine index of the next client that gets to "count"
            clientIndex = ++clientIndex % callbacks.Count;

            // Increment the count and determine of the game is over
            if (++count == MAX_COUNT)
                gameOver = true;

            // Update all clients
            updateAllClients();
        }

        // Helper method that invokes the callback method in each client
        private void updateAllClients()
        {
            foreach (ICallback cb in callbacks.Values)
                cb.Update(count, callbacks.Keys.ElementAt(clientIndex), gameOver);
        }

    }
}
