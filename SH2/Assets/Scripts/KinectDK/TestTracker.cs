using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TestTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject avatarRoot; // 位置移動するために3Dモデルの親を登録

    private Animator animator;
    public Quaternion[] BoneQua;
    SyncTest sync;
    public int o = 0;
    private System.Numerics.Vector3 jointPos;
    private Vector3 adjustPos;

    void Awake()
    {

    }

    void Start()
    {
        BoneQua = new Quaternion[32];
        Task t = CaptureLooper(); // KinectからのキャプチャーデータはTaskで回して繰り返し取得します。
    }

    private async Task CaptureLooper()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                for (int i = 0; i < 32; i++)
                {
                    BoneQua[i] = new Quaternion(0, 1, 2, 3);
                }
                jointPos = new System.Numerics.Vector3(1, 1, 1);
                adjustPos = new Vector3(jointPos.X,jointPos.Y,jointPos.Z);
            }
        });
    }

    private void OnDestroy()
    {

    }

    public Quaternion[] SendBoneQua()
    {
        return BoneQua;
    }

    public Vector3 SendBasePos()
    {
        return adjustPos;
    }
}