using System;
using System.Collections.Generic;
using System.Linq;
using TheMessage.Extensions;

namespace TheMessage
{
    public class TMCamp
    {
        public TMCamp()
        {
        }
        public TMCampCategory CampCategory { get; set; }
        public enum TMCampCategory
        {
            Red,
            Blue,
            Independent
        }

        public bool IfWon(TMIntelligence receiving, IEnumerable<TMIntelligence> received)
        {
            if (IfDie(receiving, received)) return false;
            var ifReceived = received.TryToReceive(receiving);
            switch (this.CampCategory)
            {
                case TMCampCategory.Blue:
                    {
                        return ifReceived.CountInBlue() == 3;
                    }
                case TMCampCategory.Red:
                    {
                        return ifReceived.CountInRed() == 3;
                    }
            }
            return false;
        }

        public bool IfDie(TMIntelligence receiving, IEnumerable<TMIntelligence> received)
        {
            return received.TryToReceive(receiving).CountInBlack() == 3;
        }
    }
}
