/*
 * Program:     CardsGuiClient.exe
 * Module:      MainWindow.xaml.cs
 * Author:      T. Haworth
 * Date:        March 30, 2020
 * Description: A Windows WPF client that uses CardsLibrary.dll via a WCF service.
 *              This version adds client callbacks to make sure every client receives realtime
 *              updates when the state of the Shoe object changes. 
 * 
 *              Note that we had to add a reference to the .NET Framework 
 *              assembly System.ServiceModel.dll.
 *              
 *              UPDATED SINCE THE March 23 version to call the Shoe's UnregisterFromCallbacks()
 *              method when the client is quitting. This is done in the Window_closing() event 
 *              handler.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;  // WCF types
using CardsLibrary; // Shoe and Card classes

namespace CardsGuiClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ICallback
    {
        // Member variables
        private IShoe shoe = null;

        // C'tor

        public MainWindow()
        {
            InitializeComponent();


            try
            {
                // Connect to the Shoe service using DUPLEX channel (for callbacks)
                // Note that the DuplexChannelFactory constructor accepts a second argument, this,
                // which gives the Shoe service a reference to the client's callback object 
                // (the object that implements ICallback)
                DuplexChannelFactory<IShoe> channel = new DuplexChannelFactory<IShoe>(this, "ShoeEndPoint");
                shoe = channel.CreateChannel();

                // Register for the callbacks (tells the Shoe object to include this instance of 
                // the client in future callback events (i.e. updates)
                shoe.RegisterForCallbacks();

                // Initialize the GUI
                sliderDecks.Minimum = 1;
                sliderDecks.Maximum = 10;
                sliderDecks.Value = shoe.NumDecks;
                txtShoeCount.Text = shoe.NumCards.ToString();
                updateCardCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        // Helper methods

        private void updateCardCounts()
        {
            txtHandCount.Text = lstCards.Items.Count.ToString();
            //txtShoeCount.Text = shoe.NumCards.ToString(); // The callback already does this!
        }

        // Event handlers

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnDraw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Card card = shoe.Draw();

                // Update the GUI
                lstCards.Items.Insert(0, card);
                updateCardCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                shoe.Shuffle();

                // Update the GUI
                //lstCards.Items.Clear(); // The callback already does this!
                updateCardCounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sliderDecks_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (shoe != null)
                {
                    // Reset the number of decks in the shoe
                    shoe.NumDecks = (int)sliderDecks.Value;

                    // Update the GUI
                    if (shoe.NumDecks == 1)
                        txtDeckCount.Text = "1 Deck";
                    else
                        txtDeckCount.Text = shoe.NumDecks + " Decks";
                    //lstCards.Items.Clear(); // The callback already does this!
                    updateCardCounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (shoe != null)
                // Quitting, so unregister from the client callbacks
                shoe.UnregisterFromCallbacks();
        }

        // Callback contract implementation

        private delegate void ClientUpdateDelegate(CallbackInfo info);

        public void UpdateGui(CallbackInfo info)
        {
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                // Update the GUI
                txtShoeCount.Text = info.NumCards.ToString();
                sliderDecks.Value = info.NumDecks;
                txtDeckCount.Text = (info.NumDecks == 1 ? "1 Deck" : info.NumDecks + " Decks");
                if (info.EmptyTheHand)
                {
                    lstCards.Items.Clear();
                    txtHandCount.Text = "0";
                }
            }
            else
            {
                // Not the dispatcher thread that's running this method!
                this.Dispatcher.BeginInvoke(new ClientUpdateDelegate(UpdateGui), info);
            }
        }

       
    } // end class

} // end namespace
