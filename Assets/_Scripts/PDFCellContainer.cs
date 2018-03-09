using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PDFCellContainer : MonoBehaviour {

    public Text title;
    public Text descriptor;
    [HideInInspector]
    public Subcell mycell;
    [HideInInspector]
    public int cellNumber;
}
