using System;
namespace Slark.Server.LeanCloud.Play
{
    /// <summary>
    /// Play host.
    /// </summary>
    public static class PlayHost
    {
        /// <summary>
        /// Creates the default builder.
        /// </summary>
        /// <returns>The default builder.</returns>
        public static IPlayServerBuilder CreateDefaultBuilder()
        {
            return new DefaultPlayServerBuilder();
        }
    }

    /// <summary>
    /// Play server builder.
    /// </summary>
    public interface IPlayServerBuilder
    {
        PlayLobbyServer CreateDefaultLobby();

        PlayGameServer CreateDefaultGame();
    }

    /// <summary>
    /// Default play server builder.
    /// </summary>
    public class DefaultPlayServerBuilder : IPlayServerBuilder
    {
        public PlayGameServer CreateDefaultGame()
        {
            return new StandardPlayGameServer();
        }

        public PlayLobbyServer CreateDefaultLobby()
        {
            return new StandardPlayLobbyServer();
        }
    }
}
