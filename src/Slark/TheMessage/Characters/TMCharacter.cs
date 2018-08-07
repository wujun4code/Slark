using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeanCloud;

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

    [AVClassName("TMCharacter")]
    public class TMCharacter : AVObject
    {
        public ITMSecretMission Mission { get; set; }

        [AVFieldName("screenName")]
        public string ScreenName 
        { 
            get
            {
                return this.GetProperty<string>();
            }
            set
            {
                this.SetProperty<string>(value);
            }
        }

        [AVFieldName("serial")]
        public string Serial
        {
            get
            {
                return this.GetProperty<string>();
            }
            set
            {
                this.SetProperty<string>(value);
            }
        }

        public virtual bool IfWon(TMIntelligence receiving, IEnumerable<TMIntelligence> received, TMPlayer dying, TMPlayer winning)
        {
            return this.Mission.IfWon(receiving, received, dying, winning);
        }

        public virtual IEnumerable<TMSpell> Spells { get; set; }
    }
}
