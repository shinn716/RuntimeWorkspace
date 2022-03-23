using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Text txtName = null;
    [SerializeField] private Button deleteBtn = null;
    [SerializeField] private Toggle itemBtn = null;

    public Transform Target = null;
    public Toggle ItemButton { get => itemBtn; }

    private void Start()
    {
        itemBtn.onValueChanged.AddListener(delegate { OnValueChangeToggle(itemBtn); } );
        deleteBtn.onClick.AddListener(delegate { StartCoroutine(Delete()); });
    }

    public void SetTarget(Transform obj)
    {
        Target = obj;
    }

    public void SetName(string name)
    {
        txtName.text = name;
    }

    private IEnumerator Delete()
    {
        Dashboard.Instance.Enable = false;
        yield return null;
        Destroy(Target.gameObject);
        yield return null;
        Dashboard.Instance.RefleshContainer();
        Dashboard.Instance.SetToggleGroupDefault();
        yield return null;
        Dashboard.Instance.Enable = true;
        Destroy(gameObject);
    }

    private void OnValueChangeToggle(Toggle toggle)
    {
        //print($"[OnClick]{gameObject.name}");
        if (toggle.isOn)
        {
            itemBtn.targetGraphic.color = new Color32(0, 173, 239, 255);
            Dashboard.Instance.transformGizmo.AddTarget(Target);
        }
        else
        {
            itemBtn.targetGraphic.color = Color.white;
            Dashboard.Instance.transformGizmo.ClearTargets();
        }
    }
}
