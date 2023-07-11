using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    private void Awake()
    {
        Ball.ResetStaticData();
        GridValidObject.ResetStaticData();
    }

}
