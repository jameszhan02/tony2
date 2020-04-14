/*
 * Program:         CardsServiceHost.exe
 * Module:          Program.cs
 * Author:          T. Haworth
 * Date:            March 11, 2020
 * Description:     Implements a WCF service host for the Shoe service in 
 *                  CardsLibrary.dll.
 *                  
 *                  Note that we had to add a reference to the .NET Framework 
 *                  assembly System.ServiceModel.dll.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CardsLibrary; // Service contract and implementation
using System.ServiceModel;  // WCF types

namespace CardsServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;

            try
            {
                // Register the service address
                //servHost = new ServiceHost(typeof(Shoe), new Uri("net.tcp://localhost:13200/CardsLibrary/"));

                // Register the service contract and binding
                //servHost.AddServiceEndpoint(typeof(IShoe), new NetTcpBinding(), "ShoeService");

                // The above version of the code has been modified as follows to use the endpoint 
                // configuration in the App.config file
                servHost = new ServiceHost(typeof(Shoe));

                // Run the service
                servHost.Open();
                Console.WriteLine("Service started. Please any key to quit.");
           }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Wait for a keystroke
                Console.ReadKey();
                if (servHost != null)
                    servHost.Close();
            }
        }
    }
}
