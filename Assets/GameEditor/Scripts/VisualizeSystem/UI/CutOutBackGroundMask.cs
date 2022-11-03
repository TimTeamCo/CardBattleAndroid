using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

//No usages really need?
public class CutOutBackGroundMask : Image
{
    [SerializeField] private GameObject _backGround;

    public override Material material
    {
        get
        {
            //Material material = new Material(base.material);
            _backGround.GetComponent<Material>().SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return base.material;
        }
    }
}
