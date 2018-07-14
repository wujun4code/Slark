using System;
using System.Collections.Generic;
using TheMessage.Extensions;
using TheMessage.SecretMissions;

namespace TheMessage.Characters
{
    /// <summary>
    /// 情报处长
    /// </summary>
    public class DirectorOfIntelligence : TMCharacter
    {
        public DirectorOfIntelligence()
        {
            this.Mission = new ThreeRedIntelligencesSecretMission();
        }
    }
}
