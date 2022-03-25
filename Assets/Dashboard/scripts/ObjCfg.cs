using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjCfg : MonoBehaviour
{
    public string Address { get; private set; } = string.Empty;
    public string UUID { get; private set; } = string.Empty;

    public void SetCfg(string _address, string _uuid)
    {
        Address = _address;
        UUID = _uuid;
    }
}
