using System;
using System.Collections.Generic;
using TheMessage.Extensions;

namespace TheMessage.SecretMissions
{
    public class ThreeRedIntelligencesSecretMission : ITMSecretMission
    {
        public string HumanizeDescription { get; set; } = "获得三张或以上的红色情报。";

        public bool IfWon(TMIntelligence receiving, IEnumerable<TMIntelligence> received, TMPlayer dying, TMPlayer winning)
        {
            var redAlreadyHad = received.CountInColor(TMIntelligence.TMColor.Red);
            if (receiving.IsRed)
            {
                if (redAlreadyHad == 2) return true;
            }
            return false;
        }
    }
}
