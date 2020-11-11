using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameMode : MonoBehaviour
{
    public InputHandler playerInputHandler;
    public MyPlayer player;
    public TriggerVolume[] deathTriggerVolumes;
    public TriggerVolume winTriggerVolume;
    public GameObject startPosition;

    private class InputPermission : InputHandler.PermissionProvider
    {
        public bool enable;
        public bool HasPermission()
        {
            return enable;
        }
    }
    InputPermission playerInputPermission;

    private class PlayerDeathHandler : MyPlayer.DeathHandler
    {
        MyGameMode myGameMode;
        public PlayerDeathHandler(MyGameMode myGameMode)
        {
            this.myGameMode = myGameMode;
        }
        public void OnDeath()
        {
            myGameMode.StartCoroutine(myGameMode.GameOverAndRespawnCoroutine());
        }
    }
    PlayerDeathHandler playerDeathHandler;

    public class DeathTriggerVolumeHandler : TriggerVolume.Handler
    {
        public void OnEnter(TriggerVolume trigger, Collider2D collision)
        {
            MyPlayer myPlayer = collision.GetComponent<MyPlayer>();
            if ( myPlayer != null)
                myPlayer.Die();
        }
    }
    DeathTriggerVolumeHandler deathTriggerVolumeHandler;

    public class WinTriggerVolumeHandler : TriggerVolume.Handler
    {
        MyGameMode myGameMode;
        public WinTriggerVolumeHandler(MyGameMode myGameMode)
        {
            this.myGameMode = myGameMode;
        }
        public void OnEnter(TriggerVolume trigger, Collider2D collision)
        {
            MyPlayer myPlayer = collision.GetComponent<MyPlayer>();
            if (myPlayer != null)
            {
                myPlayer.Ceremony();
                myGameMode.StartCoroutine(myGameMode.GameWinCoroutine());
            }
        }
    }
    WinTriggerVolumeHandler winTriggerVolumeHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerInputPermission = new InputPermission();
        playerInputPermission.enable = true;
        playerInputHandler.SetPermissionProvider(playerInputPermission);

        playerDeathHandler = new PlayerDeathHandler(this);
        player.deathHandler = playerDeathHandler;

        deathTriggerVolumeHandler = new DeathTriggerVolumeHandler();
        foreach (var volume in deathTriggerVolumes)
            volume.handler = deathTriggerVolumeHandler;

        winTriggerVolumeHandler = new WinTriggerVolumeHandler(this);
        winTriggerVolume.handler = winTriggerVolumeHandler;
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator GameOverAndRespawnCoroutine()
    {
        playerInputPermission.enable = false;
        Debug.Log("GameOver");
        yield return new WaitForSeconds(3);
        player.TeleportAt( startPosition.transform.position );
        playerInputPermission.enable = true;
    }

    IEnumerator GameWinCoroutine()
    {
        playerInputPermission.enable = false;
        Debug.Log("GameWin");
        yield return new WaitForSeconds(3);
        MyGameInstance.instance.hasCleared = true;
        SceneManager.LoadScene(0);
    }

}
