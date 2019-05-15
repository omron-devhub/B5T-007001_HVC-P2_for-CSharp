/*---------------------------------------------------------------------------*/
/* Copyright(C)  2018  OMRON Corporation                                     */
/*                                                                           */
/* Licensed under the Apache License, Version 2.0 (the "License");           */
/* you may not use this file except in compliance with the License.          */
/* You may obtain a copy of the License at                                   */
/*                                                                           */
/*     http://www.apache.org/licenses/LICENSE-2.0                            */
/*                                                                           */
/* Unless required by applicable law or agreed to in writing, software       */
/* distributed under the License is distributed on an "AS IS" BASIS,         */
/* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  */
/* See the License for the specific language governing permissions and       */
/* limitations under the License.                                            */
/*---------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        private Thread demoThread = null;

        public bool isConnected = false;
        public bool isExecute = false;
        public bool isRegistExecute = false;
        public bool isEndRequest = false;
        public bool isExecuting = false;
        public bool isConnectRequest = false;
        public string connect_comport = "";

        public HVCP2Api hvc_p2_api;

        // Output image file name.
        public static string img_fname = "img.jpg";
        public static bool use_stb = false;

        // Read timeout value in seconds for serial communication.
        // If you use UART slow baudrate, please edit here.
        public static int timeout = 30;

        //-------------------------
        // B5T-007001 settings
        //-------------------------
        // Execute functions
        public static int exec_func = p2def.EX_FACE
                  | p2def.EX_DIRECTION
                  | p2def.EX_AGE
                  | p2def.EX_GENDER
                  | p2def.EX_EXPRESSION
                  | p2def.EX_RECOGNITION
                  | p2def.EX_BLINK
                  | p2def.EX_GAZE
                  | p2def.EX_BODY
                  | p2def.EX_HAND;

        // Output image type
        public static int output_img_type = p2def.OUT_IMG_TYPE_QVGA;
        //p2def.OUT_IMG_TYPE_NONE;
        // OUT_IMG_TYPE_QQVGA
        // OUT_IMG_TYPE_QVGA

        // HVC camera angle setting
        public static int hvc_camera_angle = p2def.HVC_CAM_ANGLE_0;
        // HVC_CAM_ANGLE_90
        // HVC_CAM_ANGLE_180
        // HVC_CAM_ANGLE_270

        // Threshold value settings
        public static int body_thresh = 500;         // Threshold for Human body detection [1 to 1000]
        public static int hand_thresh = 500;         // Threshold for Hand detection       [1 to 1000]
        public static int face_thresh = 500;         // Threshold for Face detection       [1 to 1000]
        public static int recognition_thresh = 500;  // Threshold for Recognition          [0 to 1000]

        // Detection size setings
        public static int min_body_size = 30;      // Mininum human body detection size [20 to 8192]
        public static int max_body_size = 8192;    // Maximum human body detection size [20 to 8192]
        public static int min_hand_size = 40;      // Mininum hand detection size       [20 to 8192]
        public static int max_hand_size = 8192;    // Maximum hand detection size       [20 to 8192]
        public static int min_face_size = 64;      // Mininum face detection size       [20 to 8192]
        public static int max_face_size = 8192;    // Maximum face detection size       [20 to 8192]

        // Detection face angle settings
        public static int face_angle_yaw = p2def.HVC_FACE_ANGLE_YAW_30;
        // HVC_FACE_ANGLE_YAW_60
        // HVC_FACE_ANGLE_YAW_90
        public static int face_angle_roll = p2def.HVC_FACE_ANGLE_ROLL_15;
        // HVC_FACE_ANGLE_ROLL_45

        //-------------------------
        // STB library settings
        //-------------------------
        // Tracking parameters
        public static int max_retry_count = 2;         // Maximum tracking retry count            [0 to 30]
        public static int steadiness_param_pos = 30;   // Rectangle position steadiness parameter [0 to 100]
        public static int steadiness_param_size = 30;  // Rectangle size steadiness parameter     [0 to 100]

        // Steadiness parameters for Gender/Age estimation
        public static int pe_threshold_use = 300;  // Estimation result stabilizing threshold value
        //                                          [0 to 1000]
        public static int pe_min_UD_angle = -15;   // Minimum up-down angel threshold value    [-90 to 90]
        public static int pe_max_UD_angle = 20;    // Maxmum up-down angel threshold value     [-90 to 90]
        public static int pe_min_LR_angle = -30;   // Minimum left-right angel threshold value [-90 to 90]
        public static int pe_max_LR_angle = 30;    // Maxmum left-right angel threshold value  [-90 to 90]
        public static int pe_complete_frame_count = 5;
        // The number of previous frames applying to fix
        // stabilization.                           [1 to 20]

        // Steadiness parameters for Recognition
        public static int fr_threshold_use = 300;  // Recognition result stabilizing threshold value
        //                                          [0 to 1000]
        public static int fr_min_UD_angle = -15;   // Minimum up-down angel threshold value    [-90 to 90]
        public static int fr_max_UD_angle = 20;    // Maxmum up-down angel threshold value     [-90 to 90]
        public static int fr_min_LR_angle = -30;   // Minimum left-right angel threshold value [-90 to 90]
        public static int fr_max_LR_angle = 30;    // Maxmum left-right angel threshold value  [-90 to 90]
        public static int fr_complete_frame_count = 5;
        // The number of previous frames applying to fix
        // stabilization.                           [1 to 20]
        public static int fr_min_ratio = 60;       // Minimum account ratio in complete frame count.
        //                                           [0 to 100]
        delegate void StringArgReturningVoidDelegate(string text);
        delegate void VoidArgReturningVoidDelegate();

        public bool _check_connection(HVCP2Api hvc_p2_api, out HVCP2Wrapper.GET_VERSION_RET result)
        {
            HVCP2Wrapper.GET_VERSION_RET ret = hvc_p2_api.get_version();
            result = ret;

            if ((ret.response_code != 0) || (ret.hvc_type.IndexOf("B5T-007001") != 0))
            {
                this.SetText("Error: connection failure.");
                this.SetText(Environment.NewLine);
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public void _get_hvc_version(HVCP2Wrapper.GET_VERSION_RET result)
        {
            var sb = new StringBuilder("  HVC_GetVersion : ");
            sb.Append(result.hvc_type.ToString()).Append(result.major.ToString()).Append(".");
            sb.Append(result.minor.ToString()).Append(".").Append(result.release.ToString()).Append(".");
            sb.AppendLine(result.revision.ToString()).AppendLine();
            this.SetText(sb.ToString());
        }

        public void _regist_exec(HVCP2Api hvc_p2_api)
        {
            var out_register_image = new GrayscaleImage();

            var ret = hvc_p2_api.register_data(0, 0, out_register_image);

            if (ret == p2def.RESPONSE_CODE_NORMAL)
            {
                this.SetText("  Success to register. user_id=0 data_id=0");
                this.SetText(Environment.NewLine);
                out_register_image.save("registerd_img.jpg");
            }
            else if (ret == p2def.RESPONSE_CODE_NO_FACE)
            {
                this.SetText("  Number of faces that can be registered is 0.");
                this.SetText(Environment.NewLine);
            }
            else if (ret == p2def.RESPONSE_CODE_PLURAL_FACE)
            {
                this.SetText("  Number of detected faces is 2 or more.");
                this.SetText(Environment.NewLine);
            }
            else
            {
                // error
                this.SetText("  Error: Invalid register album. ");
                this.SetText(Environment.NewLine);
            }
        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            try
            {
                if (this.LogViewArea.InvokeRequired)
                {
                    var d = new StringArgReturningVoidDelegate(SetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.LogViewArea.AppendText(text);
                }
            }
            catch
            {
            }
        }

        private void SetToDefault()
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            try
            {
                if (this.InvokeRequired)
                {
                    var d = new VoidArgReturningVoidDelegate(SetToDefault);
                    this.Invoke(d, new object[] {  });
                }
                else
                {
                    this.btnExecute.Enabled = true;
                    this.btnExecute.Text = "Execute";

                    this.btnRegister.Enabled = true;

                    SetDetectionCheck(true);
                    this.StablirizationCheck.Enabled = true;
                    this.ComPortName.Enabled = true;

                    this.isConnected = false;
                    this.isExecute = false;
                    this.isExecuting = false;
                    this.isRegistExecute = false;
                    this.isConnectRequest = false;
                }
            }
            catch
            {
            }
        }

        private void SetDetectionCheck(bool enable)
        {
            this.BodyDetectionCheck.Enabled = enable;
            this.HandDetectionCheck.Enabled = enable;
            this.FaceDetectionCheck.Enabled = enable;
            this.FaceDirectionCheck.Enabled = enable;
            this.AgeDetectionCheck.Enabled = enable;
            this.GenderDetectionCheck.Enabled = enable;
            this.GazeDetectionCheck.Enabled = enable;
            this.BlinkDetectionCheck.Enabled = enable;
            this.ExpressionDetectionCheck.Enabled = enable;
            this.RecognitionDetectionCheck.Enabled = enable;
        }

        public void _set_hvc_p2_parameters(HVCP2Api hvc_p2_api)
        {
            // Sets camera angle
            var res_code = hvc_p2_api.set_camera_angle(hvc_camera_angle);

            if (res_code != p2def.RESPONSE_CODE_NORMAL)
            {
                this.SetText("Error: Invalid camera angle.");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets threshold
            res_code = hvc_p2_api.set_threshold(body_thresh, hand_thresh, face_thresh, recognition_thresh);

            if (res_code != p2def.RESPONSE_CODE_NORMAL)
            {
                this.SetText("Error: Invalid threshold.");
                this.SetText(Environment.NewLine);
                return;
            }
            // Sets detection size
            res_code = hvc_p2_api.set_detection_size(min_body_size, max_body_size,
                                                     min_hand_size, max_hand_size,
                                                     min_face_size, max_face_size);

            if (res_code != p2def.RESPONSE_CODE_NORMAL)
            {
                this.SetText("Error: Invalid detection size.");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets face angle
            res_code = hvc_p2_api.set_face_angle(face_angle_yaw, face_angle_roll);
            if (res_code != p2def.RESPONSE_CODE_NORMAL)
            {
                this.SetText("Error: Invalid face angle.");
                this.SetText(Environment.NewLine);
                return;
            }
        }

        public void _set_stb_parameters(HVCP2Api hvc_p2_api)
        {
            if (hvc_p2_api.use_stb != true)
            {
                return;
            }

            // Sets tracking retry count.
            var ret = hvc_p2_api.set_stb_tr_retry_count(max_retry_count);

            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_tr_retry_count().");
                this.SetText(Environment.NewLine);
                return;
            }

            //  Sets steadiness parameters
            ret = hvc_p2_api.set_stb_tr_steadiness_param(steadiness_param_pos, steadiness_param_size);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_tr_steadiness_param().");
                this.SetText(Environment.NewLine);
                return;
            }

            // -- Sets STB parameters for Gender/Age estimation
            //    Sets estimation result stabilizing threshold value
            ret = hvc_p2_api.set_stb_pe_threshold_use(pe_threshold_use);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_pe_threshold_use().");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets estimation result stabilizing angle
            ret = hvc_p2_api.set_stb_pe_angle_use(pe_min_UD_angle, pe_max_UD_angle,
                                                  pe_min_LR_angle, pe_max_LR_angle);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_pe_angle_use().");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets age/gender estimation complete frame count
            ret = hvc_p2_api.set_stb_pe_complete_frame_count(pe_complete_frame_count);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_pe_complete_frame_count().");
                this.SetText(Environment.NewLine);
                return;
            }

            // -- Sets STB parameters for Recognition
            //    Sets recognition stabilizing threshold value
            ret = hvc_p2_api.set_stb_fr_threshold_use(fr_threshold_use);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_fr_threshold_use().");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets recognition stabilizing angle
            ret = hvc_p2_api.set_stb_fr_angle_use(fr_min_UD_angle, fr_max_UD_angle,
                                                  fr_min_LR_angle, fr_max_LR_angle);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_fr_angle_use().");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets recognition stabilizing complete frame count
            ret = hvc_p2_api.set_stb_fr_complete_frame_count(fr_complete_frame_count);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_fr_complete_frame_count().");
                this.SetText(Environment.NewLine);
                return;
            }

            // Sets recognition minimum account ratio
            ret = hvc_p2_api.set_stb_fr_min_ratio(fr_min_ratio);
            if (ret != 0)
            {
                this.SetText("Error: Invalid parameter. set_stb_fr_min_ratio().");
                this.SetText(Environment.NewLine);
                return;
            }
        }

        public Main()
        {
            InitializeComponent();

            var PortList = SerialPort.GetPortNames();

            ComPortName.Items.Clear();

            foreach (string PortName in PortList)
            {
                ComPortName.Items.Add(PortName);
            }
            if (ComPortName.Items.Count > 0)
            {
                ComPortName.SelectedIndex = 0;
                this.btnExecute.Enabled = true;
                this.btnRegister.Enabled = true;
            }

            this.demoThread = new Thread(new ThreadStart(this.mainloop));

            this.demoThread.Start();
        }

        public void mainloop()
        {
            var hvc_tracking_result = new HVCTrackingResult();
            var img = new GrayscaleImage();

            HVCP2Api.EXECUTE_RET exec_ret;
            var sw = new Stopwatch();

            while (true)
            {
                try
                {
                    if (this.isConnectRequest)
                    {
                        var connector = new SerialConnector();

                        var exec_func = 0x00;

                        if (BodyDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_BODY;
                        }
                        if (HandDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_HAND;
                        }
                        if (FaceDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_FACE;
                        }
                        if (FaceDirectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_DIRECTION;
                        }
                        if (AgeDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_AGE;
                        }
                        if (GenderDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_GENDER;
                        }
                        if (GazeDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_GAZE;
                        }
                        if (BlinkDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_BLINK;
                        }
                        if (ExpressionDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_EXPRESSION;
                        }
                        if (RecognitionDetectionCheck.Checked == true)
                        {
                            exec_func += p2def.EX_RECOGNITION;
                        }

                        this.hvc_p2_api = new HVCP2Api(connector, exec_func, StablirizationCheck.Checked);

                        var comnum = int.Parse(this.connect_comport.Substring(3));
                        var ret = this.hvc_p2_api.connect(comnum, 9600, timeout * 1000);
                        if (ret == true)
                        {
                            HVCP2Wrapper.GET_VERSION_RET result;
                            this.isConnectRequest = false;
                            var retcode = _check_connection(this.hvc_p2_api, out result);
                            if (retcode == true)
                            {
                              _get_hvc_version(result);
                              
                              _set_hvc_p2_parameters(this.hvc_p2_api);

                              // Sets STB library parameters
                              _set_stb_parameters(hvc_p2_api);
                              
                              this.isConnected = true;
                            }
                            else
                            {
                                this.SetToDefault();
                                try
                                {
                                    this.hvc_p2_api.disconnect();
                                }
                                catch
                                {
                                }
                            }
                        }
                    }

                    if (this.isConnected)
                    {
                        if (this.isRegistExecute)
                        {
                            _regist_exec(this.hvc_p2_api);
                            this.SetToDefault();
                            try
                            {
                                this.hvc_p2_api.disconnect();
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            this.isExecuting = true;

                            sw.Reset();
                            sw.Start();
                            exec_ret = hvc_p2_api.execute(output_img_type, hvc_tracking_result, img);
                            sw.Stop();
                            this.isExecuting = false;

                            if (output_img_type != p2def.OUT_IMG_TYPE_NONE)
                            {
                                img.save(img_fname);
                            }
                            this.SetText(string.Format("  ==== Elapsed time:{0}[msec] ====", sw.ElapsedMilliseconds));
                            this.SetText(Environment.NewLine);
                            this.SetText(hvc_tracking_result.ToString());
                            this.SetText(Environment.NewLine);
                            this.SetText(string.Format("  Press Stop Button to end:", sw.ElapsedMilliseconds));
                            this.SetText(Environment.NewLine);
                            this.SetText(Environment.NewLine);
                            System.Windows.Forms.Application.DoEvents();
                        }
                    }
                    
                    if (this.isEndRequest)
                    {
                        try
                        {
                            this.hvc_p2_api.disconnect();
                        }
                        catch
                        {
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    this.SetText(string.Format("Unexpected exception : {0}", ex.Message));
                    this.SetText(Environment.NewLine);
                    this.SetToDefault();
                    try
                    {
                        this.hvc_p2_api.disconnect();
                    }
                    catch
                    {
                    }
                }

                System.Threading.Thread.Sleep(10);
            }

        }
        
        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.connect_comport = ComPortName.Text;
            this.isConnectRequest = true;

            this.btnExecute.Enabled = false;

            SetDetectionCheck(false);
            this.StablirizationCheck.Enabled = false;
            this.ComPortName.Enabled = false;
            this.isRegistExecute = true;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (this.isExecute == false)
            {
                this.connect_comport = ComPortName.Text;
                this.isConnectRequest = true;

                this.btnRegister.Enabled = false;

                SetDetectionCheck(false);
                this.StablirizationCheck.Enabled = false;
                this.ComPortName.Enabled = false;

                this.isExecute = true;
                btnExecute.Text = "Stop";
            }
            else
            {
                if (isConnected == false)
                {
                    return;
                }

                this.isExecute = false;
                while (this.isExecuting)
                {
                    System.Threading.Thread.Sleep(10);
                }

                if (hvc_p2_api.use_stb == true)
                {
                    try
                    {
                        hvc_p2_api.reset_tracking();
                    }
                    catch
                    {
                    }
                }
                
                this.SetToDefault();
                
                try
                {
                    this.hvc_p2_api.disconnect();
                }
                catch
                {
                }

            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isEndRequest = true;
        }
        
        private void CpyLogViewArea_Click(object sender, EventArgs e)
        {
            if (this.LogViewArea.Text.Length > 0)
            {
                Clipboard.SetText(this.LogViewArea.Text.ToString());
            }
        }
        private void ClrLogViewArea_Click(object sender, EventArgs e)
        {
            this.LogViewArea.ResetText();
        }
    }
}
