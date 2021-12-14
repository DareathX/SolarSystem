using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    private TrailRenderer[] allRenderers;
    private Toggle moonTrails;
    private Toggle planetTrails;
    private Slider timeScaleSlider;
    private Dropdown zoomDropdown;
    private void Start()
    {
        allRenderers = (TrailRenderer[])FindObjectsOfType(typeof(TrailRenderer));
        moonTrails = GameObject.Find("Moon Trails").GetComponent<Toggle>();
        planetTrails = GameObject.Find("Planet Trails").GetComponent<Toggle>();
        timeScaleSlider = GameObject.Find("Time Scale Slider").GetComponent<Slider>();
        zoomDropdown = GameObject.Find("Zoom Dropdown").GetComponent<Dropdown>();
    }

    private void Update()
    {
        TurnOnTrails(planetTrails, "Planet");
        TurnOnTrails(moonTrails, "Moon");
        CameraFollow();
        Time.timeScale = timeScaleSlider.value;
    }

    private void TurnOnTrails(Toggle trails, string tag)
    {
        if (trails.isOn)
        {
            foreach (var trail in allRenderers)
            {
                if (trail.tag != tag) continue;
                trail.enabled = true;
            }
            return;
        }

        foreach (var trail in allRenderers)
        {
            if (trail.tag != tag) continue;
            trail.enabled = false;
        }
    }

    private void CameraFollow()
    {
        string celestialObject = zoomDropdown.options[zoomDropdown.value].text;

        if (celestialObject.Equals("Sun Close"))
        {
            Camera.main.transform.position = new Vector3(0, 2000, 0);
            return;
        }
        else if (!celestialObject.Equals("Sun"))
        {
            GameObject planet = GameObject.Find(celestialObject);
            Camera.main.transform.position = new Vector3(planet.transform.position.x, planet.transform.localScale.y*4, planet.transform.position.z);
            return;
        }

        Camera.main.transform.position = new Vector3(0, 9000, 0);
    }
}
