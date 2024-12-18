using UnityEngine;
using System.IO;
using Oculus.Platform;
using Oculus.Platform.Models;
using Application = UnityEngine.Application;

public class QuestScreenshotTaker : MonoBehaviour
{
    [SerializeField] private string screenshotFolder = "QuestScreenshots"; // Folder to save the screenshot
    [SerializeField] private Camera xrCamera; // Assign the main XR Camera in the scene

    private void Start()
    {
        // Ensure the screenshot folder exists in the persistent data path
        string fullPath = GetScreenshotFolderPath();
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
    }

    public void TakeScreenshot()
    {
        // Ensure we have a valid XR Camera
        if (xrCamera == null)
        {
            Debug.LogError("XR Camera is not assigned!");
            return;
        }

        // Create a RenderTexture for capturing the frame
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        xrCamera.targetTexture = renderTexture;

        // Render the frame
        xrCamera.Render();

        // Read the rendered frame into a Texture2D
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Reset the XR Camera and render texture
        xrCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Save the screenshot to a file
        SaveScreenshotToFile(screenshot);

        Debug.Log("Screenshot taken successfully!");
    }

    private void SaveScreenshotToFile(Texture2D screenshot)
    {
        // Generate the file path with a timestamp
        string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string filePath = Path.Combine(GetScreenshotFolderPath(), fileName);

        // Convert the screenshot to PNG and save it
        byte[] screenshotBytes = screenshot.EncodeToPNG();
        File.WriteAllBytes(filePath, screenshotBytes);

        Debug.Log($"Screenshot saved at: {filePath}");
    }

    private string GetScreenshotFolderPath()
    {
        // Use Unity's persistent data path to save screenshots in a persistent location
        return Path.Combine(Application.persistentDataPath, screenshotFolder);
    }
}
