using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LigthPool : MonoBehaviour
{
    [SerializeField] private GameObject lightGreenPrefab;
    [SerializeField] private GameObject lightYellowPrefab;
    [SerializeField] private GameObject lightRedPrefab;
    [SerializeField] private List<GameObject> lightGreenList;
    [SerializeField] private List<GameObject> lightYellowList;
    [SerializeField] private List<GameObject> lightRedList;
    private int poolSize = 32;

    private static LigthPool instance;
    public static LigthPool Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AddLigthGreenToPool(poolSize);
        AddLigthYellowToPool(8);
        AddLigthRedToPool(4);
    }

    private void AddLigthGreenToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject lightGreen = Instantiate(lightGreenPrefab);
            lightGreen.SetActive(false);
            lightGreenList.Add(lightGreen);
            lightGreen.transform.parent = this.transform;
        }
    }
    private void AddLigthYellowToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject lightYellow = Instantiate(lightYellowPrefab);
            lightYellow.SetActive(false);
            lightYellowList.Add(lightYellow);
            lightYellow.transform.parent = this.transform;
        }
    }
    private void AddLigthRedToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject lightRed = Instantiate(lightRedPrefab);
            lightRed.SetActive(false);
            lightRedList.Add(lightRed);
            lightRed.transform.parent = this.transform;
        }
    }
    public GameObject RequestLightGreen()
    {
        for (int i = 0; i < lightGreenList.Count; i++)
        {
            if (!lightGreenList[i].activeSelf)
            {
                lightGreenList[i].SetActive(true);
                return lightGreenList[i];
            }
        }
        return null;
    }
    public GameObject RequestLightYellow()
    {
        for (int i = 0; i < lightYellowList.Count; i++)
        {
            if (!lightYellowList[i].activeSelf)
            {
                lightYellowList[i].SetActive(true);
                return lightYellowList[i];
            }
        }
        return null;
    }
    public GameObject RequestLightRed()
    {
        for (int i = 0; i < lightRedList.Count; i++)
        {
            if (!lightRedList[i].activeSelf)
            {
                lightRedList[i].SetActive(true);
                return lightRedList[i];
            }
        }
        return null;
    }

    public void DisableList()
    {
        for (int i = 0; i < lightGreenList.Count; i++)
        {
            lightGreenList[i].SetActive(false);
        }

        for (int i = 0; i < lightYellowList.Count; i++)
        {
            lightYellowList[i].SetActive(false);
        }

        for (int i = 0; i < lightRedList.Count; i++)
        {
            lightRedList[i].SetActive(false);
        }
    }
}
