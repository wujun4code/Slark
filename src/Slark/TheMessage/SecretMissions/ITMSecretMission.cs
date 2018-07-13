using System;
using System.Collections.Generic;

namespace TheMessage
{
    public interface ITMSecretMission
    {
        bool IfWon(TMIntelligence receiving,
                   IEnumerable<TMIntelligence> received,
                   TMPlayer dying,
                   TMPlayer winning);
    }
}
