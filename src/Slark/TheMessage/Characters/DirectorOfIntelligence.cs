using System;
using System.Collections.Generic;
using TheMessage.Extensions;
using TheMessage.SecretMissions;

namespace TheMessage.Characters
{
    public class DirectorOfIntelligence : TMCharacter
    {
        public DirectorOfIntelligence()
        {
            this.Mission = new ThreeRedIntelligencesSecretMission();
        }
    }
}
