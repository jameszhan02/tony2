﻿/*
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
    [ServiceContract]
    public interface ICallback
    {
        //    [OperationContract(IsOneWay = true)]
        //    void UpdateGui(CallbackInfo info);
        [OperationContract(IsOneWay = true)]
        void Update(int count, int numCards, int nextClient, bool gameOver);

        [OperationContract]
        void Check(int cardAsked);

        [OperationContract]
        void GoFish();

    }

}
