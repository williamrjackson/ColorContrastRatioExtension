// https://www.w3.org/WAI/WCAG21/Understanding/contrast-minimum.html
// https://www.w3.org/TR/WCAG20-TECHS/G17.html
using UnityEngine;

public static class ContrastRatioExtension
{
    public static Color EnsureContrastRatio(this Color colorA, Color colorB, float targetRatio)
    {
        targetRatio = Mathf.Clamp(targetRatio, 1f, 21f);
        bool success = false;
        float lum1 = RelativeLuminance(colorA);
        float lum2 = RelativeLuminance(colorB);
        bool brighten = false;
        // Favor retaining the same relative difference, e.g. light on dark vs dark on light
        if (lum1 > lum2)
        {
            brighten = true;
        }

        Color result1 = ReachContrastRatio(colorA, colorB, targetRatio, brighten, out success);
        if (success)
        {
            return result1;
        }

        // First direction failed. Try alternative brightness direction
        Color result2 = colorA;
        result2 = ReachContrastRatio(colorA, colorB, targetRatio, !brighten, out success);
        if (success)
        {
            return result2;
        }

        // Both failed to reach the target ratio - pick the closest and warn
        float result1Ratio = CalculateContrastRatio(RelativeLuminance(result1), lum2);
        float result2Ratio = CalculateContrastRatio(RelativeLuminance(result2), lum2);
        if (result1Ratio > result2Ratio)
        {
            Debug.LogWarning("Desired contrast ratio not achieved. Applied ratio: " + result1Ratio.ToString("0.0") + ":1.");
            return result1;
        }
        else
        {
            Debug.LogWarning("Desired contrast ratio not achieved. Applied ratio: " + result2Ratio.ToString("0.0") + ":1.");
            return result2;
        }
    }

    private static Color ReachContrastRatio(Color colorA, Color colorB, float targetRatio, bool brighten, out bool success)
    {

        float lum1 = RelativeLuminance(colorA);
        float lum2 = RelativeLuminance(colorB);

        float latestResultRatio = CalculateContrastRatio(lum1, lum2);
        success = true;

        Color initColor = colorA;
        float t = 0f;
        while (latestResultRatio < targetRatio)
        {
            t += .01f;
            if (brighten)
            {
                colorA = Color.Lerp(initColor, Color.white, t);
                lum1 = RelativeLuminance(colorA);
            }
            else
            {
                colorA = Color.Lerp(initColor, Color.black, t);
                lum1 = RelativeLuminance(colorA);
            }

            latestResultRatio = CalculateContrastRatio(lum1, lum2);
            if (t > 1)
            {
                if (latestResultRatio < targetRatio)
                {
                    success = false;
                }
                break;
            }
        }
        return colorA;
    }

    private static float CalculateContrastRatio(float lum1, float lum2)
    {
        if (lum1 > lum2)
        {
            return (lum1 + .05f) / (lum2 + .05f);
        }
        return (lum2 + .05f) / (lum1 + .05f);
    }

    private static float RelativeLuminance(Color32 color)
    {
        float r, g, b;
        r = BandLuminance(color.r);
        b = BandLuminance(color.b);
        g = BandLuminance(color.g);
        return r * 0.2126f + g * 0.7152f + b * 0.0722f;
    }

    private static float BandLuminance(float v)
    {
        v /= 255f;
        return (v <= 0.03928f) ? v / 12.92f : Mathf.Pow((v + 0.055f) / 1.055f, 2.4f);
    }
}