using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheMessage.Extensions;
using TheMessage.SecretMissions;

namespace TheMessage.Characters
{
    /// <summary>
    /// 情报处长
    /// </summary>
    [CharacterName("情报处长")]
    public class DirectorOfIntelligence : TMCharacter
    {
        public DirectorOfIntelligence()
        {
            this.Mission = new ThreeRedIntelligencesSecretMission();
        }

        public override Task<string> StructuredDescriptionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
