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

    public GameObject PageOne;
    public GameObject PageTwo;

    bool amDoing;

    List<Texture2D> combinedTextures = new List<Texture2D>();
    List<string> files = new List<string>();

    public RawImage finishedEcosystem;
    public Camera pdfCamera;

    [HideInInspector]
    public string[] emailStrings;

    public Camera spriteShotCamera;
    public GameObject defaultSubcell;


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
        PrePopulateCells();
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

    void PrePopulateCells()
    {
        spriteShotCamera.enabled = true;
        RenderTexture rTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        spriteShotCamera.targetTexture = rTex;
        spriteShotCamera.Render();

        RenderTexture.active = rTex;
        Texture2D ourTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        ourTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        RenderTexture.active = null;
        spriteShotCamera.targetTexture = null;
        spriteShotCamera.enabled = false;
        ourTexture.Apply();

        Destroy(defaultSubcell);
        // TODO how u gonna pre-populate the names and descriptors my man? (maybe dont)

        for (int i = 0; i < cellImages.Length; i++)
        {
            cellImages[i].texture = ourTexture;
            // cellNames[i].text = "";
        }
        ourTexture = null;

    }

    public void CaptureCell(Subcell cellToBeCaptured)
    {

        // TODO think about PRE-POPULATION in case we dont alter all 8 subcells in 1 presi :-)

        // here right,
        GameObject sc = Instantiate(cellToBeCaptured.gameObject, transform.position, Quaternion.identity) as GameObject;
        sc.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(sc.GetComponent<Subcell>());
        sc.transform.parent = spriteShotCamera.transform;
        sc.transform.localPosition = new Vector3(0, 0, 5);
        sc.transform.localScale = new Vector3(3.4f, 1.7f, 1.7f); // TODO DELETE ME LATER, GET THE SUBCELLS ACTUAL SCALE AND POSITION ACCORDINGLY
        sc.GetComponent<Renderer>().sharedMaterial = cellToBeCaptured.myOnMaterial; // TODO DELETE ME LATER
        // instantiate a copy of that subcell (with no functionality on it)
        // put it somewhere out of side in front of a grey box
        // our cell camera will be looking at that box, right
        spriteShotCamera.enabled = true;
        RenderTexture rTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        spriteShotCamera.targetTexture = rTex;
        spriteShotCamera.Render();

        RenderTexture.active = rTex;
        Texture2D ourTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        ourTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        RenderTexture.active = null;
        spriteShotCamera.targetTexture = null;
        spriteShotCamera.enabled = false;
        ourTexture.Apply();

        Destroy(sc);

        switch (cellToBeCaptured.ServiceDat.ServiceName)
        {
            // TODO later - actually order them by importance, or something like that?
            // TODO atm they are a pre-determined order... fix this when ur headache goes
            case "AGILE":
                cellImages[0].texture = ourTexture;
                cellNames[0].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "CONTENT":
                cellImages[1].texture = ourTexture;
                cellNames[1].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "DATA":
                cellImages[2].texture = ourTexture;
                cellNames[2].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "FAST":
                cellImages[3].texture = ourTexture;
                cellNames[3].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "GROWTH":
                cellImages[4].texture = ourTexture;
                cellNames[4].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "LIFE":
                cellImages[5].texture = ourTexture;
                cellNames[5].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "LOOP":
                cellImages[6].texture = ourTexture;
                cellNames[6].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
            case "SHOP":
                cellImages[7].texture = ourTexture;
                cellNames[7].text = cellToBeCaptured.ServiceDat.ServiceName;
                // TODO do the descriptors later, because fuck knows if they're even in the app yet
                break;
                //}
        }
        ourTexture = null;

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


