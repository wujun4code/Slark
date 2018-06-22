using System;
namespace Slark.Server.LeanCloud.Play
{
    public interface IPlayPluginManager
    {
        void Use(PlayPluginCallback callback, PlayRequestDelegate hook);
        void Abandon(PlayPluginCallback callback);
        void Find(PlayPluginCallback callback);
    }

    public enum PlayPluginCallback
    {
        OnCreateGame,
        BeforeJoin,
        OnJoin,
        OnLeave,
        OnRPC,
        BeforeSetProperties,
        OnSetProperties,
        BeforeCloseGame,
        OnCloseGame
    }
}
