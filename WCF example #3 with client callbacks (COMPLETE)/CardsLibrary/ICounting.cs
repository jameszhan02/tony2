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
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface ICounting
    {


        [OperationContract]
        void Shuffle();
        [OperationContract]
        int JoinGame();
        [OperationContract(IsOneWay = true)]
        void LeaveGame();
        [OperationContract(IsOneWay = true)]
        void NextCount();
    }
}
