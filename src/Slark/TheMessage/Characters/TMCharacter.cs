using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheMessage
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class CharacterNameAttribute : Attribute
    {
        public CharacterNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; internal set; }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SpellNameAttribute : Attribute
    {
        public SpellNameAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; internal set; }
        public string Description { get; internal set; }
    }

    public abstract class TMCharacter
    {
        public ITMSecretMission Mission { get; set; }

        public abstract string ScreenName { get; set; }

        public virtual bool IfWon(TMIntelligence receiving, IEnumerable<TMIntelligence> received, TMPlayer dying, TMPlayer winning)
        {
            return this.Mission.IfWon(receiving, received, dying, winning);
        }

        public virtual IEnumerable<TMSpell> Spells { get; set; }

        public byte ID { get; set; }
    }
}
