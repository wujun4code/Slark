using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core;
using Slark.Core.Utils;
using System.Linq;

namespace TheMessage
{
    public class TMRoomContainer : ITMRestService<TMRoom>
    {
        public TMLobby Lobby { get; set; }
        public ConcurrentHashSet<TMRoom> Rooms = new ConcurrentHashSet<TMRoom>();

        public TMRoomContainer()
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

        public async Task<string> PostAsync(TMRoom room)
        {
            room.MaxPlayerCount = 8;
            room.ClientInfos = new List<ClientInfoInRoom>();
            room.Status = TMRoom.RoomStatus.Created;
            room.RoomContainer = this;
            Rooms.Add(room);
            await room.SaveAsync();
            return room.ObjectId;
        }

        public Task<TMRoom> GetQuickStartRoom()
        {
            Console.WriteLine($"Rooms.Count: {Rooms.Count}");
            var result = Rooms.Where(room => room.Status == TMRoom.RoomStatus.Waiting).OrderByDescending(room => room.ClientInfos.Count).FirstOrDefault();
            if (result != null)
            {
                return Task.FromResult(result);
            }
            return CreateDefaultRoom();
        }

        public async Task<TMRoom> CreateDefaultRoom()
        {
            var room = new TMRoom();
            room.MaxPlayerCount = 8;
           
            room.Status = TMRoom.RoomStatus.Created;
            room.ClientInfos = new List<ClientInfoInRoom>();
            room.RoomContainer = this;
            Rooms.Add(room);

            await room.SaveAsync();
            return room;
        }

        public void ClientDisconnectLobby(TMClient client)
        {
            var room = FindRoomByClient(client);

            if (room != null)
            {
                room.DisconnectRoom(client);
                if (room.ClientInfos.Count == 0)
                {
                    ClearRoom(room);
                }
            }
        }

        public void ClearRoom(TMRoom room)
        {
            room.RecallRpc(Lobby);
            Rooms.Remove(room);
        }

        public TMRoom FindRoomByClient(TMClient client)
        {
            return Rooms.FirstOrDefault(room => room.ClientInfos.Any(info => info.Client == client));
        }

        public Task<string> PutAsync(TMRoom t)
        {
            throw new NotImplementedException();
        }

    }
}
