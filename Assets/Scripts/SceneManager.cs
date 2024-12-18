using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private GameObject stopWatchPrefab;
    [SerializeField] private GameObject stickyNotePrefabYellow;
    [SerializeField] private GameObject stickyNotePrefabCyan;
    [SerializeField] private GameObject stickyNotePrefabPink;
    [SerializeField] private Camera xrCamera;
    private string folderPath = "Screenshots"; 
    private string screenshotName = "Screenshot"; 
    private int superSize = 1;
    
    public void AddTimer()
    {
        GameObject timer = Instantiate(timerPrefab);
    }

    public void AddStopWatch()
    {
        GameObject stopWatch = Instantiate(stopWatchPrefab);
    }

    public void AddStickyNote(string colour)
    {
        switch (colour)
        {
            case "Yellow": GameObject yellowStickyNote = Instantiate(stickyNotePrefabYellow); break;
            case "Cyan": GameObject cyanStickyNote = Instantiate(stickyNotePrefabCyan); break;
            case "Pink": GameObject pinkStickyNote = Instantiate(stickyNotePrefabPink); break;
        }
    }
    
    public void DeleteGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void DisplayObject(GameObject gameObject)
    {
        StartCoroutine(DisplayObjectCoroutine(gameObject));
    }
    private IEnumerator DisplayObjectCoroutine(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
    public void TakeScreenshot()
    {
        // Ensure the folder exists
        string fullPath = System.IO.Path.Combine(Application.persistentDataPath, folderPath);
        if (!System.IO.Directory.Exists(fullPath))
        {
            try
            {
                System.IO.Directory.CreateDirectory(fullPath);
                Debug.Log($"Folder created at: {fullPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to create directory at {fullPath}. Error: {ex.Message}");
            }
        }
        else
        {
            Debug.Log($"Folder already exists at: {fullPath}");
        }

        // Generate a unique filename with timestamp
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string filePath = System.IO.Path.Combine(fullPath, $"{screenshotName}_{timestamp}.png");

        // Create a RenderTexture with the current screen dimensions
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        xrCamera.targetTexture = renderTexture;

        // Render the camera's view
        xrCamera.Render();

        // Read the RenderTexture into a Texture2D
        RenderTexture.active = renderTexture;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Reset the camera's target texture and cleanup
        xrCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // Save the Texture2D as a PNG file
        byte[] screenshotBytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(filePath, screenshotBytes);

        Debug.Log($"Screenshot saved at: {filePath}");
    }

    
}
