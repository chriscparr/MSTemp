using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;


public class PDFManager : MonoBehaviour
{

    public static PDFManager Instance { get { return s_instance; } }
    private static PDFManager s_instance = null;

    public RawImage[] cellImages; // TODO assign textures as source images at runtime, so should we use IMAGE or RAWIMAGE? (Doesnt matter either way)
    // yeah probs use raw images
    public Text[] cellNames;
    public Text[] cellDescriptors;
    public Camera cellCamera;

    public GameObject PageOne;
    public GameObject PageTwo;

    bool amDoing;

    List<Texture2D> combinedTextures = new List<Texture2D>();
    List<string> files = new List<string>();

    public RawImage finishedEcosystem;
    public Camera pdfCamera;

    [HideInInspector]
    public string[] emailStrings;


    [DllImport("__Internal")]
    private static extern void OpenPDF(string path, string imageOne, string imageTwo);

    [DllImport("__Internal")]
    private static extern void OpenPDFThenEmail(string path, string imageOne, string imageTwo, string to, string cc, string subject);


    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {
        PageOne.transform.parent.GetComponent<Canvas>().enabled = false;
    }

    // hey u could extend this so it uses a camera of your choice?
    // nah maybe not actually because the textures do different tings and go diff places.
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

    public void CaptureCell(Subcell cellToBeCaptured)
    {
        // here right,
        // instantiate a copy of that subcell (with no functionality on it)
        // put it somewhere out of side in front of a grey box
        // our cell camera will be looking at that box, right

        // then use the above steps and get a texture from our cell camera
        // then pass that into our raw image - corresponding to that subcell's name? 
        // or just do it via numbers? i dont know?
    }

    public IEnumerator GenerateEntirePDF()
    {
        amDoing = true;
        PageOne.transform.parent.GetComponent<Canvas>().enabled = true;

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

            // SOME POINT IN THE FUTURE, WE WILL ACTUALLY POPULATE OUR PDF CANVAS WITH STUFF
            // HELL, DONT ACTUALLY DO ALL OF THAT HERE, DO IT AS WE DO THINGS IN THE APP :-)
            // WE'RE DOING IT NOW

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

            System.GC.Collect(); // i swear this is needed
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

        // CONSIDER ADDING DATE AND CLIENT NAME TO THE TEMPPATH ABOVE.

        File.WriteAllBytes(tempPath, mBytes);

        if (emailStrings.Length != 0)
        {
            // crap check to determine whether we are SAVING IT (open with whatever) or EMAILING IT (prepare for mailshot)
            OpenPDFThenEmail(tempPath, files[0], files[1], emailStrings[0], emailStrings[1], emailStrings[2]);
        }
        else
        {
            OpenPDF(tempPath, files[0], files[1]);
        }
 
        PageOne.transform.parent.GetComponent<Canvas>().enabled = false;
        amDoing = false;
    

    }



    }


