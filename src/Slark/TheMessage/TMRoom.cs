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
    [AVClassName("TMClientInfoInRoom")]
    public class TMClientInfoInRoom : TMSyncObject
    {
        public TMClientInfoInRoom() { }
        public TMClientInfoInRoom(TMClient client)
        {
            Client = client;
            User = client.User;
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

        [AVFieldName("user")]
        public AVUser User
        {
            get
            {
                return this.GetProperty<AVUser>("User");
            }
            set
            {
                this.SetProperty<AVUser>(value, "User");
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
            Playing = 3
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
        public TMClientInfoInRoom HostClient { get; set; }
        public TMLobby Lobby { get; set; }

        [AVFieldName("clientInfos")]
        public IList<TMClientInfoInRoom> ClientInfos
        {
            get
            {
                return this.GetProperty<IList<TMClientInfoInRoom>>("ClientInfos");
            }
            set
            {
                this.SetProperty<IList<TMClientInfoInRoom>>(value, "ClientInfos");
            }
        }

        #region game

        [Rpc]
        public async Task<TMGame> StartNewGame(TMContext context)
        {
            await context.ReplyOKAsync();
            var game = new TMGame();
            game.Room = this;
            await game.SaveAsync();

            this.Status = RoomStatus.Playing;
            await this.SaveAsync();

            game.UseRpc(Lobby);

            var players = new List<TMPlayer>();

            foreach (var info in this.ClientInfos)
            {
                TMPlayer player = new TMPlayer(info) { Game = game };
                players.Add(player);
            }

            await SaveAllAsync(players);
            game.Players = players;

            //List<Task> createdGameTasks = new List<Task>();

            //foreach (var p in game.Players)
            //{
            //    p.UseRpc(Lobby);
            //    createdGameTasks.Add(this.RpcClientAsync(p.Client, "OnNewGameCreated", game, p));
            //}

            this.RpcAllAsync("OnNewGameCreated", game, game.Players).ContinueWith(t =>
            {
                game.RpcAllAsync("WaitingForAllottingCharacters");
                return game.InitAllotCharactersAsync();
            }).Unwrap();

            return game;
        }

        #endregion

        [Rpc]
        public async Task SetClientStatus(int statusIntValue, TMContext context)
        {
            var info = FindClientInfo(context.Client);
            if (info != null)
            {
                info.ClientStatus = (TMClientInfoInRoom.ClientStatusForRoom)statusIntValue;
            }
            await context.ReplyOKAsync();
        }

        [Rpc]
        public TMClientInfoInRoom JoinRoom(TMContext context)
        {
            var info = new TMClientInfoInRoom(context.Client)
            {
                SeatIndex = NextSeatIndex
            };

            ClientInfos.Add(info);

            if (Status == RoomStatus.Created)
            {
                Status = RoomStatus.Waiting;
            }

            info.PropertyChanged += async (sender, e) =>
             {
                 switch (e.PropertyName)
                 {
                     case "ClientStatus":
                         await this.RpcAllAsync("OnClientStatusChanged", context.Client.User.ObjectId, info.SeatIndex, (int)(info.ClientStatus));
                         break;
                     case "ScreenName":
                         await this.RpcAllAsync("OnScreenNameChanged", context.Client.User.ObjectId, info.ScreenName);
                         break;
                     case "SeatIndex":
                         break;
                 }
             };

            this.RpcAllAsync("OnNewClientJoined", info);
            return info;
        }

        [Rpc]
        public TMClientInfoInRoom QuitRoom(TMContext context)
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

        public TMClientInfoInRoom DisconnectRoom(TMClient client)
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

        public TMClientInfoInRoom FindClientInfo(TMClient client)
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
