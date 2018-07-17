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
    public class DirectorOfIntelligence : TMCharacter
    {
        public DirectorOfIntelligence()
        {
            this.Mission = new ThreeRedIntelligencesSecretMission();

            this.Spells = new List<TMSpell>()
            {
                {
                    new TMSpell()
                    {
                        Name = "偷龙转凤",
                        Description = "当情报到达你时，你可以丢弃一张手牌，然后将该情报与牌库顶的第一张牌调换。",
                    }
                },
                {
                    new TMSpell()
                    {
                        Name = "隔墙有耳",
                        Description = "当你试探一位玩家时，你可以抽二张牌，然后选择一张手牌放回牌库顶。"
                    }
                }
            };
        }

        public override string ScreenName { get; set; } = "情报处长";
    }
}
