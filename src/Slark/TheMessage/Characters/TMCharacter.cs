using System;
using System.Collections.Generic;

namespace TheMessage
{
    public abstract class TMCharacter : ITMSecretMission
    {
        public ITMSecretMission Mission { get; set; }

        public virtual bool IfWon(TMIntelligence receiving, IEnumerable<TMIntelligence> received, TMPlayer dying, TMPlayer winning)
        {
            return this.Mission.IfWon(receiving, received, dying, winning);
        }
    }
}
