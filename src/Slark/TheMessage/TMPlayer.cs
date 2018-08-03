using System;
using System.Collections.Generic;
using LeanCloud;
using Slark.Core;
using TheMessage.Extensions;

namespace TheMessage
{
    [AVClassName("TMPlayer")]
    public class TMPlayer : AVObject
    {
        public TMPlayer()
        {

        }

        public TMPlayer(ClientInfoInRoom clientInfoInRoom)
        {
            Client = clientInfoInRoom.Client;
        }

        public enum TMGameState
        {
            WaitingForOthers,
            Deciding,
            Drawing,
            Transmiting,
            Ending
        }

        public TMClient Client { get; set; }

        public TMGame Game { get; set; }

        public TMCharacter Charater { get; set; }

        public TMCamp Camp { get; set; }

        public IEnumerable<TMCard> CardsInHand { get; set; }

        public IEnumerable<TMIntelligence> ReceivedIntelligences { get; set; }

        public bool IfWon(TMIntelligence receiving)
        {
            var ifReceived = ReceivedIntelligences.TryToReceive(receiving);
            if (Camp.CampCategory == TMCamp.TMCampCategory.Blue)
                return ifReceived.CountInBlue() == 3;
            if (Camp.CampCategory == TMCamp.TMCampCategory.Red)
            {
                return ifReceived.CountInRed() == 3;
            }

            return Charater.IfWon(receiving, ReceivedIntelligences, null, null);

        }

        public bool IfDead(TMIntelligence receiving)
        {
            return ReceivedIntelligences.TryToReceive(receiving).CountInBlack() == 3;
        }

        public TMIntelligenceReceivedResult ReceiveIntelligence(TMIntelligence receiving)
        {
            return new TMIntelligenceReceivedResult()
            {
                IfWon = IfWon(receiving),
                IfDead = IfDead(receiving)
            };
        }
    }
}
