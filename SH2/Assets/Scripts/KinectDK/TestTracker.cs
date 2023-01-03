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
}