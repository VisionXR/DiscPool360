using com.VisionXR.GameElements;
using com.VisionXR.HelperClasses;
using com.VisionXR.ModelClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.VisionXR.Controllers
{
    public class MultiPlayerManager : MonoBehaviour
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
        public NetworkOutputDataSO networkOutputData;
        public NetworkInputDataSO networkInputData;



        // local variables
        [Header("Game Objects")]
        public MultiPlayerConnectionManager multiPlayerConnectionManager;
        public GameObject InputPanel;
        public GameObject PoolScoreCanvas;
        public GameObject SnookerScoreCanvas;
        public List<GameObject> pocketedCoins = new List<GameObject>();

        [Header("Logic")]
        public PoolLogic poolLogic;
        public SnookerLogic snookerLogic;
        private int firstTurnId = 1;
        public bool isFirstCoinPocketed = false;
        // Add a field to track previous turn id
        private int _previousTurnId = -1;
        private Coroutine endGameRoutine = null;


        private void OnEnable()
        {
            networkOutputData.StartGameEvent += StartGame;
            networkOutputData.SetWinnerEvent += SetWinner;
            networkOutputData.SetPlayerCoinsEvent += SetCoins;

            networkOutputData.UpdateSnookerScoreEvent += UpdateSnookerScore;

            inputData.StrikerForceChangedEvent += StrikerForceChanged ;

            gameData.PlayAgainEvent += PlayAgain;

            coinData.CoinPocketedEvent += CheckPocketedCoins;

            strikerData.StrikeForceStartedEvent += StrikeForceStarted;
            strikerData.StrikerStartedEvent += StrikeStarted;
            strikerData.StrikerStoppedEvent += StrikeStopped;

            strikerData.StrikerPocketedEvent += StrikePocketed;
            strikerData.StrikerFellOnGroundEvent += StrikeFellOnGround;
            strikerData.FoulCompleteEvent += FoulCompleted;

            gameData.ExitGameEvent += EndGame;

            // Link: re-assign snooker expectation whenever turn changes
            gameData.TurnChangeEvent += OnTurnChangedAssignCoins;
        }

        private void OnDisable()
        {
            networkOutputData.StartGameEvent -= StartGame;
            networkOutputData.SetWinnerEvent -= SetWinner;
            networkOutputData.SetPlayerCoinsEvent -= SetCoins;

            networkOutputData.UpdateSnookerScoreEvent -= UpdateSnookerScore;


            gameData.PlayAgainEvent -= PlayAgain;

            coinData.CoinPocketedEvent -= CheckPocketedCoins;


            inputData.StrikerForceChangedEvent -= StrikerForceChanged;
            strikerData.StrikeForceStartedEvent -= StrikeForceStarted;
            strikerData.StrikerStartedEvent -= StrikeStarted;
            strikerData.StrikerStoppedEvent -= StrikeStopped;

            strikerData.StrikerPocketedEvent -= StrikePocketed;
            strikerData.StrikerFellOnGroundEvent -= StrikeFellOnGround;
            strikerData.FoulCompleteEvent -= FoulCompleted;


            gameData.ExitGameEvent -= EndGame;

            gameData.TurnChangeEvent -= OnTurnChangedAssignCoins;


        }

        public void StartGame(int id)
        {
            _previousTurnId = -1;
            userData.myCoins = id;
            if (id == 0 || id == 1)
            {
                uiData.currentGameMode = GameMode.Pool;

            }
            else
            {
                uiData.currentGameMode = GameMode.Snooker;
            }

            uiData.ResetAllPanels();
            tableData.ResetPlatform();
            firstTurnId = 1;
            isFirstCoinPocketed = false;
            StartCoroutine(InitialiseGame(firstTurnId));
            gameData.currentTurnId = 1;
            multiPlayerConnectionManager.SetPlayStatus(true);
            strikerData.SetFoul(false);
            pocketedCoins.Clear();
        }
        private IEnumerator InitialiseGame(int id)
        {

            strikerData.CreateStriker(boardData.StrikerTransform);
            yield return new WaitForSeconds(0.5f);

            if (uiData.currentGameMode == GameMode.Pool)
            {
                boardData.SnookerObject.SetActive(false);
                PoolScoreCanvas.SetActive(true);
                coinData.CreateCoins(GameMode.Pool, boardData.AllCoinsTransform);
                SetCoinsOnly(PlayerCoin.AllPool, PlayerCoin.AllPool);
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                boardData.SnookerObject.SetActive(true);
                SnookerScoreCanvas.SetActive(true);
                coinData.CreateCoins(GameMode.Snooker, boardData.AllCoinsTransform);
                SetCoinsOnly(PlayerCoin.Red, PlayerCoin.Red);

                snookerLogic.StartGame();

            }

            yield return new WaitForSeconds(0.1f);
            gameData.ChangeTurn(firstTurnId);
        }

        // Called whenever turn changes to keep AI/UI in sync for snooker
        private void OnTurnChangedAssignCoins(int newTurnId)
        {


            pocketedCoins.Clear();
            uiData.UpdateCoins();

            if (uiData.currentGameMode == GameMode.Pool)
            {

                _previousTurnId = newTurnId;
                AssignPoolCoins();

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

        private void PlayAgain(int id)
        {
            multiPlayerConnectionManager.PlayAgain(); // Reuse the same logic as player joining to reset the game for both host and client
        }

        private void StrikePocketed(int id)
        {
            strikerData.SetFoul(true);
            uiData.ShowFoul();
            audioData.PlayAudio(AudioClipType.Foul);
        }
        private void StrikeFellOnGround()
        {
            strikerData.SetFoul(true);
            uiData.ShowFoul();
            audioData.PlayAudio(AudioClipType.Foul);

        }

        private void FoulCompleted()
        {
            multiPlayerConnectionManager.SendFoulComplete();
        }

        private void CheckPocketedCoins(GameObject coin)
        {
            if (pocketedCoins.Contains(coin))
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

                        if (tag == "Stripe")
                        {
                            SetCoins(PlayerCoin.Stripe, PlayerCoin.Solid);
                            multiPlayerConnectionManager.SendPlayerAssignedCoins(PlayerCoin.Stripe, PlayerCoin.Solid);
                        }
                        else
                        {
                            SetCoins(PlayerCoin.Solid, PlayerCoin.Stripe);
                            multiPlayerConnectionManager.SendPlayerAssignedCoins(PlayerCoin.Solid, PlayerCoin.Stripe);
                        }
                    }
                }

            }

        }

        private void StrikerForceChanged(float force)
        {
            strikerData.strikeForce = force;
            multiPlayerConnectionManager.SendStrikeForceChanged(force);
        }

        private void StrikeForceStarted()
        {
          //  multiPlayerConnectionManager.SendStrikeForceChanged();
        }
        private void StrikeStarted()
        {
            inputData.DisableInput();
            boardData.TurnOffInteractable();
            InputPanel.SetActive(false);

            multiPlayerConnectionManager.SendStrikeStarted();
        }

        private void StrikeStopped()
        {
            if (strikerData.isFoul)
            {
                strikerData.PlaceStrikerOnEdge();
            }

            if (uiData.currentGameMode == GameMode.Pool)
            {
                StartCoroutine(WaitAndCheckPoolLogic());
            }
            else if (uiData.currentGameMode == GameMode.Snooker)
            {
                StartCoroutine(WaitAndCheckSnookerLogic());
            }

            multiPlayerConnectionManager.SendStrikeEnded();
        }


        private IEnumerator WaitAndCheckPoolLogic()
        {


            Player currentPlayer = playerData.GetPlayerById(gameData.currentTurnId);
            var myCoinType = currentPlayer.playerProperties.myCoin;
            bool anyPocketed = pocketedCoins.Count > 0;
            bool blackPocketed = poolLogic.HasPocketedBlack(pocketedCoins);
            bool myCoinsRemainingOnBoard = poolLogic.AreAnyCoinsRemaining(myCoinType);
            bool isMyCoinPocketed = poolLogic.HasPocketedMyCoin(pocketedCoins, myCoinType);

            uiData.UpdateCoins();
            multiPlayerConnectionManager.SendActivetCoinData();

            yield return new WaitForSeconds(0.5f);

            if (!strikerData.isFoul)
            {
                if (anyPocketed)
                {
                    if (blackPocketed)
                    {
                        if (myCoinsRemainingOnBoard)
                        {

                            StartCoroutine(SetWinnerRoutine(GetNextTurn()));
                            multiPlayerConnectionManager.SendSetWinner(GetNextTurn());

                            yield break;
                        }
                        else
                        {

                            StartCoroutine(SetWinnerRoutine(gameData.currentTurnId));
                            multiPlayerConnectionManager.SendSetWinner(gameData.currentTurnId);

                            yield break;
                        }
                    }
                    else
                    {
                        if (isMyCoinPocketed)
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
                    yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));
                }

                else
                {

                    multiPlayerConnectionManager.SendSetWinner(GetNextTurn());
                    StartCoroutine(SetWinnerRoutine(GetNextTurn()));
                }

            }

        }

        private IEnumerator WaitAndCheckSnookerLogic()
        {

            var result = snookerLogic.ValidateShot(pocketedCoins, strikerData.isFoul);
            multiPlayerConnectionManager.SendSnookerScore(snookerLogic.requiredColorIndex, snookerLogic.phase, gameData.Player1SnookerScore, gameData.Player2SnookerScore);

            uiData.UpdateCoins();

            yield return new WaitForSeconds(0.5f);
            multiPlayerConnectionManager.SendActivetCoinData();

            yield return new WaitForSeconds(0.5f);

            switch (result)
            {
                case SnookerShotResult.Foul:

                    yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));

                    yield break;

                case SnookerShotResult.ContinueTurn:

                    yield return StartCoroutine(WaitAndChangeTurn(gameData.currentTurnId));

                    // Continue = same player, no turn change
                    break;

                case SnookerShotResult.ChangeTurn:

                    yield return StartCoroutine(WaitAndChangeTurn(GetNextTurn()));

                    break;

                case SnookerShotResult.Win:


                    multiPlayerConnectionManager.SendSetWinner(gameData.snookerWinnerId);

                    yield return StartCoroutine(SetWinnerRoutine(gameData.snookerWinnerId));
                    break;
            }

        }

        private IEnumerator WaitAndChangeTurn(int id)
        {
            yield return new WaitForSeconds(0.5f);
            gameData.ChangeTurn(id);


            Player p = playerData.GetMainPlayer();
            PlayerNetworkData networkData = p.GetComponent<PlayerNetworkData>();
            networkData.RPC_ChangeTurn(id);
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


        private void SetWinner(int id)
        {
            StartCoroutine(SetWinnerRoutine(id));
        }
        private IEnumerator SetWinnerRoutine(int id)
        {

            multiPlayerConnectionManager.SetPlayStatus(false);
            Player player = playerData.GetMainPlayer();

            if (player.playerProperties.myId == id)
            {
                audioData.PlayAudio(AudioClipType.Winning);


            }
            else // its AI
            {
                audioData.PlayAudio(AudioClipType.Losing);

            }
            yield return new WaitForSeconds(2f);
            gameData.GameCompleted(id);
            coinData.DestroyCoins();
            PoolScoreCanvas.SetActive(false);
            SnookerScoreCanvas.SetActive(false);
        }

        private void EndGame()
        {
            if (endGameRoutine == null)
            {
                endGameRoutine = StartCoroutine(EndGameRoutine());
            }
        }

        private IEnumerator EndGameRoutine()
        {
            pocketedCoins.Clear();
            coinData.DestroyCoins();
            InputPanel.SetActive(false);
            multiPlayerConnectionManager.SetPlayStatus(false);

            yield return new WaitForSeconds(0.1f);
            strikerData.DestroyStriker();
            playerData.DestroyAllPlayers();
            boardData.TurnOffInteractable();
            yield return new WaitForSeconds(0.1f);
            networkInputData.LeaveRoom();

            yield return new WaitForSeconds(0.1f);
            PoolScoreCanvas.SetActive(false);
            SnookerScoreCanvas.SetActive(false);
            strikerData.SetFoul(false);

            uiData.TriggerHomeEvent();
            endGameRoutine = null;
            gameObject.SetActive(false);

        }



        // coin methods
        private void UpdateSnookerScore(int requiredColorIndex, SnookerPhase phase, int p1Score, int p2Score)
        {

            SnookerShotResult result = snookerLogic.ValidateShot(pocketedCoins, strikerData.isFoul);

            snookerLogic.requiredColorIndex = requiredColorIndex;
            snookerLogic.phase = phase;
            gameData.SetSnookerScore(p1Score, p2Score);
        }

        private void SetCoinsOnly(PlayerCoin coin1, PlayerCoin coin2)
        {
            Player current = playerData.GetPlayerById(gameData.currentTurnId);
            Player opponent = playerData.GetPlayerById(GetNextTurn());

            current.playerProperties.myCoin = coin1;
            opponent.playerProperties.myCoin = coin2;
        }
        public void SetCoins(PlayerCoin coin1, PlayerCoin coin2)
        {
            Player current = playerData.GetPlayerById(gameData.currentTurnId);
            Player opponent = playerData.GetPlayerById(GetNextTurn());

            current.playerProperties.myCoin = coin1;
            opponent.playerProperties.myCoin = coin2;

            isFirstCoinPocketed = true;
            uiData.SetCoins();
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
                opponent.playerProperties.myCoin = current.playerProperties.myCoin;
            }

        }
    }
}