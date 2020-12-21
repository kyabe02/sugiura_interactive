using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteSlider : MonoBehaviour
{
    [SerializeField]
    int restaurantID = 0;

    [SerializeField]
    int getCount;
    [SerializeField]
    int eatCount;
    [SerializeField]
    int maxCount = 20;

    [SerializeField]
    Slider getSlider;
    [SerializeField]
    Slider eatSlider;

    private int totalCount;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnEnable()
    {
        //店IDによりデータベースから数値を取得する
        maxCount = Database.Instance.df.Where("RestaurantID", restaurantID).LineSize();
        getCount = Database.Instance.df.Where("RestaurantID", restaurantID).Where("Flag", 1).LineSize();
        eatCount = Database.Instance.df.Where("RestaurantID", restaurantID).Where("Flag", 2).LineSize();

        getCount += eatCount;

        getSlider.maxValue = maxCount;
        eatSlider.maxValue = maxCount;
        UpdateSlider();
        print("スライダー更新");
    }
    // Update is called once per frame
    void Update()
    {
        //UpdateSlider();
    }

    void EatFoodCount(int eatFoodCount)
    {
        eatCount = eatFoodCount;
        getCount = System.Math.Min(getCount, eatCount); //getがeatより少ないはずがないので引き上げ
        UpdateSlider();
    }

    void GetFoodCount(int getFoodCount)
    {
        getCount = getFoodCount;
        eatCount = System.Math.Max(eatCount, getCount); //eatがgetより多いはずがないので引き下げ
        UpdateSlider();
    }

    void UpdateSlider()
    {
        getSlider.value = getCount;
        eatSlider.value = eatCount;
    }

}
