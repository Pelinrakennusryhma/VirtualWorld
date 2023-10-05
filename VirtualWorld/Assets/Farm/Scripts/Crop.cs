using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cropType;
    [SerializeField] private Image GrowthBar;
    [SerializeField] private Image WaterBar;
    public Plant plant;
    private float lifespan;
    private float age = 0.0f;
    private float water = 100.0f;
    void Start()
    {
        //Application.targetFrameRate = 60;
    }

    public void InitializePlant()
    {
        lifespan = plant.lifespan;
        cropType.text = plant.species;
        StartCoroutine(Growth());
    }


    private IEnumerator Growth()
    {
        while (age < lifespan)
        {
            //Vähentää vettä. Tätä vaihtamalla pystyy säätämään kuinka paljon vettä kasvi vie.
            water -= 1.0f;

            //Jos vesi loppuu, kasvin ikä menee loppuun, jolloin kasvi kuihtuu.
            if (water <= 0.0f)
            {
                age = lifespan;
            }

            //Tätä muuttamalla pystyy säätämään kaikkien kasvien kasvu nopeutta. 
            age += 1.0f;

            WaterBar.fillAmount = water / 100;
            GrowthBar.fillAmount = age / lifespan;

            //Vaihtaa palkin väriä riippuen kuinka täynnä se on.
            if (GrowthBar.fillAmount >= 1.0f)
            {
                GrowthBar.color = Color.black;
            } 
            else if (GrowthBar.fillAmount >= 0.97f)
            {
                GrowthBar.color = Color.red;
            } 
            else if (GrowthBar.fillAmount >= 0.8f)
            {
                GrowthBar.color = Color.yellow;
            }

            yield return new WaitForSeconds(0.1f);
        }
        
    }

    public void WaterCrop()
    {
        water = 100.0f;
    }

    //Lisää CropInventoryyn jos sato oli 80%-99%.
    public void HarvestCrop()
    {
        if(GrowthBar.fillAmount != 1.0f && GrowthBar.fillAmount >= 0.8f)
        {
            CropInventory cropInventory = GameObject.Find("CropInventory").GetComponent<CropInventory>();
            cropInventory.ownedCrops[plant] += 1;
            cropInventory.UpdateInventory();
        }
        Destroy(gameObject);
    }
}
