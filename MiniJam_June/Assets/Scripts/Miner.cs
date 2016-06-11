using UnityEngine;
using System.Collections;

public class Miner : MonoBehaviour
{
    public float mineCoinDelay, mineCoinAmount;
    public float mineStarDelay, mineStarAmount;

    public bool mining;

    void Start()
    {
        StartCoroutine(StartMining());
    }

    IEnumerator StartMining()
    {
        mining = true;
        StartCoroutine(MineCoins());
        StartCoroutine(MineStars());
        yield return null;
    }

    IEnumerator MineCoins()
    {
        while (mining)
        {
            GameController.controller.coinAmount++;
            yield return new WaitForSeconds(mineCoinDelay);
        }
        yield return null;
    }

    IEnumerator MineStars()
    {
        while (mining)
        {
            GameController.controller.starAmount++;
            yield return new WaitForSeconds(mineStarDelay);
        }
        yield return null;
    }
}
