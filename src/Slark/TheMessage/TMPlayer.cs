using System;
using System.Collections.Generic;
using Slark.Server.LeanCloud.Play;
using TheMessage.Extensions;

namespace TheMessage
{
    public class TMPlayer : Player
    {
        public TMPlayer()
        {
        }

        public TMCharacter Charater { get; set; }

        public TMCamp Camp { get; set; }

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
