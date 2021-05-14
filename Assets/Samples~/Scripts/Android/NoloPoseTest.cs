using UnityEngine;
namespace NXR.Samples
{
    public class NoloPoseTest : MonoBehaviour {
    public Transform parentTransform;
    public string type = "left";

    TextMesh textMesh;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
       if(parentTransform !=null) textMesh.text = type +".Rotation: "+parentTransform.eulerAngles.ToString() + ", Position: " + parentTransform.position.ToString();
	}


}
}