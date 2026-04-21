
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

    public enum BoardType { Circle6, Square4, Hexagon4, Octagon4, Triangle3, Hexagon6, Circle4 }
    public enum AIDifficulty { Easy, Medium, Hard }
    public enum GameState { Idle,Starting,Running }
    public enum RoomType { Public,Private}
    public enum AudioType { Coin, Edge, Hole }


    public enum ServerRegion { any, us, @in, eu, asia, au, uae, jp, kr, cae, hk, sa, tr, ussc, usw }

    public enum AchievementSection { SinglePlayer, MultiPlayer, General }

    public enum AchievementType { Simple, Progess }

}

