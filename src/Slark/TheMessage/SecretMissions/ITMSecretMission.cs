using System;
using System.Collections.Generic;

namespace TheMessage
{
    public interface ITMSecretMission
    {
        string HumanizeDescription { get; set; }
        bool IfWon(TMIntelligence receiving,
                   IEnumerable<TMIntelligence> received,
                   TMPlayer dying,
                   TMPlayer winning);
    }
}
