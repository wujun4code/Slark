using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheMessage
{
    public class TMSpell
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Task<TMFeedback> TriggerAsync(TMPlayer player, TMPhase phase, TMOperation operation)
        {
            return null;
        }
    }
}
