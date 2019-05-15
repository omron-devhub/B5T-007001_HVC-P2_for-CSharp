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
using System.Linq;
using System.Text;
using System.Diagnostics;

public class HVCP2Api
{
    // This class provide python full API for HVC-P2(B5T-007001) with STB library.
    public bool use_stb;
    public STB _stb;
    public HVCP2Wrapper _hvc_p2_wrapper;
    public int _exec_func;

    public struct EXECUTE_RET
    {
        public int response_code;
        public int stb_return;
    }

    /// <summary>Constructor
    /// </summary>
    /// <param name="connector">serial connector</param>
    /// <param name="exec_func">functions flag to be executed
    /// (e.g. p2def.EX_FACE | p2def.EX_AGE )</param>
    /// <param name="use_stabilizer">use_stb (bool): use STB library</param>
    /// <returns> void    public HVCP2Api(Connector connector, int exec_func, bool use_stabilizer)</returns>  
    public HVCP2Api(Connector connector, int exec_func, bool use_stabilizer)
    {
        bool _use_stb;

        this._hvc_p2_wrapper = new HVCP2Wrapper(connector);

        // Disable to use STB if using Hand detection only.
        if ((use_stabilizer == p2def.USE_STB_ON) && (exec_func == p2def.EX_HAND))
        {
            _use_stb = p2def.USE_STB_OFF;
        }
        else
        {
            _use_stb = use_stabilizer;
        }
        this.use_stb = _use_stb;

        // Adds face flag if using facial estimation function
        if ((exec_func & (p2def.EX_DIRECTION
                      | p2def.EX_AGE
                      | p2def.EX_GENDER
                      | p2def.EX_GAZE
                      | p2def.EX_BLINK
                      | p2def.EX_RECOGNITION
                      | p2def.EX_EXPRESSION)) != 0x00)
        {
            exec_func |= p2def.EX_FACE + p2def.EX_DIRECTION;
        }

        this._exec_func = exec_func;

        var stb_lib_name = "libSTB.dll";

        if (this.use_stb == true)
        {
            this._stb = new STB(stb_lib_name, exec_func);
        }

    }

    /// <summary>
    /// Connects to HVC-P2 by COM port via USB or UART interface.
    /// </summary>
    /// <param name="com_port">COM port ('COM3', '/dev/ttyACM0' etc. )</param>
    /// <param name="baudrate">baudrate (9600/38400/115200/230400/460800/921600)</param>
    /// <param name="timeout">timeout period(sec) for serial communication</param>
    /// <returns>bool: status    public bool connect(int com_port, int baudrate, int timeout)</returns>
    public bool connect(int com_port, int baudrate, int timeout)
    {

        return this._hvc_p2_wrapper.connect(com_port, baudrate, timeout);
    }

    public HVCP2Wrapper.GET_VERSION_RET get_version()
    {
        return this._hvc_p2_wrapper.get_version();
    }

    /// <summary>
    /// Sets camera angle.
    /// </summary>
    /// <param name="camera_angle">the angle used when facing the camera
    /// <para>        HVC_CAM_ANGLE_0   (00h):   0 degree</para>
    /// <para>        HVC_CAM_ANGLE_90  (01h):  90 degree</para>
    /// <para>        HVC_CAM_ANGEL_180 (02h): 180 degree</para>
    /// <para>        HVC_CAM_ANGEL_270 (03h): 270 degree</para>
    /// </param>
    /// <returns>int: response_code form B5T-007001.</returns>
    public int set_camera_angle(int camera_angle)
    {
        return this._hvc_p2_wrapper.set_camera_angle(camera_angle);
    }

    /// <summary>
    /// Gets camera angle.
    /// </summary>
    /// <returns>struct of (response_code, camera_angle)
    /// <para>    response_code (int): response code form B5T-007001</para>
    /// <para>    camera_angle (int): the angle used when facing the camera</para></returns>
    public HVCP2Wrapper.GET_CAMERA_ANGLE_RET get_camera_angle()
    {
        return this._hvc_p2_wrapper.get_camera_angle();
    }

    /// <summary>
    /// <para>Executes functions specified in the constructor.</para>
    /// <para>e.g. Face detection, Age estimation etc.</para>
    /// </summary>
    /// <param name="out_img_type">output image type
    /// <para>    OUT_IMG_TYPE_NONE  (00h): no image output</para>
    /// <para>    OUT_IMG_TYPE_QVGA  (01h): 320x240 pixel resolution(QVGA)</para>
    /// <para>         OUT_IMG_TYPE_QQVGA (02h): 160x120 pixel resolution(QQVGA)</para></param>
    /// <param name="tracking_result">the tracking result is stored</param>
    /// <param name="out_img">output image</param>
    /// <returns>struct of (response_code, stb_return)
    /// <para>    response_code (int): response code form B5T-007001</para>
    /// <para>    stb_return (bool): return status of STB library</para>
    /// </returns>
    public EXECUTE_RET execute(int out_img_type, HVCTrackingResult tracking_result, GrayscaleImage out_img)
    {
        EXECUTE_RET retvalue;

        retvalue.response_code = 0;
        retvalue.stb_return = 0;

        var frame_result = new HVCResult();
        retvalue.response_code = this._hvc_p2_wrapper.execute(this._exec_func, out_img_type, frame_result, out_img);
        tracking_result.clear();

        if ((this.use_stb == true) && (this._exec_func != p2def.EX_NONE))
        {
            var stb_in = new STBLibWrapper.STB_FRAME_RESULT();

            stb_in.bodys.body = new STBLibWrapper.STB_FRAME_RESULT_DETECTION[STB.STB_FRAME_NUM];
            stb_in.faces.face = new STBLibWrapper.STB_FRAME_RESULT_FACE[STB.STB_FRAME_NUM];

            frame_result.export_to_C_FRAME_RESULT(ref stb_in);

            var stb_out_f = new STBLibWrapper.STB_FACE[STB.STB_FRAME_NUM];
            var stb_out_b = new STBLibWrapper.STB_BODY[STB.STB_FRAME_NUM];

            STB.STB_EXECUTE_RET stb_ret = this._stb.execute(stb_in, stb_out_f, stb_out_b);

            if (stb_ret.retcode < 0)
            {
                // STB error
                retvalue.stb_return = stb_ret.retcode;

                return retvalue;
            }

            tracking_result.faces.append_C_FACE_RES35(this._exec_func, (int)stb_ret.face_count, stb_out_f);

            if ((this._exec_func & p2def.EX_DIRECTION) != 0)
            {
                tracking_result.faces.append_direction_list(frame_result.faces);
            }

            if ((this._exec_func & p2def.EX_GAZE) != 0)
            {
                tracking_result.faces.append_gaze_list(frame_result.faces);
            }

            if ((this._exec_func & p2def.EX_BLINK) != 0)
            {
                tracking_result.faces.append_blink_list(frame_result.faces);
            }

            if ((this._exec_func & p2def.EX_EXPRESSION) != 0)
            {
                tracking_result.faces.append_expression_list(frame_result.faces);
            }

            tracking_result.bodies.append_BODY_RES35(this._exec_func, (int)stb_ret.body_count, stb_out_b);
            tracking_result.hands.append_hand_list(frame_result.hands);
        }
        else
        {
            tracking_result.appned_FRAME_RESULT(frame_result);
        }
        return retvalue;
    }

    /// <summary>
    /// <para>Resets tracking.</para>
    /// <para>Note:</para>
    /// <para>   The tracking status will be cleared(i.e. TrackingID will be cleared),</para>
    /// <para>   but other settings will not cleared.</para>
    /// </summary>
    /// <returns>int: return status</returns>
    public int reset_tracking()
    {
        return this._stb.clear_stb_frame_results();
    }

    /// <summary>
    /// <para>Sets the thresholds value for Human body detection, Hand detection,</para>
    /// <para>Face detection and/or Recongnition.</para>
    /// </summary>
    /// <param name="body_thresh">Threshold value for Human body detection[1-1000]</param>
    /// <param name="hand_thresh">Threshold value for Hand detection[1-1000]</param>
    /// <param name="face_thresh">Threshold value for Face detection[1-1000]</param>
    /// <param name="recognition_thresh">Threshold value for Recognition[0-1000]</param>
    /// <returns>response_code form B5T-007001.</returns>
    public int set_threshold(int body_thresh, int hand_thresh, int face_thresh,
                                                      int recognition_thresh)
    {
        return this._hvc_p2_wrapper.set_threshold(body_thresh, hand_thresh, face_thresh, recognition_thresh);
    }

    /// <summary>
    /// <para>Sets the detection size for Human body detection, Hand detection</para>
    /// <para> and/or Face detection</para>
    /// </summary>
    /// <param name="min_body">Minimum detection size for Human body detection</param>
    /// <param name="max_body">Maximum detection size for Human body detection</param>
    /// <param name="min_hand">Minimum detection size for Hand detection</param>
    /// <param name="max_hand">Maximum detection size for Hand detection</param>
    /// <param name="min_face">Minimum detection size for Face detection</param>
    /// <param name="max_face">Maximum detection size for Face detection/param>
    /// <returns>response_code form B5T-007001.</returns>
    public int set_detection_size(int min_body, int max_body, int min_hand, int max_hand,
                           int min_face, int max_face)
    {
        return this._hvc_p2_wrapper.set_detection_size(min_body, max_body,
                                        min_hand, max_hand, min_face, max_face);
    }

    /// <summary>
    /// <para>Sets the face angle, i.e. the yaw angle range and the roll angle</para>
    /// <para> range for Face detection.</para>
    /// </summary>
    /// <param name="yaw_angle">
    /// <para>face direction yaw angle range.</para>
    /// <para>        HVC_FACE_ANGLE_YAW_30 (00h): +/-30 degree (frontal face)</para>
    /// <para>        HVC_FACE_ANGLE_YAW_60 (01h): +/-60 degree (half-profile face)</para>
    /// <para>        HVC_FACE_ANGLE_YAW_90 (02h): +/-90 degree (profile face)</para>
    /// </param>
    /// <param name="roll_angle">
    /// <para>face inclination roll angle range.</para>
    /// <para>        HVC_FACE_ANGLE_ROLL_15 (00h): +/-15 degree</para>
    /// <para>        HVC_FACE_ANGLE_ROLL_45 (01h): +/-45 degree</para></param>
    /// <returns>response_code form B5T-007001.</returns>
    public int set_face_angle(int yaw_angle, int roll_angle)
    {
        return this._hvc_p2_wrapper.set_face_angle(yaw_angle, roll_angle);
    }

    /// <summary>
    /// <para>Sets the UART baudrate.</para>
    /// <para>Note:</para>
    /// <para>   The setting can be done when the USB is connected and will have</para>
    /// <para>   no influence on the transmission speed as this is a command for UART</para>
    /// <para>   connection.</para>
    /// </summary>
    /// <param name="baudrate">
    /// <para>UART baudrate in bps.</para>
    /// <para>        (9600/38400/115200/230400/460800/921600)</para></param>
    /// <returns>response_code form B5T-007001.</returns>
    public int set_uart_baudrate(int baudrate)
    {
        return this._hvc_p2_wrapper.set_uart_baudrate(baudrate);
    }

    // ==========================================================================
    //  APIs for Album operation of Face recognition
    // ==========================================================================

    /// <summary>
    /// Registers data for Recognition and gets a normalized image.
    /// </summary>
    /// <param name="user_id">User ID [0-9]</param>
    /// <param name="data_id">Data ID [0-99]</param>
    /// <param name="out_register_image">normalized face image</param>
    /// <returns>response_code form B5T-007001.</returns>
    public int register_data(int user_id, int data_id, GrayscaleImage out_register_image)
    {
        return this._hvc_p2_wrapper.register_data(user_id, data_id, out_register_image);
    }

    /// <summary>
    /// Deletes a specified registered data. (Recognition)
    /// </summary>
    /// <param name="user_id">User ID [0-9]</param>
    /// <param name="data_id">Data ID [0-99]</param>
    /// <returns>response_code form B5T-007001.</returns>
    public int delete_data(int user_id, int data_id)
    {
        return this._hvc_p2_wrapper.delete_data(user_id, data_id);
    }

    /// <summary>
    /// Deletes a specified registerd user. (Recognition)
    /// </summary>
    /// <param name="user_id">User ID [0-9]</param>
    /// <returns>response_code form B5T-007001.</returns>
    public int delete_user(int user_id)
    {
        return this._hvc_p2_wrapper.delete_user(user_id);
    }

    /// <summary>
    /// Deletes all the registerd data. (Recognition)
    /// </summary>
    /// <returns>response_code form B5T-007001.</returns>
    public int delete_all_data()
    {
        return this._hvc_p2_wrapper.delete_all_data();
    }

    /// <summary>
    /// <para>Gets the registration info of a specified user. (Recognition)</para>
    /// <para>i.e. the presence or absence of registered data, for the specified user.</para>
    /// </summary>
    /// <param name="user_id">User ID [0-9]</param>
    /// <returns>
    /// <para>struct of (response_code, data_list)</para>
    /// <para>    response_code (int): response_code form B5T-007001.</para>
    /// <para>    data_list (list): data presence of registered data.</para></returns>
    public HVCP2Wrapper.get_user_data_value get_user_data(int user_id)
    {
        return this._hvc_p2_wrapper.get_user_data(user_id);
    }

    /// <summary>
    /// Saves the album on the host side. (Recognition)
    /// </summary>
    /// <returns>
    /// <para>struct of (response_code, album)</para>
    /// <para>  response_data (int): response_code form B5T-007001.</para>
    /// <para>  album (str): album</para></returns>
    public HVCP2Wrapper.save_album_value save_album()
    {
        return this._hvc_p2_wrapper.save_album();
    }

    /// <summary>
    /// Loads the album on the host side. (Recognition)
    /// </summary>
    /// <param name="album">album</param>
    /// <returns>response_code form B5T-007001.</returns>
    public int load_album(byte[] album)
    {
        return this._hvc_p2_wrapper.load_album(album);
    }

    /// <summary>
    /// <para>Saves the album on the flash ROM.  (Recognition)</para>
    /// <para>Note:</para>
    /// <para>   The processing time will be longer if there is a lot of data.</para>
    /// <para>   Album data already present on the flash ROM of the device will be</para>
    /// <para>   overwritten.</para>
    /// </summary>
    /// <returns>response_code form B5T-007001.</returns>
    public int save_album_to_flash()
    {
        return this._hvc_p2_wrapper.save_album_to_flash();
    }

    /// <summary>
    /// Reformats the album save area on the flash ROM. (Recognition)
    /// </summary>
    /// <returns>response_code form B5T-007001.</returns>
    public int reformat_flash()
    {
        return this._hvc_p2_wrapper.reformat_flash();
    }

    /// <summary>
    /// Disconnects to HVC-P2.
    /// </summary>
    public void disconnect()
    {
        this._hvc_p2_wrapper.disconnect();
        if (this.use_stb == true)
        {
            this._stb.stb_delete();
        }
    }

    // #==========================================================================
    // # APIs for STB library
    // #==========================================================================

    /// <summary>
    /// Gets the version number of STB library.
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, major, minor)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    major (int): major version number of STB library</para>
    /// <para>    minor (int): minor version number of STB library.</para>
    /// </returns>
    public STB.STB_VERSION_RET get_stb_version()
    {
        if (this._stb == null)
        {
            STB.STB_VERSION_RET ret;
            ret.retcode = 0;
            ret.major_version = 0;
            ret.minor_version = 0;
            return ret;
        }

        return this._stb.get_stb_version();
    }

    /// <summary>
    /// <para>Sets maximum tracking retry count.</para>
    /// <para>Set the number of maximum retry when not finding a face/human body while</para>
    /// <para>tracking. Terminates tracking as lost object when keeps failing for this</para>
    /// <para>maximum retry count.</para>
    /// </summary>
    /// <param name="max_retry_count">maximum tracking retry count. [0-300]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_tr_retry_count(int max_retry_count)
    {
        return this._stb.set_stb_tr_retry_count(max_retry_count);
    }

    /// <summary>
    /// Gets maximum retry count.
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, max_retry)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    max_retry_count (int): maximum tracking retry count.</para>
    /// </returns>
    public STB.STB_GETRETRYCOUNT_RET get_stb_tr_retry_count()
    {
        return this._stb.get_stb_tr_retry_count();
    }

    /// <summary>
    /// <para>Sets steadiness parameter of position and size.</para>
    /// <para>-- pos_steadiness_param</para>
    /// <para>For example, outputs the previous position coordinate data if the</para>
    /// <para>shifting measure is within 30%, existing position coordinate data if it</para>
    /// <para> has shift more than 30% when the rectangle position steadiness</para>
    /// <para> parameter has set as initial value of 30.</para>
    /// <para>-- size_steadiness_param</para>
    /// <para>For example, outputs the previous detecting size data if the changing</para>
    /// <para>measure is within 30%, existing size data if it has changed more than</para>
    /// <para>30% when the rectangle size steadiness parameter has set as initial</para>
    /// <para>value of 30.</para>
    /// </summary>
    /// <param name="pos_steadiness_param">rectangle position steadiness parameter[0-100]</param>
    /// <param name="size_steadiness_param">rectangle size steadiness parameter[0-100]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_tr_steadiness_param(int pos_steadiness_param, int size_steadiness_param)
    {
        return this._stb.set_stb_tr_steadiness_param(pos_steadiness_param, size_steadiness_param);
    }

    /// <summary>
    /// Gets steadiness parameter of position and size.
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, pos_steadiness_param, size_steadiness_param)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    pos_steadiness_param (int): rectangle position steadiness parameter</para>
    /// <para>    size_steadiness_param (int): rectangle size steadiness parameter</para></returns>
    public STB.STB_TR_STEADINESS_PARAM_RET get_stb_tr_steadiness_param()
    {
        return this._stb.get_stb_tr_steadiness_param();
    }

    /// <summary>
    /// <para>Sets estimation result stabilizing threshold value.</para>
    /// <para>Sets the stabilizing threshold value of Face direction confidence.</para>
    /// <para>Eliminates face data with lower confidence than the value set at this</para>
    /// <para>function for accuracy improvement of result stabilizing.</para>
    /// <para>For example, the previous data confidence with below 500 will not be</para>
    /// <para>applied for stabilizing when the face direction confidence threshold</para>
    /// <para>value has set as 500.</para>
    /// <para>* This is for the three functions of age, gender and face direction</para>
    /// <para>  estimation functions.</para>
    /// </summary>
    /// <param name="threshold">face direction confidence threshold value.[0-1000]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_pe_threshold_use(int threshold)
    {
        return this._stb.set_stb_pe_threshold_use(threshold);
    }

    /// <summary>
    /// Gets estimation result stabilizing threshold value.
    /// </summary>
    /// <returns>
    /// <para>Struct of (stb_return, threshold)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    threshold (int): face direction confidence threshold value</para></returns>
    public STB.STB_PE_THRESHOLD get_stb_pe_threshold_use()
    {
        return this._stb.get_stb_pe_threshold_use();
    }

    /// <summary>
    /// <para>Sets estimation result stabilizing angle</para>
    /// <para>Sets angle threshold value of Face direction.</para>
    /// <para>Eliminates face data with out of the set angle at this function for</para>
    /// <para>accuracy improvement of result stabilizing.</para>
    /// <para>For example, the previous data with up-down angle of below -16 degree</para>
    /// <para>and over 21 degree will not be applied for stabilizing when the up-down</para>
    /// <para>* This is for the three functions of age, gender and face direction</para>
    /// <para>  estimation functions.</para>
    /// <para>min_UD_angle ≦ max_UD_angle</para>
    /// <para>min_LR_angle ≦ max_LR_angle</para>
    /// </summary>
    /// <param name="min_UD_angle">minimum up-down angle of the face [-90 to 90]</param>
    /// <param name="max_UD_angle">maximum up-down angle of the face [-90 to 90]</param>
    /// <param name="min_LR_angle">minimum left-right angle of the face [-90 to 90]</param>
    /// <param name="max_LR_angle">maximum left-right angle of the face [-90 to 90]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_pe_angle_use(int min_UD_angle, int max_UD_angle,
                                    int min_LR_angle, int max_LR_angle)
    {
        return this._stb.set_stb_pe_angle_use(min_UD_angle, max_UD_angle, min_LR_angle, max_LR_angle);
    }

    /// <summary>
    /// Gets estimation result stabilizing angle
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, min_UD_angle, max_UD_angle,min_LR_angle, max_LR_angle)</para>
    /// <para>stb_return (int): return value of STB library</para>
    /// <para>min_UD_angle (int): minimum up-down angle of the face</para>
    /// <para>max_UD_angle (int): maximum up-down angle of the face</para>
    /// <para>min_LR_angle (int): minimum left-right angle of the face</para>
    /// <para>max_LR_angle (int): maximum left-right angle of the face</para>
    /// </returns>
    public STB.STB_PE_ANGLE get_stb_pe_angle_use()
    {
        return this._stb.get_stb_pe_angle_use();
    }

    /// <summary>
    /// <para>Sets age/gender estimation complete frame count</para>
    /// <para>Sets the number of previous frames applying to fix stabilization.</para>
    /// <para>The data used for stabilizing process (=averaging) is only the one</para>
    /// <para>fulfilled the set_stb_pe_threshold_use() and set_stb_pe_angle_use()</para>
    /// <para>condition.</para>
    /// <para>Stabilizing process will be completed with data more than the number of</para>
    /// <para>frames set at this function and it won't be done with less data.</para>
    /// <para>* This is for the two functions of age and gender estimation.</para>
    /// </summary>
    /// <param name="frame_count">the number of previous frames applying to fix the result [1-20]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_pe_complete_frame_count(int frame_count)
    {
        return this._stb.set_stb_pe_complete_frame_count(frame_count);

    }

    /// <summary>
    /// Gets age/gender estimation complete frame count.
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, frame_count)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    frame_count (int): the number of previous frames applying to fix the result</para>
    /// </returns>
    public STB.STB_FRAMERESULT get_stb_pe_complete_frame_count()
    {
        return this._stb.get_stb_pe_complete_frame_count();
    }

    /// <summary>
    /// <para>Sets recognition stabilizing threshold value</para>
    /// <para>Sets stabilizing threshold value of Face direction confidence to improve</para>
    /// <para>recognition stabilization.</para>
    /// <para>Eliminates face data with lower confidence than the value set at this</para>
    /// <para>function.</para>
    /// <para>For example, the previous data confidence with below 500 will not be</para>
    /// <para>applied for stabilizing when the face direction confidence threshold</para>
    /// <para>value has set as 500.</para>
    /// </summary>
    /// <param name="threshold">face direction confidence threshold value [0-1000]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_fr_threshold_use(int threshold)
    {
        return this._stb.set_stb_fr_threshold_use(threshold);
    }

    /// <summary>
    /// Gets recognition stabilizing threshold value
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, threshold)</para>
    /// <para>stb_return (int): return value of STB library</para>
    /// <para>threshold (int): face direction confidence threshold value</para>
    /// </returns>
    public STB.STB_FR_THRESHOLD get_stb_fr_threshold_use()
    {
        return this._stb.get_stb_fr_threshold_use();
    }

    /// <summary>
    /// <para>Sets recognition stabilizing angle</para>
    /// <para>Sets angle threshold value of Face direction for accuracy improvement of</para>
    /// <para>recognition stabilizing.</para>
    /// <para>Eliminates face data with out of the set angle at this function.</para>
    /// <para>For example, the previous data with up-down angle of below -16degree and</para>
    /// <para>over 21 degree will not be applied for stabilizing when the up-down</para>
    /// <para>angle threshold value of Face direction has set as 15 for minimum and/para>
    /// <para>21 for maximum.</para>
    /// <para>min_UD_angle ≦ max_UD_angle</para>
    /// <para>min_LR_angle ≦ max_LR_angle</para>
    /// </summary>
    /// <param name="min_UD_angle">minimum up-down angle of the face [-90 to 90]</param>
    /// <param name="max_UD_angle">maximum up-down angle of the face [-90 to 90]</param>
    /// <param name="min_LR_angle">minimum left-right angle of the face [-90 to 90]</param>
    /// <param name="max_LR_angle">maximum left-right angle of the face [-90 to 90]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_fr_angle_use(int min_UD_angle, int max_UD_angle,
                                    int min_LR_angle, int max_LR_angle)
    {
        return this._stb.set_stb_fr_angle_use(min_UD_angle, max_UD_angle,
                                              min_LR_angle, max_LR_angle);
    }

    /// <summary>
    /// Gets recognition stabilizing angle
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, min_UD_angle, max_UD_angle,min_LR_angle, max_LR_angle)</para>
    /// <para>stb_return (int): return value of STB library</para>
    /// <para>min_UD_angle (int): minimum up-down angle of the face</para>
    /// <para>max_UD_angle (int): maximum up-down angle of the face</para>
    /// <para>min_LR_angle (int): minimum left-right angle of the face</para>
    /// <para>max_LR_angle (int): maximum left-right angle of the face</para>
    /// </returns>
    public STB.STB_FR_ANGLE get_stb_fr_angle_use()
    {
        return this._stb.get_stb_fr_angle_use();
    }

    /// <summary>
    /// <para>Sets recognition stabilizing complete frame count</para>
    /// <para>Sets the number of previous frames applying to fix the recognition</para>
    /// <para>stabilizing.</para>
    /// <para>The data used for stabilizing process (=averaging) is only the one</para>
    /// <para>fulfilled the STB_SetFrThresholdUse and STB_SetFrAngleUse condition.</para>
    /// <para>Stabilizing process will be completed with a recognition ID fulfilled</para>
    /// <para>seizing ratio in result fixing frames and will not be done without one.</para>
    /// <para>* Refer set_stb_fr_min_ratio function for account ratio function</para>
    /// </summary>
    /// <param name="frame_count">the number of previous frames applying to fix the result. [0-20]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_fr_complete_frame_count(int frame_count)
    {
        return this._stb.set_stb_fr_complete_frame_count(frame_count);
    }

    /// <summary>
    /// Gets recognition stabilizing complete frame count
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, frame_count)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    frame_count (int): the number of previous frames applying to fix the result. [0-20]</para>
    /// </returns>
    public STB.STB_FR_COMPLETE_FRAME_COUNT get_stb_fr_complete_frame_count()
    {
        return this._stb.get_stb_fr_complete_frame_count();
    }

    /// <summary>
    /// <para>Sets recognition minimum account ratio</para>
    /// <para>Sets minimum account ratio in complete frame count for accuracy</para>
    /// <para>improvement of recognition stabilizing.</para>
    /// <para>For example, when there are 7 frames of extracted usable data in</para>
    /// <para>referred previous 20 frames, STB_SetFrCompleteFrameCount function has</para>
    /// <para>set "10"for the complete frame count and "60" for the recognition</para>
    /// <para>minimum account ratio.</para>
    /// <para>Creates frequency distribution of recognition result in the set 10 frames.</para>
    /// <para>    Recognized as "Mr. A"(1 frame)</para>
    /// <para>    Recognized as "Mr. B"(4 frames)</para>
    /// <para>    Recognized as "Mr. C"(4 frames)</para>
    /// <para>In this case, the most account ratio “Mr. B” will be output as</para>
    /// <para>stabilized result.</para>
    /// <para>However, this recognition status will be output as "STB_STAUS_CALCULATING"</para>
    /// <para>since the account ratio is about57%(= 4 frames/10 frames) ,</para>
    /// <para>(Mr. B seizing ratio=) 57% < recognition account ratio (=60%).</para>
    /// </summary>
    /// <param name="min_ratio">recognition minimum account ratio [0-100]</param>
    /// <returns>return value of STB library</returns>
    public int set_stb_fr_min_ratio(int min_ratio)
    {
        return this._stb.set_stb_fr_min_ratio(min_ratio);

    }

    /// <summary>
    /// Gets recognition minimum account ratio
    /// </summary>
    /// <returns>
    /// <para>struct of (stb_return, min_ratio)</para>
    /// <para>    stb_return (int): return value of STB library</para>
    /// <para>    min_ratio (int): recognition minimum account ratio</para>
    /// </returns>
    public STB.STB_FR_MIN_RATIO get_stb_fr_min_ratio()
    {
        return this._stb.get_stb_fr_min_ratio();
    }

}
