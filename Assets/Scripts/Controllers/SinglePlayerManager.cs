using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class SinglePlayerManager : MonoBehaviour
    {
        [Header("Scriptable Objects")]
        public UserDataSO userData;
        public StrikerDataSO strikerData;
        public CoinDataSO coinData;
        public BoardDataSO boardData;
        public AudioDataSO audioData;
        public GameDataSO gameData;

        public InputDataSO inputData;
        public PlayerDataSO playerData;
        public UIDataSO uiData;
        public AIDataSO aiData;
        public TableDataSO tableData;


        // local variables
        [Header("Game Objects")]
        public GameObject InputPanel;
        public GameObject PoolScoreCanvas;
        public GameObject SnookerScoreCanvas;
        public List<GameObject> pocketedCoins = new List<GameObject>();

        [Header("Logic")]
        
        public PoolLogic poolLogic;
        public SnookerLogic snookerLogic;
        private int firstTurnId = 1;
        private bool isFirstCoinPocketed = false;
        public int _previousTurnId = -1;

        private Coroutine endGameRoutine = null;

        private void OnEnable()
        {

            coinData.CoinPocketedEvent += CheckPocketedCoins;

            strikerData.StrikerStartedEvent += StrikeStarted;
            strikerData.StrikerStoppedEvent += StrikeStopped;

            strikerData.StrikerPocketedEvent += StrikePocketed;
            strikerData.FoulCompleteEvent += FoulCompleted;

            gameData.PlayAgainEvent += PlayAgain;

            gameData.ExitGameEvent += EndGame;
            gameData.TurnChangeEvent += OnTurnChangedAssignCoins;

            InputPanel.SetActive(false);
        }

        private void OnDisable()
        {

            coinData.CoinPocketedEvent -= CheckPocketedCoins;

            strikerData.StrikerStartedEvent -= StrikeStarted;
            strikerData.StrikerStoppedEvent -= StrikeStopped;

            strikerData.StrikerPocketedEvent -= StrikePocketed;
            strikerData.FoulCompleteEvent -= FoulCompleted;

            gameData.PlayAgainEvent -= PlayAgain;

            gameData.ExitGameEvent -= EndGame;
            gameData.TurnChangeEvent -= OnTurnChangedAssignCoins;
        }

        // Called whenever turn changes to keep AI/UI in sync for snooker
        private void OnTurnChangedAssignCoins(int newTurnId)
        {
            strikerData.SetFoul(false); // reset foul state at turn change
            pocketedCoins.Clear();
           

            if (uiData.currentGameMode == GameMode.Pool)
            {        
                _previousTurnId = newTurnId;
                AssignPoolCoins();
                uiData.UpdateCoins();
                Player cp = playerData.GetPlayerById(newTurnId);
                Player mp = playerData.GetMainPlayer();
                if (mp.playerProperties.myId == cp.playerProperties.myId)
                {
                    poolLogic.GlowCoins(cp.playerProperties.myCoin);
                    InputPanel.SetActive(true);
                }
            }
            else
            {
                if (_previousTurnId != newTurnId)
                {
                   
                    snookerLogic.SetPhase();
                }
                else
                {
                    snookerLogic.ChangePhase();

                }
                _previousTurnId = newTurnId;
                AssignSnookerCoins();
                uiData.UpdateCoins();

                Player cp = playerData.GetPlayerById(newTurnId);
                Player mp = playerData.GetMainPlayer();
                if (mp.playerProperties.myId == cp.playerProperties.myId)
                {
                    snookerLogic.GlowCoins(cp.playerProperties.myCoin);
                    InputPanel.SetActive(true);
                }

            }
        }

        
        public void StartGame(int id)
        {
            gameData.StartGame();

            userData.CreateSameBoard();

            firstTurnId = id;
            isFirstCoinPocketed = false;
            _previousTurnId = -1;
            StartCoroutine(InitialiseGame(firstTurnId));

            strikerData.SetFoul(false);
      
            tableData.ResetPlatform();
           
        }

        private IEnumerator WaitAndPlayAgain(int id)
        {
             gameData.StartGame();
            _previousTurnId = -1;
            userData.CreateSameBoard();
            strikerData.CreateStriker(boardData.StrikerTransform);
            yield return new WaitForSeconds(0.1f);

           
            firstTurnId = id;
            isFirstCoinPocketed = false;

          
            tableData.ResetPlatform();


            if (uiData.currentGameMode == GameMode.Pool)
            {
                Player current = playerData.GetPlayerById(gameData.currentTurnId);
                Player opponent = playerData.GetPlayerById(GetNextTurn());
                current.playerProperties.myCoin = PlayerCoin.AllPool;
                opponent.playerProperties.myCoin = PlayerCoin.AllPool;

                boardData.SnookerObject.SetActive(false);
                coinData.CreateCoins(GameMode.Pool, boardData.AllCoinsTransform);
                PoolScoreCanvas.SetActive(true);
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                boardData.SnookerObject.SetActive(true);
                SnookerScoreCanvas.SetActive(true);
                Player current = playerData.GetPlayerById(gameData.currentTurnId);
                Player opponent = playerData.GetPlayerById(GetNextTurn());
                current.playerProperties.myCoin = PlayerCoin.Red;
                opponent.playerProperties.myCoin = PlayerCoin.Red;
                coinData.CreateCoins(GameMode.Snooker, boardData.AllCoinsTransform);
                snookerLogic.StartGame();
                _previousTurnId = -1; // reset previous turn for snooker logic


            }

            GameObject striker = strikerData.currentStriker;
            if (striker != null)
            {
                striker.transform.position = boardData.StrikerTransform.position;
                striker.transform.rotation = boardData.StrikerTransform.rotation;
                Rigidbody rb = striker.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }

            pocketedCoins.Clear();
            strikerData.SetFoul(false);

            yield return new WaitForSeconds(1);
            gameData.ChangeTurn(id);
        }

        private void PlayAgain(int id)
        {
            StartCoroutine(WaitAndPlayAgain(id));
         
        }

        private IEnumerator InitialiseGame(int id)
        {
            pocketedCoins.Clear();
            strikerData.SetFoul(false);

            strikerData.CreateStriker(boardData.StrikerTransform);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CreatePlayers());
            yield return new WaitForSeconds(0.1f);


            if (uiData.currentGameMode == GameMode.Pool)
            {
                boardData.SnookerObject.SetActive(false);
                PoolScoreCanvas.SetActive(true);
                coinData.CreateCoins(GameMode.Pool, boardData.AllCoinsTransform);
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                boardData.SnookerObject.SetActive(true);
                SnookerScoreCanvas.SetActive(true);
                coinData.CreateCoins(GameMode.Snooker, boardData.AllCoinsTransform);
                snookerLogic.StartGame();
               
            }

            yield return new WaitForSeconds(0.1f);
            gameData.ChangeTurn(id);
        }


        private IEnumerator CreatePlayers()
        {
            if (uiData.currentGameMode == GameMode.Pool)
            {
                PlayerProperties p1 = new PlayerProperties
                {
                    myId = 1,
                    myName = userData.MyName,
                    myOculusID = userData.MyOculusId,
                    imageURL = userData.MyImageUrl,
                    myPlayerControl = PlayerControl.Local,
                    myPlayerType = PlayerType.Human,
                    myCoin = PlayerCoin.AllPool,
                    myAiDifficulty = uiData.currentAIDifficulty,
                    myImage = userData.MyProfileImage

                };
                playerData.CreatePlayer(p1);

                yield return new WaitForSeconds(0.1f);

                PlayerProperties p2 = new PlayerProperties
                {
                    myId = 2,
                    myName = "AI Player",
                    myPlayerControl = PlayerControl.Local,
                    myPlayerType = PlayerType.AI,
                    myCoin = PlayerCoin.AllPool,
                    myAiDifficulty = uiData.currentAIDifficulty,
                    myImage = userData.AIImages[(int)uiData.currentAIDifficulty]
                };

                playerData.CreatePlayer(p2);

            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                PlayerProperties p1 = new PlayerProperties
                {
                    myId = 1,
                    myName = userData.MyName,
                    myOculusID = userData.MyOculusId,
                    imageURL = userData.MyImageUrl,
                    myPlayerControl = PlayerControl.Local,
                    myPlayerType = PlayerType.Human,
                    myCoin = PlayerCoin.Red,
                    myAiDifficulty = uiData.currentAIDifficulty,
                    myImage = userData.MyProfileImage
                };
                playerData.CreatePlayer(p1);
                yield return new WaitForSeconds(0.1f);
                PlayerProperties p2 = new PlayerProperties
                {
                    myId = 2,
                    myName = "AI Player",
                    myPlayerControl = PlayerControl.Local,
                    myPlayerType = PlayerType.AI,
                    myCoin = PlayerCoin.Red,
                    myAiDifficulty = uiData.currentAIDifficulty,
                    myImage = userData.AIImages[(int)uiData.currentAIDifficulty]
                };
                playerData.CreatePlayer(p2);
            }
        }

        private void CheckPocketedCoins(GameObject coin)
        {
            if(pocketedCoins.Contains(coin))
            { 
                return;
            }

            pocketedCoins.Add(coin);

            if (uiData.currentGameMode == GameMode.Pool)
            {
                if (!isFirstCoinPocketed && coin != null)
                {
                    var tag = coin.tag;
                    if (tag == "Stripe" || tag == "Solid")
                    {
                        Player current = playerData.GetPlayerById(gameData.currentTurnId);
                        Player opponent = playerData.GetPlayerById(GetNextTurn());
                        if (current != null && opponent != null)
                        {
                            if (tag == "Stripe")
                            {
                                current.playerProperties.myCoin = PlayerCoin.Stripe;
                                opponent.playerProperties.myCoin = PlayerCoin.Solid;
                            }
                            else
                            {
                                current.playerProperties.myCoin = PlayerCoin.Solid;
                                opponent.playerProperties.myCoin = PlayerCoin.Stripe;
                            }

                            isFirstCoinPocketed = true;
                            uiData.SetCoins();
                          
                        }
                    }
                }

            }
 
        }

        private void AssignPoolCoins()
        {
            

            Player current = playerData.GetPlayerById(gameData.currentTurnId);
            Player opponent = playerData.GetPlayerById(GetNextTurn());
            if (current == null || opponent == null) return;

            // Helper: check if any coin in list is active
            bool AnyActive(List<GameObject> list)
            {
                if (list == null) return false;
                for (int i = 0; i < list.Count; i++)
                {
                    var c = list[i];
                    if (c != null && c.activeInHierarchy) return true;
                }
                return false;
            }

            // Current player
            if (current.playerProperties.myCoin == PlayerCoin.Stripe && !AnyActive(coinData.stripes))
            {
                current.playerProperties.myCoin = PlayerCoin.Black;
            }
            else if (current.playerProperties.myCoin == PlayerCoin.Solid && !AnyActive(coinData.solids))
            {
                current.playerProperties.myCoin = PlayerCoin.Black;
            }

            // Opponent
            if (opponent.playerProperties.myCoin == PlayerCoin.Stripe && !AnyActive(coinData.stripes))
            {
                opponent.playerProperties.myCoin = PlayerCoin.Black;
            }
            else if (opponent.playerProperties.myCoin == PlayerCoin.Solid && !AnyActive(coinData.solids))
            {
                opponent.playerProperties.myCoin = PlayerCoin.Black;
            }


        }

        private void AssignSnookerCoins()
        {
        

            Player current = playerData.GetPlayerById(gameData.currentTurnId);
            Player opponent = playerData.GetPlayerById(GetNextTurn());
            if (current == null || opponent == null) return;

            if (snookerLogic.phase == SnookerPhase.RedsPhase)
            {
                // Current expected: Red or Color
                current.playerProperties.myCoin =
                    snookerLogic.expectation == ShotExpectation.ExpectRed
                    ? PlayerCoin.Red
                    : PlayerCoin.Color;

                // Opponent informational
                opponent.playerProperties.myCoin = PlayerCoin.Red;
            }
            else
            {
                // Colors phase: assign specific expected color to current using the logic mapping
                current.playerProperties.myCoin = snookerLogic.GetExpectedPlayerCoin();
                opponent.playerProperties.myCoin = current.playerProperties.myCoin ;
            }



        }

        private void StrikeStarted()
        {
            InputPanel.SetActive(false);
            inputData.DisableInput();
            boardData.TurnOffInteractable();
        }

        private void StrikeStopped()
        {
            if (strikerData.isFoul)
            {
                strikerData.PlaceStriker();
            }

            if (uiData.currentGameMode == GameMode.Pool)
            {
                StartCoroutine(WaitAndCheckPoolLogic());
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                StartCoroutine(WaitAndCheckSnookerLogic());
            }
        }

        private void StrikePocketed(int id)
        {

            strikerData.SetFoul(true);
            uiData.ShowFoul();
            audioData.PlayAudio(AudioClipType.Foul);
        }

        private void FoulCompleted()
        {

            StartCoroutine(WaitAndChangeTurn(GetNextTurn()));
        }


        private IEnumerator WaitAndCheckPoolLogic()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject coin in pocketedCoins)
            {
                coin.SetActive(false);
            }

          
            Player currentPlayer = playerData.GetPlayerById(gameData.currentTurnId);
            var myCoinType = currentPlayer.playerProperties.myCoin;

            bool anyPocketed = pocketedCoins.Count > 0;
            bool blackPocketed = poolLogic.HasPocketedBlack(pocketedCoins);
            bool myCoinsRemainingOnBoard = poolLogic.AreAnyCoinsRemaining(myCoinType);

            uiData.UpdateCoins();
            yield return new WaitForSeconds(0.5f);
           
            if (!strikerData.isFoul)
            {
                if (anyPocketed)
                {
                    if (blackPocketed)
                    {
                        if (myCoinsRemainingOnBoard)
                        {
                           
                            pocketedCoins.Clear();
                            coinData.DestroyCoins();

                            gameData.GameCompleted(GetNextTurn());
                            StartCoroutine(SetWinner(GetNextTurn()));
                           
                            yield break;
                        }
                        else
                        {
                            
                            pocketedCoins.Clear();
                            coinData.DestroyCoins();

                            gameData.GameCompleted(gameData.currentTurnId);              
                            StartCoroutine(SetWinner(gameData.currentTurnId));

                            yield break;
                        }
                    }
                    else
                    {
                        if (poolLogic.HasPocketedMyCoin(pocketedCoins, myCoinType))
                        {
                            
                            yield return StartCoroutine(WaitAndChangeTurn(gameData.currentTurnId));
                        }
                        else
                        {
                           
                            yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));
                        }
                    }
                }
                else
                {
                   
                    yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));
                }
            }
            else
            {
                if (!blackPocketed)
                {
                    strikerData.HandleFoul(GetNextTurn());

                    yield break;
                }

           
                pocketedCoins.Clear();
                gameData.GameCompleted(GetNextTurn());
                StartCoroutine(SetWinner(GetNextTurn()));
                coinData.DestroyCoins();
            }            

        }

        private IEnumerator WaitAndCheckSnookerLogic()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject coin in pocketedCoins)
            {
                coin.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);
            var result = snookerLogic.ValidateShot(pocketedCoins, strikerData.isFoul);

            switch (result)
            {
                case SnookerShotResult.Foul:
                    strikerData.HandleFoul(GetNextTurn());
                    pocketedCoins.Clear();
                    yield break;

                case SnookerShotResult.ContinueTurn:
                    yield return StartCoroutine(WaitAndChangeTurn(gameData.currentTurnId));
                    // Continue = same player, no turn change
                    break;

                case SnookerShotResult.ChangeTurn:
                    pocketedCoins.Clear();
                    yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));
                    break;

                case SnookerShotResult.Win:
                    pocketedCoins.Clear();
                    coinData.DestroyCoins();

                    gameData.GameCompleted(gameData.snookerWinnerId);
                  
                    yield return StartCoroutine(SetWinner(gameData.snookerWinnerId));
                    break;
            }
       
        }

        private IEnumerator WaitAndChangeTurn(int id)
        {

            pocketedCoins.Clear();
            yield return new WaitForSeconds(1f);
            gameData.ChangeTurn(id);
        }

        private int GetNextTurn()
        {
            int nextId = gameData.currentTurnId;
            if (nextId == 1)
            {
                nextId = 2;
            }
            else
            {
                nextId = 1;
            }
            return nextId;
        }

        private IEnumerator SetWinner(int id)
        {

            PoolScoreCanvas.SetActive(false);
            SnookerScoreCanvas.SetActive(false);

            Player player = playerData.GetPlayerById(id);
            if (id == 1)
            {
                audioData.PlayAudio(AudioClipType.Winning);
                
                aiData.PlayHandAnimation("Loose", true);

            }
            else // its AI
            {
                audioData.PlayAudio(AudioClipType.Losing);
                aiData.PlayHandAnimation("Win", true);

            }

            yield return new WaitForSeconds(5f);
            aiData.PlayHandAnimation("Loose", false);
            aiData.PlayHandAnimation("Win", false);

        }

        private void EndGame()
        {
            if ((endGameRoutine == null))
            {
                endGameRoutine = StartCoroutine(EndGameRoutine());
            }
           
        }

        private IEnumerator EndGameRoutine()
        {
            pocketedCoins.Clear();
            coinData.DestroyCoins();
            yield return new WaitForSeconds(0.1f);
            strikerData.DestroyStriker();
            yield return new WaitForSeconds(0.1f);
            playerData.DestroyAllPlayers();
            yield return new WaitForSeconds(0.1f);
           
            PoolScoreCanvas.SetActive(false);
            SnookerScoreCanvas.SetActive(false);
            strikerData.SetFoul(false);

            boardData.TurnOffInteractable();
            tableData.ResetPlatform();

            uiData.TriggerHomeEvent();
            endGameRoutine = null;
            gameObject.SetActive(false);

        }

    }
}