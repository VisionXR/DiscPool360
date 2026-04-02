
namespace com.VisionXR.HelperClasses
{

    public enum DominantHand { Right, Left }

    public enum EventCodes
    {
        gameData, coinData, strikerData, avatarData,
        turnData, StartGame, destroyCoins, TurnInformation, GameResult, PutFine, SoundData, AvatarData,
        NetworkPlayer, StrikerArrowOff, PlayerReady, AIMovement, AllCoinsRot
    }
    public enum NetworkType { Host, Client }
    public enum PlayerType { Human, AI }
    public enum PlayerControl { Local, Remote }
    public enum PlayerCoin 
    { 
        Stripe, 
        Solid, 
        Black, 
        Red,
        Color, 
        AllPool, 
        Yellow,
        Green,
        Brown,
        Blue,
        Pink,
        
    }
    public enum Team { TeamA, TeamB }
    public enum GameType { SinglePlayer, MultiPlayer, Tutorial}
    public enum GameMode { Pool, Snooker }

    public enum AIDifficulty { Easy, Medium, Hard }
    public enum GameState { Idle,Starting,Running }
    public enum RoomType { Public,Private}
    public enum AudioType { Coin, Edge, Hole }


    public enum ServerRegion { any, us, @in, eu, asia, au, uae, jp, kr, cae, hk, sa, tr, ussc, usw }

}

