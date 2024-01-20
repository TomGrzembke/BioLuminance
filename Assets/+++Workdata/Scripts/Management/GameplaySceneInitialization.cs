using UnityEngine;

public class GameplaySceneInitialization : MonoBehaviour
{
    #region serialized fields

    #endregion

    #region private fields

    #endregion

    void Awake()
    {
        PointSystem.Instance.GetCreatures();
        PauseManager.Instance.PauseLogic(false);
        MusicManager.Instance.PlaySong(MusicManager.Songs.Ingame);
    }
}