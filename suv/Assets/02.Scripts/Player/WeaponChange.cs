using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChange : MonoBehaviour
{
    public SkinnedMeshRenderer spas12;
    public MeshRenderer[] Ak47; //ak와 m4는 메쉬랜더가 다수이므로 배열 선언
    public MeshRenderer[] M4A1;
    public Animation ComBatSg;

    
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //Alpha1 = 키보드 위쪽 숫자 1이다.
        {
            WeaponChage_ak();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponChage_m4();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponChage_Shotgun();
        }
    }

    private void WeaponChage_Shotgun()
    {
        ComBatSg.Play("draw");
        foreach(MeshRenderer AK in Ak47)
            AK.enabled = false;
        foreach(MeshRenderer M4 in M4A1)
            M4.enabled = false;
        spas12.enabled = true;
    }

    private void WeaponChage_m4()
    {
        ComBatSg.Play("draw");
        foreach (MeshRenderer AK in Ak47)
            AK.enabled = false;
        foreach (MeshRenderer M4 in M4A1)
            M4.enabled = true;
        spas12.enabled = false;
    }

    private void WeaponChage_ak()
    {
        ComBatSg.Play("draw");
        foreach (MeshRenderer AK in Ak47) // ak의 매쉬를 활성화 시킨다.
            AK.enabled = true; ; //위에서 AK47은 매쉬정보를 저장한 변수로 선언되어 조절하면 매쉬 컨트롤.
        foreach (MeshRenderer M4 in M4A1) // m4의 매쉬를 비활성화 시킨다.
            M4.enabled = false;
        spas12.enabled = false; //샷건의 매쉬를 비활성화 시킨다.
    }
}
