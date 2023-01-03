using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading.Tasks;

public class ClientCsharp : MonoBehaviour
{

    public static class Define
    {
        public static readonly int BoneNum = 32;
    }

    [System.Serializable]
    public class QuaList
    {
        public double X;
        public double Y;
        public double Z;
        public double W;
    }

    [System.Serializable]
    public class QuaAr
    {
        public QuaList[] qualist;
    }

    private Quaternion[] BoneQua;
    private Animator AvatarAnimator;
    private Transform[] AvatarBone;
    private WebSocket ws;
    private QuaAr quaar;

    bool wsopen,synced;
    int i, j;

    private void Start()
    {
        //�A�o�^�[�̃{�[����Transform���擾
        AvatarAnimator = GameObject.Find("SimpleSkeleton2").GetComponent<Animator>();

        //�T�[�o�����M����p��JSON�f�[�^�̌`���Ɖ�]���i�[����z���錾
        quaar = new QuaAr();
        quaar.qualist = new QuaList[Define.BoneNum];
        for (i = 0; i < Define.BoneNum; i++) quaar.qualist[i] = new QuaList();
        BoneQua = new Quaternion[Define.BoneNum];

        wsopen = false;

        Task B = BoneReceived();

    }

    private async Task BoneReceived()
    {
        await Task.Run(() =>
        {
            SyncStart();

            while (true)
            {
                ws.OnMessage += (sender, e) =>
                {
                    Debug.Log(e.Data);
                    JsonUtility.FromJsonOverwrite(e.Data.ToString(),quaar);
                    Debug.Log(quaar.qualist[1].X);
                    foreach (KeyValuePair<HumanBodyBones, int> pair in this.boneperseint)
                    {
                        var jointId = pair.Value;

                        BoneQua[jointId].x = (float)quaar.qualist[jointId].X;
                        BoneQua[jointId].y = (float)quaar.qualist[jointId].Y;
                        BoneQua[jointId].z = (float)quaar.qualist[jointId].Z;
                        BoneQua[jointId].w = (float)quaar.qualist[jointId].W;
                        AvatarAnimator.GetBoneTransform(pair.Key).rotation = BoneQua[jointId]; // HumanoidAvatar�̊e�֐߂ɉ�]�𓖂č���
                    }

                };
            }

        });
    }

    private void Update()
    {
        

    }

    public void SyncStart()
    {
        ws = new WebSocket("ws://localhost:3000/");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        ws.Connect();
        wsopen = true;
    }


    private void OnDestroy()
    {
        ws.Close();
    }

    private readonly Dictionary<HumanBodyBones, int> boneperseint = new Dictionary<HumanBodyBones, int>() // Kinect���̊e�֐߂�HumanoidAvatar�̊e�֐߂̕R�Â��B
        {
            // �㔼�g
            {HumanBodyBones.Hips, 0},
            {HumanBodyBones.Head, 26},
            {HumanBodyBones.Neck, 3},
            {HumanBodyBones.Chest, 2},
            {HumanBodyBones.Spine, 1},

            // ���r
            {HumanBodyBones.LeftShoulder, 4},
            {HumanBodyBones.LeftUpperArm, 5},
            {HumanBodyBones.LeftLowerArm, 6},
            {HumanBodyBones.LeftHand, 7},

            // �E�r
            {HumanBodyBones.RightShoulder, 11},
            {HumanBodyBones.RightUpperArm, 12},
            {HumanBodyBones.RightLowerArm, 13},
            {HumanBodyBones.RightHand, 14},

            // ���r
            {HumanBodyBones.LeftUpperLeg, 18},
            {HumanBodyBones.LeftLowerLeg, 19},
            {HumanBodyBones.LeftFoot, 20},

            // �E�r
            {HumanBodyBones.RightUpperLeg, 22},
            {HumanBodyBones.RightLowerLeg, 23},
            {HumanBodyBones.RightFoot, 24},

            // ����
            {HumanBodyBones.LeftToes, 21},
            {HumanBodyBones.RightToes, 25},

            // ����
            {HumanBodyBones.LeftIndexProximal, 9},
            {HumanBodyBones.LeftIndexIntermediate, 9},
            {HumanBodyBones.LeftIndexDistal, 9},
            {HumanBodyBones.LeftMiddleProximal, 9},
            {HumanBodyBones.LeftMiddleIntermediate, 9},
            {HumanBodyBones.LeftMiddleDistal, 9},
            {HumanBodyBones.LeftRingProximal, 9},
            {HumanBodyBones.LeftRingIntermediate, 9},
            {HumanBodyBones.LeftRingDistal, 9},
            {HumanBodyBones.LeftLittleProximal, 9},
            {HumanBodyBones.LeftLittleIntermediate, 9},
            {HumanBodyBones.LeftLittleDistal, 9},

            // �E��
            {HumanBodyBones.RightIndexProximal, 16},
            {HumanBodyBones.RightIndexIntermediate, 16},
            {HumanBodyBones.RightIndexDistal, 16},
            {HumanBodyBones.RightMiddleProximal, 16},
            {HumanBodyBones.RightMiddleIntermediate, 16},
            {HumanBodyBones.RightMiddleDistal, 16},
            {HumanBodyBones.RightRingProximal, 16},
            {HumanBodyBones.RightRingIntermediate, 16},
            {HumanBodyBones.RightRingDistal, 16},
            {HumanBodyBones.RightLittleProximal, 16},
            {HumanBodyBones.RightLittleIntermediate, 16},
            {HumanBodyBones.RightLittleDistal, 16},

            // ���w
            {HumanBodyBones.LeftThumbProximal, 8},
            {HumanBodyBones.LeftThumbIntermediate, 10},
            {HumanBodyBones.LeftThumbDistal, 10},

            // �E�w
            {HumanBodyBones.RightThumbProximal, 15},
            {HumanBodyBones.RightThumbIntermediate, 17},
            {HumanBodyBones.RightThumbDistal, 17},
        };
}
