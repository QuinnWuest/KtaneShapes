using UnityEngine;

public struct ConsonantInfo
{
    public Vector3 ExtrusionMainInfo;
    public Vector3 ExtrusionAInfo;
    public Vector3 ExtrusionBInfo;
    public int MeshIx;
    public float Rotation;

    public ConsonantInfo(Vector3 emi, Vector3 eai, Vector3 ebi, int m, float r)
    {
        ExtrusionMainInfo = emi;
        ExtrusionAInfo = eai;
        ExtrusionBInfo = ebi;
        MeshIx = m;
        Rotation = r;
    }
}
