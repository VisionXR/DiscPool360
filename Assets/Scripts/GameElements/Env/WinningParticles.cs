using com.VisionXR.GameElements;
using com.VisionXR.ModelClasses;
using UnityEngine;

public class WinningParticles : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public GameDataSO gameData;
    public PlayerDataSO playerData;

    [Header("Particle Objects")]
    public ParticleSystem winningParticleSystemP1;
    public ParticleSystem winningParticleSystemP2;



    private void OnEnable()
    {
        gameData.GameCompletedEvent += PlayWinningParticles;    
    }

    private void OnDisable()
    {
        gameData.GameCompletedEvent -= PlayWinningParticles;
    }

    public void PlayWinningParticles(int playerId)
    {
        Player mp = playerData.GetMainPlayer();

        if (mp.playerProperties.myId == playerId)
        {
            winningParticleSystemP1.Play();
            winningParticleSystemP2.Play();
        }
    }
}
