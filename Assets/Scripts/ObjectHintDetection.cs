using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using OpenCvSharp;

struct ColorRange
{
    public Scalar Lower { get; set; }
    public Scalar Upper { get; set; }
    public string ColorName { get; set; }
}

public class ObjectHintDetection : MonoBehaviour
{
    // Public variables
    public RawImage rawImage;          // RawImage to display the webcam input
    public TextMeshProUGUI detectionText;         // Text to show the detected class
    public TextMeshProUGUI gameText;
    public TextMeshProUGUI timerText;
    public int imageWidth = 640;       // Width of the input image
    public int imageHeight = 480;      // Height of the input image
    public int queueLength = 55;         // Maximun recent detection
    public HintGetPoint addPoint;

    // Private variables
    private WebCamTexture webCamTexture;    // Webcam texture
    private Queue<string> detectedShapesQueue = new Queue<string>();
    private Queue<float> distanceQueue = new Queue<float>();
    private List<ColorRange> colorRanges;
    private string shapeName;
    private string colorName;
    private Vector2 imageCenter;


    // Start is called before the first frame update
    void Start()
    {
        //webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, imageWidth, imageHeight, 30);
        webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name, Screen.width, Screen.height, 30);
        rawImage.texture = webCamTexture;
        webCamTexture.Play();


        for (int i = 0; i < queueLength; i++)
        {
            detectedShapesQueue.Enqueue("None");
            distanceQueue.Enqueue(100000f);
        }

        colorRanges = new List<ColorRange>
        {
            new ColorRange { Lower = new Scalar(170, 50, 50), Upper = new Scalar(180, 255, 255), ColorName = "Red" },
            new ColorRange { Lower = new Scalar(5, 120, 70), Upper = new Scalar(15, 255, 255), ColorName = "Orange" },
            new ColorRange { Lower = new Scalar(20, 120, 70), Upper = new Scalar(40, 255, 255), ColorName = "Yellow" },
            new ColorRange { Lower = new Scalar(40, 60, 60), Upper = new Scalar(90, 255, 255), ColorName = "Green" },
            new ColorRange { Lower = new Scalar(90, 60, 60), Upper = new Scalar(150, 255, 255), ColorName = "Blue" },
            new ColorRange { Lower = new Scalar(130, 60, 60), Upper = new Scalar(170, 255, 255), ColorName = "Purple" },
            new ColorRange { Lower = new Scalar(160, 60, 60), Upper = new Scalar(180, 255, 255), ColorName = "Pink" },
            new ColorRange { Lower = new Scalar(0, 60, 20), Upper = new Scalar(30, 255, 200), ColorName = "Brown" },
            new ColorRange { Lower = new Scalar(0, 0, 0), Upper = new Scalar(180, 255, 40), ColorName = "Black" },
            new ColorRange { Lower = new Scalar(0, 0, 180), Upper = new Scalar(180, 25, 255), ColorName = "White" }
        //     new ColorRange { Lower = new Scalar(0, 153, 102), Upper = new Scalar(9, 255, 255), ColorName = "Red" },
        //     new ColorRange { Lower = new Scalar(10, 64, 166), Upper = new Scalar(20, 255, 255), ColorName = "Orange" },
        //     new ColorRange { Lower = new Scalar(21, 38, 166), Upper = new Scalar(34, 255, 255), ColorName = "Yellow" },
        //     new ColorRange { Lower = new Scalar(35, 52, 72), Upper = new Scalar(84, 255, 255), ColorName = "Green" },
        //     new ColorRange { Lower = new Scalar(85, 40, 72), Upper = new Scalar(123, 255, 255), ColorName = "Blue" },
        //     new ColorRange { Lower = new Scalar(124, 64, 64), Upper = new Scalar(150, 255, 255), ColorName = "Purple" },
        //     new ColorRange { Lower = new Scalar(150, 10, 188), Upper = new Scalar(180, 255, 255), ColorName = "Pink" },
        //     new ColorRange { Lower = new Scalar(0, 10, 188), Upper = new Scalar(20, 64, 255), ColorName = "Pink" },
        //     // new ColorRange { Lower = new Scalar(0, 100, 20), Upper = new Scalar(20, 255, 200), ColorName = "Brown" },
        //     new ColorRange { Lower = new Scalar(0, 0, 0), Upper = new Scalar(180, 255, 40), ColorName = "Black" },
        //     new ColorRange { Lower = new Scalar(0, 0, 180), Upper = new Scalar(180, 15, 255), ColorName = "White" }
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (timerText.text != "0")
        {
            // Check if the webcam texture has been correctly initialized
            if (webCamTexture == null || !webCamTexture.isPlaying)
                return;

            // // Create a new texture from the webcam texture
            findColoredShape();

            detectionText.text = FindNearestItem(distanceQueue, detectedShapesQueue);
            if (detectionText.text == gameText.text)
            {
                addPoint.Add(gameText.text);

                for (int i = 0; i < queueLength; i++)
                {
                    detectedShapesQueue.Dequeue();
                    detectedShapesQueue.Enqueue("None");
                    distanceQueue.Dequeue();
                    distanceQueue.Enqueue(100000f);
                }
            }


        }
    }

    void findColoredShape()
    {
        Mat img = OpenCvSharp.Unity.TextureToMat(webCamTexture);
        imageCenter = new Vector2((float)webCamTexture.width / 2f, (float)webCamTexture.height / 2f);

        // Convert BGR to HSV for color filtering
        Mat hsv = new Mat();
        Cv2.CvtColor(img, hsv, ColorConversionCodes.BGR2HSV);

        
        foreach (ColorRange colorRange in colorRanges)
        {
            colorName = colorRange.ColorName;

            // Define lower and upper bounds for color in HSV
            Scalar lower = colorRange.Lower;
            Scalar upper = colorRange.Upper;

            // Create a mask by thresholding the HSV image using the lower and upper bounds
            Mat mask = new Mat();
            Cv2.InRange(hsv, lower, upper, mask);

            // Apply morphological operations to the mask to remove noise and smooth the edges of the detected objects
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
            Mat eMask = new Mat();
            Cv2.Erode(mask, eMask, kernel, iterations: 1);
            Mat dMask = new Mat();
            Cv2.Dilate(eMask, dMask, kernel, iterations: 1);

            // Extract Contours
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours (dMask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            foreach (Point[] contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 20000 && area < 50000)
                {
                    double length = Cv2.ArcLength(contour, true);
                    Point[] approx = Cv2.ApproxPolyDP(contour, length * 0.04, true);
                    string shapeName = null;

                    if (approx.Length == 3)
                    {
                        shapeName = "Triangle";
                    }
                    else if (approx.Length == 4) 
                    {
                        shapeName = "Rectangle";
                    }
                    else if (approx.Length >= 5)
                    {
                        shapeName = "Circle";
                    }

                    if (shapeName != null)
                    {
                        Moments m = Cv2.Moments(contour);
						float cx = (float)(m.M10 / m.M00);
						float cy = (float)(m.M01 / m.M00);
                        float eu_d = CalculateEuclideanDistance(cx, cy, imageCenter);
                        distanceQueue.Dequeue();
                        distanceQueue.Enqueue(eu_d);
                        detectedShapesQueue.Dequeue();
                        detectedShapesQueue.Enqueue(colorName + " " + shapeName);
                    }

                }
            }

            mask.Dispose();
            eMask.Dispose();
            dMask.Dispose();

        }

        img.Dispose();
        hsv.Dispose();

    }

    private static float CalculateEuclideanDistance(float cx, float cy, Vector2 imageSize)
    {
        Vector2 point = new Vector2(cx, cy);
        float distance = Vector2.Distance(point, imageSize);

        return distance;
    }

    private static string FindNearestItem(Queue<float> indexQueue, Queue<string> stringQueue)
    {
        int leastIndex = indexQueue.Select((value, index) => new { Value = value, Index = index })
                                   .OrderBy(item => item.Value)
                                   .First().Index;

        // Retrieve the item from stringQueue using the least index
        string selectedItem = stringQueue.ElementAt(leastIndex);

        return selectedItem;
    }

    private static T FindMostFrequentElement<T>(Queue<T> queue)
    {
        T mostFrequentElement = queue
            .GroupBy(element => element)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .First();
        
        return mostFrequentElement;
    }

    static void Show<T>(Queue<T> queue)
    {
        foreach (T element in queue)
        {
            Debug.Log(element);
        }
    }

}
