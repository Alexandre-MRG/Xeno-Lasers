using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour {

    [SerializeField] bool sameProbabilityforAllItems = false;
    [SerializeField] bool probabilityByTableIndex = true;
    [SerializeField] float probabilityOfFirstItem = 0.5f;
    [SerializeField] GameObject[] droppableItems;

    GameObject drop;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void dropRandomItem(Vector3 position)
    {

        if (sameProbabilityforAllItems)
        {
            int randomItem = Random.Range(1, droppableItems.Length+1);
            drop = Instantiate(droppableItems[randomItem-1], position, Quaternion.identity) as GameObject;
        }
        else if (probabilityByTableIndex)
        {
            float[] probabilityPerItem = new float[droppableItems.Length];
            float randomItem = Random.value;
            int indexFinal = 0;

            probabilityPerItem[0] = probabilityOfFirstItem;

            for (int i = 1; i < droppableItems.Length; i++)
            {
                probabilityPerItem[i] = probabilityPerItem[i-1] / (2*i);
            }

            for (int i = 1; i < droppableItems.Length; i++)
            {
                if(randomItem < probabilityPerItem[i])
                {
                    print("randomItem " + randomItem);
                    print("< probabilityPerItem[i] " + probabilityPerItem[i]);

                    indexFinal = i;
                }
            }
            GameObject drop = Instantiate(droppableItems[indexFinal], position, Quaternion.identity) as GameObject;
        }
    }
}
