using DG.Tweening;
using UnityEngine;


public class ZombieGameManager : MonoBehaviour
{
    public static ZombieGameManager Instance { get; private set; }

    public PrepareUI prepareUI;
    public CardListUI cardListUI;
    public FailUI failUI;
    public WinUI winUI;

    private bool isGameEnd = false;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameStart();
    }
    void GameStart()
    {
        Vector3 currentPositon = Camera.main.transform.position;
        Camera.main.transform.DOPath(
            new Vector3[] { currentPositon, new Vector3(5, 0, -10), currentPositon },
            3, PathType.Linear).OnComplete(OnCameraMoveComplete);
    }
    public void GameEndFail()
    {
        if (isGameEnd == true) return;
        isGameEnd = true;
        failUI.Show();
        ZombieManager.Instance.Pause();
        cardListUI.DisableCardList();
        SunManager.Instance.StopProduce();

        AudioManager.Instance.PlayClip(Config.lose_music);
    }
    public void GameEndSuccess()
    {
        if (isGameEnd == true) return;
        isGameEnd = true;
        winUI.Show();
        cardListUI.DisableCardList();
        SunManager.Instance.StopProduce();

        AudioManager.Instance.PlayClip(Config.win_music);
    }
    void OnCameraMoveComplete()
    {
        prepareUI.Show(OnPrepreUIComplete);
    }
    void OnPrepreUIComplete()
    {
        SunManager.Instance.StartProduce();
        ZombieManager.Instance.StartSpawn();
        cardListUI.ShowCardList();
        AudioManager.Instance.PlayBgm(Config.bgm1);
    }
}
