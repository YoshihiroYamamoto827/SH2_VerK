using Microsoft.Azure.Kinect.BodyTracking;
using Microsoft.Azure.Kinect.Sensor;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AvatarTracker : MonoBehaviour
{
    [SerializeField]
    private GameObject avatarRoot; // �ʒu�ړ����邽�߂�3D���f���̐e��o�^

    private Device kinect;
    private Tracker tracker;
    private Animator animator;
    public Quaternion[] BoneQua;
    SyncTest sync;
    public int o = 0;

    void Awake()
    {
        this.kinect = Device.Open(0); // Kinect���N���B
        this.kinect.StartCameras(new DeviceConfiguration
        {
            ColorFormat = ImageFormat.ColorBGRA32,
            ColorResolution = ColorResolution.R720p,
            DepthMode = DepthMode.NFOV_2x2Binned,
            SynchronizedImagesOnly = true,
            CameraFPS = FPS.FPS30
        });

        this.tracker = Tracker.Create(this.kinect.GetCalibration(), TrackerConfiguration.Default);
        this.animator = this.GetComponent<Animator>();
    }

    void Start()
    {
        BoneQua = new Quaternion[32];
        Task t = CaptureLooper(); // Kinect����̃L���v�`���[�f�[�^��Task�ŉ񂵂ČJ��Ԃ��擾���܂��B
    }

    private async Task CaptureLooper()
    {

        while (true)
        {
            using (var capture = await Task.Run(() => this.kinect.GetCapture()).ConfigureAwait(true))
            {
                this.tracker.EnqueueCapture(capture);
                var frame = tracker.PopResult();
                if (frame.NumberOfBodies > 0) // �L���v�`���[���Ă���l�̂̐����`�F�b�N�B�l�̖����̏�ԂŃg���b�L���O����Ǝ��ɂ܂��B
                {
                    var skeleton = frame.GetBodySkeleton(0);
                    foreach (KeyValuePair<HumanBodyBones, JointId> pair in this.boneJointMap)
                    {
                        var jointId = pair.Value;
                        var jointRotation = skeleton.GetJoint(jointId).Quaternion;
                        var quaternion = new Quaternion(jointRotation.X, jointRotation.Y, jointRotation.Z, jointRotation.W);
                        var adjustedQuaternion = quaternion * this.GetQuaternionOffset(jointId); // �֐߂ɂ���ď�������قȂ�̂Œ������܂��B
                        this.animator.GetBoneTransform(pair.Key).rotation = adjustedQuaternion; // HumanoidAvatar�̊e�֐߂ɉ�]�𓖂č���ł����܂��B
                        BoneQua[(int)jointId] = adjustedQuaternion;
                        //Debug.Log(BoneQua[(int)jointId]);
                    }

                    var jointPos = frame.GetBodySkeleton(0).GetJoint(JointId.Pelvis).Position; // �g���b�L���O�������̈ʒu���A�o�^�[�̈ʒu���ɓ��č��݂܂��B
                    this.avatarRoot.transform.localPosition = new Vector3(-jointPos.X / 500, -jointPos.Y / 500, -jointPos.Z / 1000); // ���̂܂܂��Ɠ���������̂Œ����B
                }
            }
        }
    }

    private void OnDestroy()
    {
        this.kinect.StopCameras(); // �A�v���P�[�V�����I������Kinect���I�������܂��B�I�����Ȃ��Ǝ��̋N����hung���܂��B
    }

    private Quaternion GetQuaternionOffset(JointId jointId) // �֐ߖ��̕��������ł��B��q�B
    {
        switch (jointId)
        {
            case JointId.Pelvis:
            case JointId.SpineChest:
            case JointId.SpineNavel:
            case JointId.Neck:
            case JointId.Head:
                return Quaternion.Euler(90, 0, 90);
            case JointId.HipLeft:
            case JointId.KneeLeft:
            case JointId.AnkleLeft:
                return Quaternion.Euler(90, 0, 270);
            case JointId.HipRight:
            case JointId.KneeRight:
            case JointId.AnkleRight:
                return Quaternion.Euler(270, 0, 90);
            case JointId.ClavicleLeft:
            case JointId.ShoulderLeft:
            case JointId.ElbowLeft:
            case JointId.ThumbLeft:
                return Quaternion.Euler(180, 0, 90);
            case JointId.ClavicleRight:
            case JointId.ShoulderRight:
            case JointId.ElbowRight:
            case JointId.ThumbRight:
                return Quaternion.Euler(0, 0, 270);
            case JointId.FootLeft:
                return Quaternion.Euler(180, 90, 0);
            case JointId.FootRight:
                return Quaternion.Euler(180, 270, 180);
            case JointId.WristLeft:
            case JointId.HandLeft:
            case JointId.HandTipLeft:
                return Quaternion.Euler(0, 0, 90);
            case JointId.WristRight:
            case JointId.HandRight:
            case JointId.HandTipRight:
                return Quaternion.Euler(270, 0, 270);
        }

        return Quaternion.Euler(0, 0, 0);
    }

    private readonly Dictionary<HumanBodyBones, JointId> boneJointMap = new Dictionary<HumanBodyBones, JointId>() // Kinect���̊e�֐߂�HumanoidAvatar�̊e�֐߂̕R�Â��B
        {
            // �㔼�g
            {HumanBodyBones.Hips, JointId.Pelvis},
            {HumanBodyBones.Head, JointId.Head},
            {HumanBodyBones.Neck, JointId.Neck},
            {HumanBodyBones.Chest, JointId.SpineChest},
            {HumanBodyBones.Spine, JointId.SpineNavel},

            // ���r
            {HumanBodyBones.LeftShoulder, JointId.ClavicleLeft},
            {HumanBodyBones.LeftUpperArm, JointId.ShoulderLeft},
            {HumanBodyBones.LeftLowerArm, JointId.ElbowLeft},
            {HumanBodyBones.LeftHand, JointId.WristLeft},

            // �E�r
            {HumanBodyBones.RightShoulder, JointId.ClavicleRight},
            {HumanBodyBones.RightUpperArm, JointId.ShoulderRight},
            {HumanBodyBones.RightLowerArm, JointId.ElbowRight},
            {HumanBodyBones.RightHand, JointId.WristRight},

            // ���r
            {HumanBodyBones.LeftUpperLeg, JointId.HipLeft},
            {HumanBodyBones.LeftLowerLeg, JointId.KneeLeft},
            {HumanBodyBones.LeftFoot, JointId.AnkleLeft},

            // �E�r
            {HumanBodyBones.RightUpperLeg, JointId.HipRight},
            {HumanBodyBones.RightLowerLeg, JointId.KneeRight},
            {HumanBodyBones.RightFoot, JointId.AnkleRight},

            // ����
            {HumanBodyBones.LeftToes, JointId.FootLeft},
            {HumanBodyBones.RightToes, JointId.FootRight},

            // ����
            {HumanBodyBones.LeftIndexProximal, JointId.HandTipLeft},
            {HumanBodyBones.LeftIndexIntermediate, JointId.HandTipLeft},
            {HumanBodyBones.LeftIndexDistal, JointId.HandTipLeft},
            {HumanBodyBones.LeftMiddleProximal, JointId.HandTipLeft},
            {HumanBodyBones.LeftMiddleIntermediate, JointId.HandTipLeft},
            {HumanBodyBones.LeftMiddleDistal, JointId.HandTipLeft},
            {HumanBodyBones.LeftRingProximal, JointId.HandTipLeft},
            {HumanBodyBones.LeftRingIntermediate, JointId.HandTipLeft},
            {HumanBodyBones.LeftRingDistal, JointId.HandTipLeft},
            {HumanBodyBones.LeftLittleProximal, JointId.HandTipLeft},
            {HumanBodyBones.LeftLittleIntermediate, JointId.HandTipLeft},
            {HumanBodyBones.LeftLittleDistal, JointId.HandTipLeft},

            // �E��
            {HumanBodyBones.RightIndexProximal, JointId.HandTipRight},
            {HumanBodyBones.RightIndexIntermediate, JointId.HandTipRight},
            {HumanBodyBones.RightIndexDistal, JointId.HandTipRight},
            {HumanBodyBones.RightMiddleProximal, JointId.HandTipRight},
            {HumanBodyBones.RightMiddleIntermediate, JointId.HandTipRight},
            {HumanBodyBones.RightMiddleDistal, JointId.HandTipRight},
            {HumanBodyBones.RightRingProximal, JointId.HandTipRight},
            {HumanBodyBones.RightRingIntermediate, JointId.HandTipRight},
            {HumanBodyBones.RightRingDistal, JointId.HandTipRight},
            {HumanBodyBones.RightLittleProximal, JointId.HandTipRight},
            {HumanBodyBones.RightLittleIntermediate, JointId.HandTipRight},
            {HumanBodyBones.RightLittleDistal, JointId.HandTipRight},

            // ���w
            {HumanBodyBones.LeftThumbProximal, JointId.HandLeft},
            {HumanBodyBones.LeftThumbIntermediate, JointId.ThumbLeft},
            {HumanBodyBones.LeftThumbDistal, JointId.ThumbLeft},

            // �E�w
            {HumanBodyBones.RightThumbProximal, JointId.HandRight},
            {HumanBodyBones.RightThumbIntermediate, JointId.ThumbRight},
            {HumanBodyBones.RightThumbDistal, JointId.ThumbRight},
        };

    public Quaternion[] SendBoneQua()
    {
        return BoneQua;
    }
}