using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum ActionType
{
    FasterCollectionCoins,
    FasterCollectionStars,
    MoreCollectionCoins,
    MoreCollectionStars,
    FasterSlider,
    SlowerSlider,
    CheaperGoods,
    Nothing
}
public class GameController : MonoBehaviour
{
    public static GameController controller;

    public float speed;
    public Slider slider;

    public bool startPlaying = false;
    public bool playing = false;

    public float minValue = 0f, maxValue = 10f;
    public float inc = 1f;

    float value;

    public Image targetImage;

    public Color[] colors = new Color[8];
    public HashSet<int> availableColors = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
    public Image[] sliderImages;

    public float sliderDelay = 0.5f;
    float startInc;

    bool sliderStopped;

    public int coinAmount, starAmount;
    public Text coinText, starText;

    public GameObject boxPrefab;

    public bool hasBox = false;
    Box box;

    void Awake()
    {
        if (controller == null)
        {
            controller = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (controller != null)
        {
            Destroy(gameObject);
        }

        slider.maxValue = maxValue;
        slider.minValue = minValue;
        slider.value = (minValue + maxValue) / 2f;
        value = slider.value;
        startInc = inc;
        startPlaying = true;

        UpdateColorSlider();

    }

    void Start()
    {
        AudioController.controller.PlaySound(SoundType.Background);
    }

    void Update()
    {
        if (startPlaying && !playing)
        {
            startPlaying = false;
            StartCoroutine(MoveSlider());
        }

        if (addColor)
        {
            AddColor(colorToAlter);
            addColor = false;
        }
        else if (removeColor)
        {
            removeColor = false;
            RemoveColor(colorToAlter);
        }

        if (Input.GetButtonDown("StopSlider"))
        {
            if (hasBox)
            {
                hasBox = false;
                Center.controller.PlaceBox(box);
                box = null;
            }
            else
            {
                if (!sliderStopped)
                {
                    StartCoroutine(StopSlider());

                }
            }
        }

        coinText.text = "Coins: " + coinAmount;
        starText.text = "Stars: " + starAmount;
    }

    IEnumerator StopSlider()
    {
        sliderStopped = true;
        inc = 0;

        int colorAction = GetColorStoppedOn();
        TakeAction(colorAction);
        targetImage.color = colors[colorAction];

        AudioController.controller.PlaySound(SoundType.Create);

        Box b = Instantiate<GameObject>(boxPrefab).GetComponent<Box>();
        b.transform.position = Center.controller.transform.position;
        b.SetColor(colorAction);
        hasBox = true;
        box = b;

        yield return new WaitForSeconds(sliderDelay);
        inc = startInc;
        sliderStopped = false;
        yield return null;
    }

    IEnumerator MoveSlider()
    {
        playing = true;
        while (playing)
        {
            while (slider.value < maxValue)
            {
                value += inc;
                slider.value = value;
                yield return null;
            }
            while (slider.value > minValue)
            {
                value -= inc;
                slider.value = value;
                yield return null;
            }
            yield return null;
        }
        yield return null;
    }


    public int GetColorStoppedOn()
    {
        //based on the slider value and the amount of colours in the hashset
        //sort the hashset, and then 
        List<int> cs = new List<int>(availableColors);
        cs.Sort();
        List<float> values = new List<float>();

        int numberOfAvailableColors = cs.Count;
        float chunkSize = maxValue / numberOfAvailableColors;
        for (int i = 0; i < numberOfAvailableColors; i++)
        {
            if (value >= i * chunkSize && value < (i + 1) * chunkSize)
            {
                Debug.Log("Color found : " + cs[i]);
                return cs[i];
            }
        }
        return 0;
    }
    public void UpdateColorSlider()
    {
        foreach (Image i in sliderImages)
        {
            i.transform.parent.gameObject.SetActive(false);
        }
        foreach (int i in availableColors)
        {
            sliderImages[i].transform.parent.gameObject.SetActive(true);
        }
    }

    public bool addColor, removeColor;
    public int colorToAlter;

    public void RemoveColor(int color)
    {
        if (availableColors.Contains(color))
        {
            sliderImages[color].transform.parent.gameObject.SetActive(false);
            availableColors.Remove(color);
        }
    }

    public void AddColor(int color)
    {
        if (!availableColors.Contains(color))
        {
            sliderImages[color].transform.parent.gameObject.SetActive(true);
            availableColors.Add(color);
        }

    }

    public void TakeAction(int color)
    {
        switch (color)
        {
            case 0: AddColor(1); break;
            case 1: AddColor(2); break;
            case 2: AddColor(3); break;
            case 3: AddColor(4); break;
            case 4: AddColor(5); break;
            case 5: AddColor(6); break;
            case 6: AddColor(7); break;
            case 7: break;
        }
    }

    public int GetAmountNeeded(ActionType action)
    {
        return 1;
    }
}
