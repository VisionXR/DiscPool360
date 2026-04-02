using com.VisionXR.HelperClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "CoinDataSO", menuName = "ScriptableObjects/CoinDataSO", order = 1)]
    public class CoinDataSO : ScriptableObject
    {


        // Coin Data
        public List<GameObject> stripes;
        public List<GameObject> solids;
        public List<GameObject> reds;
        public List<GameObject> colors;
        public GameObject black;


        public List<Rigidbody> AvailableCoinsInGame = new List<Rigidbody>();

        // Events

        public  Action<GameObject> RegisterCoinEvent;
        public  Action<GameObject> UnregisterCoinEvent;

        public  Action<GameObject> CoinPocketedEvent;
        public  Action<GameObject> CoinFellOnGroundEvent;
        public  Action<GameObject> CoinPocketedIntoHoleEvent;


        public Action<GameMode,Transform> CreateCoinsEvent;
        public Action DestroyCoinsEvent;



        //Methods

        private void OnEnable()
        {
            AvailableCoinsInGame.Clear();
        }
        public void RegisterCoin(GameObject coin)
        {
            AvailableCoinsInGame.Add(coin.GetComponent<Rigidbody>());
            // Implement logic for registering a coin
            if (coin.tag == "Stripe")
            {
                if (!stripes.Contains(coin))
                {
                    stripes.Add(coin);
                    
                }

            }

            else if(coin.tag == "Solid")
            {
                if(!solids.Contains(coin))
                {
                    solids.Add(coin);
                  
                }
            }

            else if (coin.tag == "Black")
            {
                black = coin;
               
            }


            else if (coin.tag == "Red")
            {
                if (!reds.Contains(coin))
                {
                    reds.Add(coin);
                    
                }
            }


            else if (coin.tag == "Color")
            {
                if (!colors.Contains(coin))
                {
                    colors.Add(coin);
                    
                }
            }
        }

        public void UnregisterCoin(GameObject coin)
        {

            if (coin.tag == "Stripe")
            {
                if (stripes.Contains(coin))
                {
                    stripes.Remove(coin);
                }

            }

            else if (coin.tag == "Solid")
            {
                if (solids.Contains(coin))
                {
                    solids.Remove(coin);
                }
            }

            else if (coin.tag == "Black")
            {
                black = null;
            }

            else if (coin.tag == "Red")
            {
                if (reds.Contains(coin))
                {
                    reds.Remove(coin);
                }
            }

            else if (coin.tag == "Color")
            {
                if (colors.Contains(coin))
                {
                    colors.Remove(coin);
                }
            }


            AvailableCoinsInGame.Remove(coin.GetComponent<Rigidbody>());
        }

        

        public void CoinPocketed(GameObject coin)
        {
            // Implement logic for when a coin falls into a hole

            CoinPocketedEvent?.Invoke( coin );
        }


        public void CoinFellOnGround(GameObject coin)
        {
            // Implement logic for when a coin falls on the ground

            CoinFellOnGroundEvent?.Invoke( coin );
        }

        public void CoinPocketedIntoHole(GameObject hole)
        {
            // Implement logic for when a coin is pocketed into a specific hole
  
            CoinPocketedIntoHoleEvent?.Invoke( hole );
        }

        public void CreateCoins(GameMode gameMode,Transform coinsTransform)
        {
            CreateCoinsEvent?.Invoke(gameMode,coinsTransform);
        }

        public void DestroyCoins()
        {
            DestroyCoinsEvent?.Invoke();
        }


        public void TurnOffRigidBodies()
        {
            foreach (var coin in stripes)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
            foreach (var coin in solids)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }

            foreach (var coin in reds)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }

            foreach (var coin in colors)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }

            if (black != null)
            {
                Rigidbody rb = black.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }

        public void TurnOnRigidBodies()
        {
            foreach (var coin in stripes)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
            foreach (var coin in solids)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
            foreach (var coin in reds)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
            foreach (var coin in colors)
            {
                if (coin != null)
                {
                    Rigidbody rb = coin.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                }
            }
            if (black != null)
            {
                Rigidbody rb = black.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                }
            }
        }
    }
}
