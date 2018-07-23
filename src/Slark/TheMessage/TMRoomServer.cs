using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Utils;

namespace TheMessage
{
    public class TMRoomServer : ITMRestService<TMRoom>
    {
        public ConcurrentHashSet<TMRoom> Rooms = new ConcurrentHashSet<TMRoom>();

        public TMRoomServer()
        {

        }

        public Task<TMRoom> DeleteAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<TMRoom> GetAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<string> PostAsync(TMRoom t)
        {
            t.Id = StringRandom.RandomHexString(16);
            Rooms.Add(t);
            return Task.FromResult(t.Id);
        }

        public Task<string> PutAsync(TMRoom t)
        {
            throw new NotImplementedException();
        }
    }
}
