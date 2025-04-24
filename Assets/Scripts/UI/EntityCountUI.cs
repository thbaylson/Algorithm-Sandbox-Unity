using System.Collections;
using TMPro;
using UnityEngine;

public class EntityCountUI : MonoBehaviour
{
    public TextMeshProUGUI entityCountText;
    public TextMeshProUGUI framerateText;

    private void Start()
    {
        StartCoroutine(UpdateFramerateTextRoutine());
    }

    public void UpdateCountText(int count)
    {
        entityCountText.text = $"Entities: {count}";
    }

    private IEnumerator UpdateFramerateTextRoutine()
    {
        while (enabled)
        {
            framerateText.text = $"FPS: {1 / Time.deltaTime}";
            yield return new WaitForSeconds(0.1f);
        }
    }
}
