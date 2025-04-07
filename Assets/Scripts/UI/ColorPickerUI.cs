using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerUI : MonoBehaviour
{
    private GameObject gObject;
    private Color sliderColor = new Color(1f, 1f, 1f, 1f);

    public void OnToggleGroupChanged(Toggle toggle)
    {
        //Debug.Log($"Shape Toggle State Changed: {toggle.name} is {toggle.isOn}");
        Destroy(gObject);
        if (toggle.isOn)
        {
            switch (toggle.name)
            {
                case "CubeToggle":
                    gObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case "SphereToggle":
                    gObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case "CapsuleToggle":
                    gObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
                case "CylinderToggle":
                    gObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case "PlaneToggle":
                    gObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    // This flips the plane to face the camera
                    gObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    break;
            }
            gObject.GetComponent<Renderer>().material.color = sliderColor;
        }
    }

    public void OnColorSliderChanged(Slider slider)
    {
        //Debug.Log($"Color Toggle State Changed: {slider.name} is {slider.value}");
        switch (slider.name)
        {
            case "RedColorSlider":
                sliderColor = new Color(slider.value, sliderColor.g, sliderColor.b, sliderColor.a);
                break;
            case "GreenColorSlider":
                sliderColor = new Color(sliderColor.r, slider.value, sliderColor.b, sliderColor.a);
                break;
            case "BlueColorSlider":
                sliderColor = new Color(sliderColor.r, sliderColor.g, slider.value, sliderColor.a);
                break;
            case "AlphaColorSlider":
                sliderColor = new Color(sliderColor.r, sliderColor.g, sliderColor.b, slider.value);
                break;
        }

        if (gObject != null)
        {
            Renderer renderer = gObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = sliderColor;
            }
        }
    }
}