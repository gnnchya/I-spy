using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using TMPro;

public class ObjectDetection : MonoBehaviour
{
    // Public variables
    public RawImage rawImage;          // RawImage to display the webcam input
    public TextMeshProUGUI detectionText;         // Text to show the detected class
    public TextMeshProUGUI gameText;
    public TextMeshProUGUI timerText;
    public NNModel modelFile;          // Barracuda model file
    public TextAsset labelsFile;       // Text file containing the class labels
    public int imageWidth = 640;       // Width of the input image
    public int imageHeight = 480;      // Height of the input image
    public TempGetPoint addPoint;

    // Private variables
    private WebCamTexture webCamTexture;    // Webcam texture
    private IWorker worker;                 // Barracuda worker
    private Model model;                    // Barracuda model
    private int classCount;                 // Number of classes
    private string[] labels;                // Class labels
    private Color32[] colors;               // Colors for drawing bounding boxes
    private float treshold = 0.2f;
    

    // Start is called before the first frame update
    void Start()
    {
        // Load the Barracuda model
        model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);

        // Load the class labels
        labels = labelsFile.text.Split('\n');
        classCount = labels.Length;

        // Initialize the colors for drawing bounding boxes
        colors = new Color32[classCount];
        for (int i = 0; i < classCount; i++)
        {
            colors[i] = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
        }

        // Start the webcam texture
        webCamTexture = new WebCamTexture(WebCamTexture.devices[1].name, imageWidth, imageHeight, 30);
        rawImage.texture = webCamTexture;
        webCamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerText.text != "0")
        {
            // Check if the webcam texture has been correctly initialized
            if (webCamTexture == null || !webCamTexture.isPlaying)
                return;

            // Create a new texture from the webcam texture
            var texture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
            texture.SetPixels32(webCamTexture.GetPixels32());
            texture.Apply();

            // Create a tensor from the texture data
            var inputTensor = new Tensor(texture, 3);

            // Execute the Barracuda model
            Profiler.BeginSample("Execute");
            worker.Execute(inputTensor);
            Profiler.EndSample();

            // Get the output tensor from the Barracuda model
            var outputTensor = worker.PeekOutput();

            // Get the detected class and draw the bounding box
            int detectedClass = GetDetectedClass(outputTensor);
            DrawBoundingBox(detectedClass);

            // Update the detection text
            if (detectedClass != 100)
            {
                detectionText.text = labels[detectedClass];
                if (detectionText.text == gameText.text)
                {
                    addPoint.Add(gameText.text);
                }
            }
            else
            {
                detectionText.text = "Nothing";
            }


            // Display the webcam texture in the RawImage
            rawImage.texture = texture;
            rawImage.rectTransform.localScale = new Vector3(1, webCamTexture.videoVerticallyMirrored ? -1 : 1, 1);
            rawImage.rectTransform.localEulerAngles = new Vector3(webCamTexture.videoVerticallyMirrored ? 180 : 0, 0, 0);

            // Dispose the input and output tensors
            inputTensor.Dispose();
            outputTensor.Dispose();
        }
    }

    // Get the detected class from the output tensor
    private int GetDetectedClass(Tensor outputTensor)
    {
        float maxScore = 0f;
        int detectedClass = 0;

        // Find the class with the highest score
        for (int i = 0; i < classCount; i++)
        {
            float score = outputTensor[0, i];
            //Debug.Log(score);
            if (score > maxScore && score > treshold)
            {
                maxScore = score;
                detectedClass = i;
            }
            else if (score < treshold && score > 0.03)
            {
                detectedClass = 100;
            }
        }

        return detectedClass;
    }

    // Draw a bounding box around the detected object
    private void DrawBoundingBox(int detectedClass)
    {
        // Get the output tensor from the Barracuda model
        var outputTensor = worker.PeekOutput();

        // Check that the outputTensor is long enough to contain the class
        if (classCount + detectedClass * 4 + 3 >= outputTensor.length)
        {
            //Debug.LogWarning("Output tensor is too small to contain class " + detectedClass);
            return;
        }

        // Get the position and size of the bounding box
        float x = outputTensor[0, classCount + detectedClass * 4];
        float y = outputTensor[0, classCount + detectedClass * 4 + 1];
        float width = outputTensor[0, classCount + detectedClass * 4 + 2];
        float height = outputTensor[0, classCount + detectedClass * 4 + 3];

        // Scale the position and size of the bounding box to the RawImage
        x *= rawImage.rectTransform.rect.width;
        y *= rawImage.rectTransform.rect.height;
        width *= rawImage.rectTransform.rect.width;
        height *= rawImage.rectTransform.rect.height;

        // Draw the bounding box
        var rectTransform = detectionText.gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(x, -y);
        rectTransform.sizeDelta = new Vector2(width, height);
        rectTransform.gameObject.SetActive(true);
    }

    // Clean up the Barracuda worker and the webcam texture
    private void OnDestroy()
    {
        worker.Dispose();
        webCamTexture.Stop();
    }

}

