using Microsoft.Azure.Kinect.BodyTracking;
using Microsoft.Azure.Kinect.Sensor;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectManager : MonoBehaviour
{
    [SerializeField]
    //private Text HEADtxt, PELVIStxt, HANDLtxt, HANDRtxt, FOOTLtxt, FOOTRtxt;

    public GameObject H, P, HL, HR, FL, FR;

    private Device kinect;
    private Tracker tracker;

    private void Awake()
    {
        this.kinect = Device.Open(0);
        this.kinect.StartCameras(new DeviceConfiguration
        {
            ColorFormat = ImageFormat.ColorBGRA32,
            ColorResolution = ColorResolution.R720p,
            DepthMode = DepthMode.NFOV_2x2Binned,
            SynchronizedImagesOnly = true,
            CameraFPS = FPS.FPS30
        });
        this.tracker = Tracker.Create(this.kinect.GetCalibration(), TrackerConfiguration.Default);
    }
    // Start is called before the first frame update
    void Start()
    {
       /* HEADtxt = GameObject.Find("HEADText").GetComponent<Text>();
        PELVIStxt = GameObject.Find("PELVISText").GetComponent<Text>();
        HANDLtxt = GameObject.Find("HANDLText").GetComponent<Text>();
        HANDRtxt = GameObject.Find("HANDRText").GetComponent<Text>();
        FOOTLtxt = GameObject.Find("FOOTLText").GetComponent<Text>();
        FOOTRtxt = GameObject.Find("FOOTRText").GetComponent<Text>();
       */
        Task t = CaptureLooper();
    }

    private async Task CaptureLooper()
    {
        while (true)
        {
            using (var capture =await Task.Run(() => this.kinect.GetCapture()).ConfigureAwait(true))
            {
                this.tracker.EnqueueCapture(capture);
                var frame = tracker.PopResult();
                if (frame.NumberOfBodies > 0)
                {
                    var skelton = frame.GetBodySkeleton(0);
                    var headJoint = skelton.GetJoint(JointId.Head);
                    var pelvisJoint = skelton.GetJoint(JointId.Pelvis);
                    var handLJoint = skelton.GetJoint(JointId.HandLeft);
                    var handRJoint = skelton.GetJoint(JointId.HandRight);
                    var footLJoint = skelton.GetJoint(JointId.FootLeft);
                    var footRJoint = skelton.GetJoint(JointId.FootRight);
                    var elbowLJoint = skelton.GetJoint(JointId.ElbowLeft);
                    var elbowRJoint = skelton.GetJoint(JointId.ElbowRight);
                    var shoulderLJoint = skelton.GetJoint(JointId.ShoulderLeft);
                    var shoulderRJoint = skelton.GetJoint(JointId.ShoulderRight);
                    var chestJoint = skelton.GetJoint(JointId.SpineChest);
                    var kneeLJoint = skelton.GetJoint(JointId.KneeLeft);
                    var kneeRJoint = skelton.GetJoint(JointId.KneeRight);

                    /*  HEADtxt.text = "X:" + headJoint.Position.X + ",Y:" + headJoint.Position.Y + "Z:" + headJoint.Position.Z;
                      PELVIStxt.text = "X:" + pelvisJoint.Position.X + ",Y:" + pelvisJoint.Position.Y + "Z:" + pelvisJoint.Position.Z;
                      HANDLtxt.text = "X:" + handLJoint.Position.X + ",Y:" + handLJoint.Position.Y + "Z:" + handLJoint.Position.Z;
                      HANDRtxt.text = "X:" + handRJoint.Position.X + ",Y:" + handRJoint.Position.Y + "Z:" + handRJoint.Position.Z;
                      FOOTLtxt.text = "X:" + footLJoint.Position.X + ",Y:" + footLJoint.Position.Y + "Z:" + footLJoint.Position.Z;
                      FOOTRtxt.text = "X:" + footRJoint.Position.X + ",Y:" + footRJoint.Position.Y + "Z:" + footRJoint.Position.Z;
                    */

                    Vector3 HEAD = new Vector3(-headJoint.Position.X/50, -headJoint.Position.Y / 50, -headJoint.Position.Z / 1000);
                    Vector3 PELVIS = new Vector3(-pelvisJoint.Position.X / 50, -pelvisJoint.Position.Y / 50, -pelvisJoint.Position.Z / 1000);
                    Vector3 HANDL = new Vector3(-handLJoint.Position.X / 50, -handLJoint.Position.Y / 50, -handLJoint.Position.Z / 1000);
                    Vector3 HANDR = new Vector3(-handRJoint.Position.X / 50, -handRJoint.Position.Y / 50, -handRJoint.Position.Z / 1000);
                    Vector3 FOOTL = new Vector3(-footLJoint.Position.X / 50, -footLJoint.Position.Y / 50, -footLJoint.Position.Z / 1000);
                    Vector3 FOOTR = new Vector3(-footRJoint.Position.X / 50, -footRJoint.Position.Y / 50, -footRJoint.Position.Z / 1000);

                    H.transform.localPosition = HEAD;
                    P.transform.localPosition=PELVIS;
                    HL.transform.localPosition =(HANDL);
                    HR.transform.localPosition =(HANDR);
                    FL.transform.localPosition = (FOOTL);
                    FR.transform.localPosition = (FOOTR);
                }
            }
        }
    }

    private void OnDestroy()
    {
        this.kinect.StopCameras();
    }
}
