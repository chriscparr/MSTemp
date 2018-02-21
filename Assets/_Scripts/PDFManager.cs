using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sharpPDF;
using sharpPDF.Collections;
using sharpPDF.PDFControls;
using sharpPDF.Elements;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;


public class PDFManager : MonoBehaviour
{

    public static PDFManager Instance { get { return s_instance; } }
    private static PDFManager s_instance = null;

    // put differentiator names, descriptors and sprites here
    // then populate as needed
    // but do this later pls

    public GameObject PageOne;
    public GameObject PageTwo;

    List<Texture2D> combinedTextures = new List<Texture2D>();
    List<string> files = new List<string>();

    public RawImage finishedEcosystem;
    public Camera pdfCamera;


    [DllImport("__Internal")]
    private static extern void OpenPDF(string path, string imageOne, string imageTwo);


    private void Awake()
    {
        s_instance = this;
    }

    public void CaptureEcosystem()
    {

        files.Clear();
        combinedTextures.Clear();

        RenderTexture rTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        Camera.main.targetTexture = rTex;
        Camera.main.Render();

        RenderTexture.active = rTex;
        Texture2D ourTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        ourTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        RenderTexture.active = null;
        Camera.main.targetTexture = null;

        byte[] bytes;
        bytes = ourTexture.EncodeToPNG();



        ourTexture.Apply();

        finishedEcosystem.texture = ourTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 3 || Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine("GenerateEntirePDF");
        }
    }

    public IEnumerator GenerateEntirePDF()
    {
        pdfDocument doc = new pdfDocument("pdfTitle", "Mindshare");


        for (int i = 0; i <= 1; i++)
        {
            if (i == 0)
            {
                PageOne.SetActive(true);
                PageTwo.SetActive(false);
                CaptureEcosystem();
            }
            if (i == 1)
            {
                PageOne.SetActive(false);
                PageTwo.SetActive(true);
            }

            pdfPage pg = doc.addPage(Screen.height, Screen.width);

            pdfCamera.gameObject.SetActive(true);

            RenderTexture rTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
            pdfCamera.targetTexture = rTex;
            pdfCamera.Render();

            RenderTexture.active = rTex;
            Texture2D ourTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

            ourTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            RenderTexture.active = null;
            pdfCamera.targetTexture = null;

            ourTexture.Apply();

            byte[] bytes;
            bytes = ourTexture.EncodeToJPG(100);

           

            int iS = Random.Range(0, 500);
            string StempPath = Application.persistentDataPath + "/" + iS.ToString() + ".jpeg";
            File.WriteAllBytes(StempPath, bytes);
            combinedTextures.Add(ourTexture);
            files.Add(StempPath);

            //pdfImageReference ref = new pdfImageRenference(tempPath);
            //pg.AddImageref,0,0) - this doesnt work

            pg = null;
            System.GC.Collect();
            yield return new WaitForSeconds(0.3f);

        }

 
        Texture2D finalTexture = new Texture2D(Screen.width, Screen.height * 2);
        combinedTextures[0].GetPixels();
        finalTexture.SetPixels(0, 0, Screen.width, Screen.height, combinedTextures[0].GetPixels());
        finalTexture.SetPixels(0, Screen.height, Screen.width, Screen.height, combinedTextures[1].GetPixels());
        PageTwo.SetActive(false);
        byte[] mBytes;
        mBytes = finalTexture.EncodeToPNG();
        string tempPath = Application.persistentDataPath + "/" + "OpenMind" + ".pdf";
        File.WriteAllBytes(tempPath, mBytes);
        OpenPDF(tempPath, files[0], files[1]);
 
    

    }



    }


