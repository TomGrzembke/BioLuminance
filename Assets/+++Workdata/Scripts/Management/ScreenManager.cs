using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    [SerializeField] LoadingScreen _loadingScreen;
    public LoadingScreen LoadingScreen => _loadingScreen;

    void Awake()
    {
        Instance = this;
        LoadingScreen.Initialize();
    }
}