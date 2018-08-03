using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slark.Core.Extensions;
using System.Linq;
using TheMessage.Extensions;
using LeanCloud;
using Slark.Core;
using TheMessage.Protocols;

namespace TheMessage
{
    [AVClassName("ClientInfoInRoom")]
    public class ClientInfoInRoom : AVObject
    {
        public ClientInfoInRoom() { }
        public ClientInfoInRoom(TMClient client)
        {
            Client = client;
            ClientStatus = ClientStatusForRoom.Init;
            ScreenName = client.User.Username;
        }

        [AVFieldName("screenName")]
        public string ScreenName
        {
            get
            {
                return this.GetProperty<string>("ScreenName");
            }
            set
            {
                this.SetProperty<string>(value, "ScreenName");
            }
        }

        [AVFieldName("seatIndex")]
        public int SeatIndex
        {
            get
            {
                return this.GetProperty<int>("SeatIndex");
            }
            set
            {
                this.SetProperty<int>(value, "SeatIndex");
            }
        }

        public TMClient Client { get; set; }

        [AVFieldName("clientStatus")]
        public ClientStatusForRoom ClientStatus
        {
            get
            {
                var originalValue = this.GetProperty<int>("ClientStatus");
                return (ClientStatusForRoom)originalValue;
            }
            set
            {
                var originalValue = (int)value;
                this.SetProperty<int>(originalValue, "ClientStatus");
            }
        }

        public enum ClientStatusForRoom
        {
            Init = 0,
            Ready = 1,
            Unready = 2,
        }
    }

    [RpcHost]
    [AVClassName("TMRoom")]
    public class TMRoom : AVObject, IRpc
    {
        public TMRoom()
        {

        }

        [RpcHostIdProperty]
        public string Id
        {
            get
            {
                return this.ObjectId;
            }
        }

        public object mutex = new object();
        public int NextSeatIndex
        {
            get
            {
                lock (mutex)
                {
                    int i = 1;
                    while (i < MaxPlayerCount)
                    {
                        if (ClientInfos.Any(info => info.SeatIndex == i))
                        {
                            i++;
                            continue;
                        }
                        return i;
                    }
                    return 1;
                }
            }
        }

        [AVFieldName("status")]
        public RoomStatus Status
        {
            get
            {
                var originalValue = this.GetProperty<int>("Status");
                return (RoomStatus)originalValue;
            }
            set
            {
                var originalValue = (int)value;
                this.SetProperty<int>(originalValue, "Status");
            }
        }

        public enum RoomStatus
        {
            Created = 1,
            Waiting = 2,
            Playing = 3
        }

        [AVFieldName("maxPlayerCount")]
        public int MaxPlayerCount
        {
            get
            {
                return this.GetProperty<int>();
            }
            set
            {
                this.SetProperty<int>(value);
            }
        }

        [AVFieldName("host")]
        public AVUser HostUser
        {
            get
            {
                return this.GetProperty<AVUser>();
            }
            set
            {
                this.SetProperty<AVUser>(value);
            }
        }


        public TMRoomContainer RoomContainer { get; set; }
        public ClientInfoInRoom HostClient { get; set; }

        [AVFieldName("clientInfos")]
        public IList<ClientInfoInRoom> ClientInfos
        {
            get
            {
                return this.GetProperty<IList<ClientInfoInRoom>>("ClientInfos");
            }
            set
            {
                this.SetProperty<IList<ClientInfoInRoom>>(value, "ClientInfos");
            }
        }

        #region game
        [Rpc]
        public async Task<TMGame> StartNewGame(TMContext context)
        {
            await context.ReplyOKAsync();
            var game = new TMGame();
            game.Room = this;
            game.Players = ClientInfos.Select(info => new TMPlayer(info) { Game = game });
            game.AdjustMode();
            await this.RpcAllAsync("OnNewGameCreated", game);
            return game;
        }
        #endregion
        [Rpc]
        public async Task SetReady(bool toggle, TMContext context)
        {
            var info = FindClientInfo(context.Client);
            if (info != null)
            {
                if (toggle)
                {
                    info.ClientStatus = ClientInfoInRoom.ClientStatusForRoom.Ready;
                }
                else
                {
                    info.ClientStatus = ClientInfoInRoom.ClientStatusForRoom.Unready;
                }
            }

            await context.ReplyOKAsync();

            this.RpcAllAsync("OnReady", context.Client.User.ObjectId, info.SeatIndex, toggle);
        }

        [Rpc]
        public ClientInfoInRoom JoinRoom(TMContext context)
        {
            var info = new ClientInfoInRoom(context.Client)
            {
                SeatIndex = NextSeatIndex
            };

            ClientInfos.Add(info);

            if (Status == RoomStatus.Created)
            {
                Status = RoomStatus.Waiting;
            }

            this.RpcAllAsync("OnNewClientJoined", info);
            return info;
        }

        [Rpc]
        public ClientInfoInRoom QuitRoom(TMContext context)
        {
            var info = FindClientInfo(context.Client);
            ClientInfos.Remove(info);
            this.RpcAllAsync("OnClientQuit", info);
            if (ClientInfos.Count == 0)
            {
                RoomContainer.ClearRoom(this);
            }
            return info;
        }

        public ClientInfoInRoom DisconnectRoom(TMClient client)
        {
            var info = FindClientInfo(client);
            ClientInfos.Remove(info);
            this.RpcAllAsync("OnClientDisconnect", info);
            return info;
        }

        [Rpc]
        public void SetRoomHost(TMContext context)
        {
            var info = FindClientInfo(context.Client);
            HostClient = info;
            this.HostUser = info.Client.User;
        }

        [Rpc]
        public async Task RoomTextMessage(string message, TMContext context)
        {
            await this.RpcAllAsync("OnRoomTextMessage", context.Client.User, message);
        }

        public ClientInfoInRoom FindClientInfo(TMClient client)
        {
            var info = ClientInfos.FirstOrDefault(ci => ci.Client == client);
            return info;
        }

        public Task BroadcastAsync(string message)
        {
            return this.ClientInfos.Select(info => info.Client).BroadcastAsync(message);
        }



        //public Task InitAllotCampsAsync(IEnumerable<TMPlayer> blues, IEnumerable<TMPlayer> reds, IEnumerable<TMPlayer> independents)
        //{
        //    //if (roomMode.Total != Players.Count) return Task.FromException(new Exception("mode not macth player's count."));
        //}

        //public Task InitAllotCardsAsync(IEnumerable<TMPlayer> blues, IEnumerable<TMPlayer> reds, IEnumerable<TMPlayer> independents)
        //{

        //}

    }
}
