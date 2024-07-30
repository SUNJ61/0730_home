using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Texture[] textures;
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private GameObject Effect;
    private AudioClip Expclip;
    private AudioSource Source;
    private MeshFilter filter;
    private Mesh[] meshes;

    private int HitCount = 0;
    private readonly string BarTexture = "BarrelTextures";
    private readonly string bulletStr = "BULLET";
    private readonly string EffStr = "ExpEffect";
    private readonly string ExploreSound = "Sound/grenade_exp2";
    private readonly string meshestag = "Meshes";
    void Start()
    {
        textures = Resources.LoadAll<Texture>(BarTexture);
        meshRenderer = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        Effect = Resources.Load<GameObject>(EffStr);

        meshRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
        Source = GetComponent<AudioSource>();
        Expclip = Resources.Load<AudioClip>(ExploreSound);

        filter = GetComponent<MeshFilter>();
        meshes = Resources.LoadAll<Mesh>(meshestag);
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag(bulletStr))
        {
            if(++HitCount == 5)
            {
                ExpBarrel();
            }
        }
    }
    private void ExpBarrel()
    {
        Vector3 hitPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject eff = Instantiate(Effect, transform.position, Quaternion.identity);
        Destroy(eff, 2.0f);
        Source.PlayOneShot(Expclip, 1.0f);

        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 6 | 1 << 7 | 1 << 8 | 1 << 9);
        foreach (Collider col in Cols) //위에서 담은 배럴 콜라이더를 다 꺼낸다.
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f;
                rigidbody.AddExplosionForce(1000, transform.position, 20.0f, 1200f);
                col.gameObject.SendMessage("Die", col, SendMessageOptions.DontRequireReceiver);
                col.gameObject.SendMessage("ExpHp", col, SendMessageOptions.DontRequireReceiver);
            }
            Invoke("BarrelMassChange", 3.0f);
        }
        int Idx = Random.Range(0, meshes.Length);
        filter.sharedMesh = meshes[Idx];
        GetComponent <MeshCollider>().sharedMesh = meshes[Idx];
    }
    private void BarrelMassChange()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, 20f, 1 << 9);
        foreach(Collider col in Cols)
        {
            Rigidbody rigidbody = col.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.mass = 60.0f;
            }
        }
    }

}
