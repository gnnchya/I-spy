namespace OpenCvSharp.Demo
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Networking;
	using UnityEngine.Profiling;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using TMPro;
	using OpenCvSharp;

	public class LiveSketchScript : WebCamera
	{
		protected override void Awake()
		{
			base.Awake();
			this.forceFrontalCamera = true;
		}
		private Queue<string> detectedShapesQueue = new Queue<string>();

		// Our sketch generation function
		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			Mat img = Unity.TextureToMat(input, TextureParameters);
			// Debug.Log("C"+img.Size());
			Mat hsv = new Mat();
        	Cv2.CvtColor(img, hsv, ColorConversionCodes.BGR2HSV);

			// Define lower and upper bounds for red color in HSV
			Scalar lowerRed = new Scalar(150, 10, 188);
			Scalar upperRed = new Scalar(180, 255, 255);

        	// Create a mask by thresholding the HSV image using the lower and upper bounds
			Mat mask = new Mat();
        	Cv2.InRange(hsv, lowerRed, upperRed, mask);

			// Apply morphological operations to the mask to remove noise and smooth the edges of the detected objects
			Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(5, 5));
			Mat redMask = new Mat();
			Cv2.Erode(mask, redMask, kernel, iterations: 1);
			Cv2.Dilate(redMask, redMask, kernel, iterations: 1);

			// Extract Contours
			Point[][] contours;
			HierarchyIndex[] hierarchy;
			Cv2.FindContours (redMask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

			foreach (Point[] contour in contours) {
				double area = Cv2.ContourArea(contour);
				if (area > 5000)
            	{
					detectedShapesQueue.Enqueue(area.ToString());
					double length = Cv2.ArcLength(contour, true);
					Point[] approx = Cv2.ApproxPolyDP(contour, length * 0.04, true);
					string shapeName = null;
					Scalar color = new Scalar();

					if (approx.Length == 3) {
						shapeName = "Triangle";
						color = new Scalar(203,192,255);
					}
					else if (approx.Length == 4) {
						OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
						if (rect.Width / rect.Height <= 0.1) {
							shapeName = "Square";
							color = new Scalar(0,125 ,255);
						} else {
							shapeName = "Rectangle";
							color = new Scalar(0, 0 ,255);
						}
					}
					else if (approx.Length == 10) {
						shapeName = "Star";
						color = new Scalar(255, 255, 0);
					}
					else if (approx.Length >= 15) {
						shapeName = "Circle";
						color = new Scalar(0, 255, 255);
					}

					if (shapeName != null) {
						Moments m = Cv2.Moments(contour);
						int cx = (int)(m.M10 / m.M00);
						int cy = (int)(m.M01 / m.M00);

						// Debug.Log("mx="+cx);
						// Debug.Log("my="+cy);
						Show(detectedShapesQueue);
						Cv2.DrawContours(img, new Point[][] {contour}, 0, color, -1);
						Cv2.PutText(img, shapeName, new Point(cx-50, cy), HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 0, 0));
					}
				}
			}

			// //Convert image to grayscale
			// Mat imgGray = new Mat ();
			// Cv2.CvtColor (img, imgGray, ColorConversionCodes.BGR2GRAY);
			
			// // Clean up image using Gaussian Blur
			// Mat imgGrayBlur = new Mat ();
			// Cv2.GaussianBlur (imgGray, imgGrayBlur, new Size (5, 5), 0);

			// //Extract edges
			// Mat cannyEdges = new Mat ();
			// Cv2.Canny (imgGrayBlur, cannyEdges, 10.0, 70.0);

			// //Do an invert binarize the image
			// Mat mask = new Mat ();
			// Cv2.Threshold (cannyEdges, mask, 70.0, 255.0, ThresholdTypes.BinaryInv);

			// result, passing output texture as parameter allows to re-use it's buffer
			// should output texture be null a new texture will be created
			output = Unity.MatToTexture(img, output);
			return true;
		}

		static void Show<T>(Queue<T> queue)
		{
			foreach (T element in queue)
			{
				Debug.Log("Start");
				Debug.Log(element);
				Debug.Log("Done");
			}
		}
	}
}