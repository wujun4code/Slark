using System;
using System.Collections.Generic;
using LeanCloud;
using Slark.Core;
using TheMessage.Extensions;

namespace TheMessage
{
    [AVClassName("TMPlayer")]
    public class TMPlayer : AVObject, IRpc
    {
        public TMPlayer()
        {

        }

        public TMPlayer(TMClientInfoInRoom clientInfoInRoom) : this()
        {
            Client = clientInfoInRoom.Client;
            RoleId = clientInfoInRoom.SeatIndex;
            User = clientInfoInRoom.Client.User;
        }

        [RpcHostIdProperty]
        public string Id
        {
            get
            {
                return this.ObjectId;
            }
        }

        [AVFieldName("roleId")]
        public int RoleId
        {
            get
            {
                return this.GetProperty<int>("RoleId");
            }
            set
            {
                this.SetProperty<int>(value, "RoleId");
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


        [AVFieldName("game")]
        public TMGame Game
        {
            get
            {
                return this.GetProperty<TMGame>("Game");
            }
            set
            {
                this.SetProperty<TMGame>(value, "Game");
            }
        }

        public enum TMGameState
        {
            WaitingForOthers,
            Deciding,
            Drawing,
            Transmiting,
            Ending
        }

        public TMClient Client { get; set; }

        public TMCharacter Charater { get; set; }

        public TMCamp Camp { get; set; }

        public IEnumerable<TMCard> CardsInHand { get; set; }

        public IEnumerable<TMIntelligence> ReceivedIntelligences { get; set; }

        public bool IfWon(TMIntelligence receiving)
        {
            var ifReceived = ReceivedIntelligences.TryToReceive(receiving);
            if (Camp.CampCategory == TMCamp.TMCampCategory.Blue)
                return ifReceived.CountInBlue() == 3;
            if (Camp.CampCategory == TMCamp.TMCampCategory.Red)
            {
                return ifReceived.CountInRed() == 3;
            }

            return Charater.IfWon(receiving, ReceivedIntelligences, null, null);

        }

        public bool IfDead(TMIntelligence receiving)
        {
            return ReceivedIntelligences.TryToReceive(receiving).CountInBlack() == 3;
        }

        public TMIntelligenceReceivedResult ReceiveIntelligence(TMIntelligence receiving)
        {
            return new TMIntelligenceReceivedResult()
            {
                IfWon = IfWon(receiving),
                IfDead = IfDead(receiving)
            };
        }
    }
}
