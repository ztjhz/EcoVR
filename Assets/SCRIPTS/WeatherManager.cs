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
    public WeatherButton button_NormalDay;
    public WeatherButton button_Night;


    [Header("Tenkoku System Reference")]
    public TenkokuModule tenkoku; // Drag & Drop your TenkokuModule object here

    void Start()
    {
        // Assign button click events
        if (button_Sunny.button) button_Sunny.button.onClick.AddListener(SetSunny);
        if (button_Stormy.button) button_Stormy.button.onClick.AddListener(SetStormy);
        if (button_Snowy.button) button_Snowy.button.onClick.AddListener(SetSnowy);
        if (button_Foggy.button) button_Foggy.button.onClick.AddListener(SetFoggy);
        if (button_NormalDay.button) button_NormalDay.button.onClick.AddListener(SetNormalDay);
        if (button_Night.button) button_Night.button.onClick.AddListener(SetNight);



    }


    void SetSunny()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = 0f;
        tenkoku.weather_FogAmt = 0f;
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.2f, 0.4f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(75f, 90f); // Adjusted from 90–110
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.2f, 0.4f);  // Adjusted up slightly
        tenkoku.sunBright = 2.0f;

        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.0f, 0.15f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.1f, 0.3f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.1f, 0.3f);
        tenkoku.weather_lightning = 0f;
    }



    void SetStormy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = UnityEngine.Random.Range(0.6f, 1.0f);
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0.5f, 1.0f); 
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.2f, 0.4f);     
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.7f, 1.0f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(55f, 75f); 
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.85f, 1.0f);  
        tenkoku.sunBright = 0.6f;

        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.6f, 1.0f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.3f, 0.6f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.7f, 1.0f);
        tenkoku.weather_lightning = UnityEngine.Random.value > 0.5f ? 1.0f : 0.0f;
    }


    void SetSnowy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 1.0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0.2f, 0.6f);
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.3f, 0.5f);
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.4f, 0.8f);     
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(-10f, 32f);
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.5f, 0.8f);    
        tenkoku.sunBright = 0.6f;

        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.6f, 0.9f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.2f, 0.5f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.4f, 0.6f);
        tenkoku.weather_lightning = (UnityEngine.Random.value > 0.9f) ? 1.0f : 0.0f;
    }


    void SetFoggy()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0f, 0.3f);
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.8f, 1.0f);      // Heavy fog emphasized
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.1f, 0.3f);   
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(45f, 60f);   
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.9f, 1.0f);
        tenkoku.sunBright = 0.4f;

        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.3f, 0.6f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.1f, 0.3f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.1f, 0.4f);
        tenkoku.weather_lightning = 0f;
    }


    void SetNormalDay()
    {
        if (tenkoku == null) return;

        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_OvercastAmt = UnityEngine.Random.Range(0f, 0.2f);
        tenkoku.weather_FogAmt = UnityEngine.Random.Range(0.0f, 0.2f);
        tenkoku.weather_WindAmt = UnityEngine.Random.Range(0.2f, 0.4f);
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);
        tenkoku.weather_temperature = UnityEngine.Random.Range(60f, 75f);
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.4f, 0.7f);
        tenkoku.sunBright = UnityEngine.Random.Range(1.2f, 1.6f);

        tenkoku.weather_cloudAltoStratusAmt = UnityEngine.Random.Range(0.2f, 0.4f);
        tenkoku.weather_cloudCirrusAmt = UnityEngine.Random.Range(0.2f, 0.4f);
        tenkoku.weather_cloudCumulusAmt = UnityEngine.Random.Range(0.2f, 0.4f);
        tenkoku.weather_lightning = 0f;
    }

    void SetNight()
    {
        if (tenkoku == null) return;

        // Set time to night
        tenkoku.currentHour = 21;
        tenkoku.currentMinute = 0;
        tenkoku.currentSecond = 0;

        // Clear weather
        tenkoku.weather_RainAmt = 0f;
        tenkoku.weather_SnowAmt = 0f;
        tenkoku.weather_FogAmt = 0f;
        tenkoku.weather_OvercastAmt = 0f;

        // Calm wind
        tenkoku.weather_WindAmt = 0.3f;
        tenkoku.weather_WindDir = UnityEngine.Random.Range(0f, 360f);

        
        tenkoku.weather_temperature = UnityEngine.Random.Range(15f, 40f);
        tenkoku.weather_humidity = UnityEngine.Random.Range(0.2f, 0.4f);

        tenkoku.weather_cloudAltoStratusAmt = 0.3f;
        tenkoku.weather_cloudCirrusAmt = 0.25f;
        tenkoku.weather_cloudCumulusAmt = 0.2f;


        tenkoku.sunBright = 0.1f;
        tenkoku.moonBright = 0.3f;
        tenkoku.moonLightIntensity = 0.25f;

        // Aurora-specific
        tenkoku.auroraIntensity = 0.4f;
        tenkoku.auroraLatitude = 1.0f;
        tenkoku.auroraSpeed = 0.3f;
        tenkoku.auroraSize = 1.4f;


        tenkoku.starIntensity = 1.5f;
        tenkoku.galaxyIntensity = 1.5f;
    }


}
