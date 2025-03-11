using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Tenkoku.Core; // Ensure correct namespace for TenkokuModule

[System.Serializable]
public class WeatherButton
{
    public string buttonName; // For clarity in Inspector
    public Button button;
}

public class WeatherManager : MonoBehaviour
{
    [Header("Weather Buttons")]
    public WeatherButton button_Sunny;
    public WeatherButton button_Stormy;
    public WeatherButton button_Snowy;
    public WeatherButton button_Foggy;
    public WeatherButton button_Randomized;

    [Header("Tenkoku System Reference")]
    public TenkokuModule tenkoku; // Drag & Drop your TenkokuModule object here

    void Start()
    {
        // Assign button click events
        if (button_Sunny.button) button_Sunny.button.onClick.AddListener(SetSunny);
        if (button_Stormy.button) button_Stormy.button.onClick.AddListener(SetStormy);
        if (button_Snowy.button) button_Snowy.button.onClick.AddListener(SetSnowy);
        if (button_Foggy.button) button_Foggy.button.onClick.AddListener(SetFoggy);
        if (button_Randomized.button) button_Randomized.button.onClick.AddListener(SetRandomizedWeather);
    }

    void SetSunny()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = 0f; // Clear skies
        tenkoku.weather_FogAmt = 0f;
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.2f, 0.6f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(90f, 110f); // Extreme heat
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.05f, 0.15f); // Very low humidity
        tenkoku.sunBright = 2.0f; // Extra sun brightness

        // UnityEngine.Random cloud distribution (low chance)
        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.0f, 0.2f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.0f, 0.3f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.0f, 0.3f);
    }

    void SetStormy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = UnityEngine.Random.Range(0.6f, 1.0f); // Heavy rain
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0.3f, 1.0f); // cloud cover
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.2f, 0.5f);
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.7f, 1.0f); // Strong wind
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(60f, 80f); // Storm temperatures
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.8f, 1.0f); // High humidity
        tenkoku.sunBright = 0.7f; // Reduced sunlight

        // Cloud distribution for storms
        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.5f, 1.0f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.2f, 0.5f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.6f, 1.0f);

        // Lightning (50% chance)
        tenkoku.weather_lightning = UnityEngine.Random.value > 0.5f ? 1.0f : 0.0f;
    }

    void SetSnowy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 1.0f; // Full snow coverage
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0.5f, 1.0f);
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.3f, 0.6f);
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.4f, 0.7f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(-10f, 32f); // Freezing temperatures
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.4f, 0.7f);
        tenkoku.sunBright = 0.7f; // Sunlight dimmed by snow reflection

        // Cloud distribution for snow
        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.4f, 0.9f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.3f, 0.7f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.4f, 0.8f);
        tenkoku.weather_lightning = (UnityEngine.Random.value > 0.9f) ? 1.0f : 0.0f;

    }

    void SetFoggy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0.3f, 0.7f);
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.7f, 1.0f); // Heavy fog
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.3f, 0.5f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(50f, 65f);
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.8f, 1.0f);
        tenkoku.sunBright = 0.5f; // Sunlight reduced due to fog

        // Cloud distribution for foggy weather
        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.2f, 0.6f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.3f, 0.6f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.2f, 0.5f);
    }

    void SetRandomizedWeather()
    {
        if (tenkoku == null) return;

        int randomChoice = UnityEngine.Random.Range(0, 3); // 0 = Sunny, 1 = Stormy, 2 = Foggy
        switch (randomChoice)
        {
            case 0: SetSunny(); break;
            case 1: SetStormy(); break;
            case 2: SetFoggy(); break;
        }
    }
}
