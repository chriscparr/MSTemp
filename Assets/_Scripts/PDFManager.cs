using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;

public class PDFManager : MonoBehaviour
{
    public static PDFManager Instance { get { return s_instance; } }
    private static PDFManager s_instance = null;

    RawImage[] cellImages;

    public GameObject PageOne;

    [Header("Page One Texts")]
    public Text ClientCompanyName;
    public Text ClientActualName;
    public Text ClientPosition;

    public GameObject PageTwo;

    List<Texture2D> combinedTextures = new List<Texture2D>();
    List<string> files = new List<string>();

    public RawImage finishedEcosystem;
    public Camera pdfCamera;

    [HideInInspector]
    public string[] emailStrings;

    public Camera spriteShotCamera;
    public GameObject defaultSubcell;

    public GridLayoutGroup pageTwoGrid; 
    public GameObject imageCellPrefab;
    public Transform gridLayoutParent;

    private int cachedCellNumber = 0;
	private PresentationData m_pData;

    [DllImport("__Internal")]
    private static extern void OpenPDF(string path, string imageOne, string imageTwo);

    [DllImport("__Internal")]
    private static extern void OpenPDFThenEmail(string path, string imageOne, string imageTwo, string to, string cc, string subject);


    private void Awake()
    {
        s_instance = this;
    }

	public void PrePopulate(PresentationData a_Pdata)
    {
		m_pData = a_Pdata;
        PageOne.transform.parent.GetComponent<Canvas>().enabled = false;

        ClientCompanyName.text = m_pData.ClientName;
        ClientActualName.text = m_pData.PresenterName;
        ClientPosition.text = m_pData.PresenterPosition;

        AdjustGrid();
        PrePopulateCells();
    }

    void AdjustGrid()
    {
        // FUTURE PROOFED. ish.

        List<RawImage> imgs = new List<RawImage>();
        List<Text> titles = new List<Text>();
        List<Text> descriptions = new List<Text>();

        imgs.Clear();
        titles.Clear();
        descriptions.Clear();

		int numberOfSubCells = ModelManager.Instance.GetAllSubCells().Length;

        imgs.Add(imageCellPrefab.GetComponent<RawImage>());
        titles.Add(imageCellPrefab.GetComponent<PDFCellContainer>().title);
        descriptions.Add(imageCellPrefab.GetComponent<PDFCellContainer>().descriptor);

        for (int i = 1; i < numberOfSubCells; i++)
        {
            GameObject c = Instantiate(imageCellPrefab, transform.position, Quaternion.identity) as GameObject;

			c.transform.SetParent (gridLayoutParent.transform, false);
            c.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            c.GetComponent<RectTransform>().localPosition = new Vector3(c.GetComponent<RectTransform>().localPosition.x,
                                                                        c.GetComponent<RectTransform>().localPosition.y,
                                                                        0f);
            imgs.Add(c.GetComponent<RawImage>());
            titles.Add(c.GetComponent<PDFCellContainer>().title);
            descriptions.Add(c.GetComponent<PDFCellContainer>().descriptor);
        }

        cellImages = imgs.ToArray();

        Debug.Log(cellImages.Length);
    }


    public void CaptureEcosystem()
    {

        files.Clear();
        combinedTextures.Clear();
        Vector3 posCache = Camera.main.transform.position;
        Camera.main.transform.position = new Vector3(0, 1, -15); //TODO I AM TEMPORARY DELETE ME LATER

        RenderTexture rTex = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
        Camera.main.targetTexture = rTex;
        Camera.main.Render();

        RenderTexture.active = rTex;
        Texture2D ourTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);

        ourTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        RenderTexture.active = null;
        Camera.main.targetTexture = null;
        Camera.main.transform.position = posCache;
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
        // TODO how u gonna pre-populate the names and descriptors my man? (maybe dont) - or maybe do!

        for (int i = 0; i < cellImages.Length; i++)
        {
            cellImages[i].texture = ourTexture;
            // cellNames[i].text = "";
        }
        ourTexture = null;

    }

    public void DestroyAll()
    {
        for (int i = 1; i < cellImages.Length; i++)
        {
            Destroy(cellImages[i].gameObject);
        }
    }

    public void CaptureCell(Subcell cellToBeCaptured)
    {

        // here right,
        GameObject sc = new GameObject();
        sc.AddComponent<MeshFilter>();
        sc.GetComponent<MeshFilter>().mesh = cellToBeCaptured.GetComponent<MeshFilter>().mesh;
        sc.AddComponent<MeshRenderer>();
        sc.GetComponent<MeshRenderer>().material = cellToBeCaptured.GetComponent<Renderer>().material;

        sc.transform.parent = spriteShotCamera.transform;
        sc.transform.localPosition = new Vector3(0, 0, 5);
        sc.transform.localScale = new Vector3(3.4f, 1.7f, 1.7f); // TODO DELETE ME LATER
        // TODO only call this function when a subcell has been locked!

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

        List<PDFCellContainer> usedCells = new List<PDFCellContainer>();
        bool getOut = false;

        foreach (RawImage i in cellImages)
        {
            if (i.gameObject.GetComponent<PDFCellContainer>()!=null)
            {
                usedCells.Add(i.gameObject.GetComponent<PDFCellContainer>());
            }
        }

        foreach (PDFCellContainer used in usedCells)
        {
            if (cellToBeCaptured == used.mycell)
            {
                cellImages[used.cellNumber].texture = ourTexture;
                getOut = true;
            }
        }

        if (getOut == true)
        {
            ourTexture = null;
            return;
        }

        else
        {
            cellImages[cachedCellNumber].GetComponent<PDFCellContainer>().mycell = cellToBeCaptured;
            cellImages[cachedCellNumber].GetComponent<PDFCellContainer>().cellNumber = cachedCellNumber;
            cellImages[cachedCellNumber].GetComponent<PDFCellContainer>().title.text = cellToBeCaptured.ServiceDat.ServiceName;
            cellImages[cachedCellNumber].texture = ourTexture;

            cachedCellNumber++;

            if (cachedCellNumber >= cellImages.Length)
            {
                cachedCellNumber = 0;
            }
        }
        ourTexture = null;
        Debug.Log("Cell capture finished!");
    }

    public void DoPDF()
    {
        StartCoroutine("GenerateEntirePDF");
    }

    public IEnumerator GenerateEntirePDF()
    {
        
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
        // PageTwo.SetActive(false);
        byte[] mBytes;
        mBytes = finalTexture.EncodeToPNG();
        string tempPath = Application.persistentDataPath + "/" + "OpenMind" + ".pdf";

        // CONSIDER ADDING DATE AND CLIENT NAME TO THE TEMPPATH ABOVE.

        File.WriteAllBytes(tempPath, mBytes);
        // DestroyAll();

#if UNITY_IOS
        if (emailStrings.Length != 0)
        {
            //TODO crap check to determine whether we are SAVING IT (open with whatever) or EMAILING IT (prepare for mailshot)
            OpenPDFThenEmail(tempPath, files[0], files[1], emailStrings[0], emailStrings[1], emailStrings[2]);
        }
        else
        {
            OpenPDF(tempPath, files[0], files[1]);
        }
#endif
        PageOne.transform.parent.GetComponent<Canvas>().enabled = false;
        DestroyAll();
    

    }



    }


