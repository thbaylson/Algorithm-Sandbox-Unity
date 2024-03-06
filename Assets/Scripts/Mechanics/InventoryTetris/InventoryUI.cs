using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Composite UI element
    [SerializeField] private GridLayoutGroup panelGroupPrefab;
    // Leaf UI element
    [SerializeField] private Image panelPrefab;

    private GridLayoutGroup panelGroupObject;
    private Image panelObject;

    // Start is called before the first frame update
    void Start()
    {
        panelGroupObject = Instantiate(panelGroupPrefab, Vector3.zero, Quaternion.identity);
        panelGroupObject.transform.SetParent(transform, false);
        for(int i = 0; i < 8;  i++)
        {
            panelObject = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelObject.transform.SetParent(panelGroupObject.transform, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
