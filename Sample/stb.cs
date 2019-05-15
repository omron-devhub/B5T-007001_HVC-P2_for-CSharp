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
using System.Runtime.InteropServices;

public class STB
{
    public const int STB_EX_BODY = 0x01;
    public const int STB_EX_FACE = 0x04;
    public const int STB_EX_DIRECTION = 0x08;
    public const int STB_EX_AGE = 0x10;
    public const int STB_EX_GENDER = 0x20;
    // public const int STB_EX_GAZE = 0x40;  // Not use now.
    // public const int STB_EX_BLINK = 0x80; // Not use now.
    // public const int STB_EX_EXPRESSION = 0x100; // Not use now.
    public const int STB_EX_RECOGNITION = 0x200;
    public const int STB_EX_FUNC_ALL = STB_EX_BODY | STB_EX_FACE | STB_EX_DIRECTION | STB_EX_AGE | STB_EX_GENDER | STB_EX_RECOGNITION;


    // STB error code definition
    public const int STB_RET_NORMAL = 0x00;  // Normal end
    public const int STB_RET_ERR_INITIALIZE = -0x02;  // Initializing error
    public const int STB_RET_ERR_INVALIDPARAM = -0x03;  // Parameter error
    public const int STB_RET_ERR_NOHANDLE = -0x07;  // Handle error
    public const int STB_RET_ERR_PROCESSCONDITION = -0x08; // Processing condition error

    public IntPtr hstb;

    public const int STB_FRAME_NUM = 35;

    public struct STB_EXECUTE_RET
    {
        public int retcode;
        public uint face_count;
        public uint body_count;
    }

    public struct STB_VERSION_RET
    {
        public int retcode;
        public sbyte major_version;
        public sbyte minor_version;
    }

    public struct STB_GETRETRYCOUNT_RET
    {
        public int retcode;
        public int max_retry_count;
    }

    public struct STB_TR_STEADINESS_PARAM_RET
    {
        public int retcode;
        public int pos_steadiness_param;
        public int size_steadiness_param;
    }


    public struct STB_TR_STEADINESS_PARAM
    {
        public int retcode;
        public int pos_steadiness_param_value;
        public int size_steadiness_param_value;
    }

    public struct STB_PE_THRESHOLD
    {
        public int retcode;
        public int threshold;
    }

    public struct STB_PE_ANGLE
    {
        public int retcode;
        public int min_UD_angle;
        public int max_UD_angle;
        public int min_LR_angle;
        public int max_LR_angle;
    }

    public struct STB_FRAMERESULT
    {
        public int retcode;
        public int frame_count;
    }

    public struct STB_FR_THRESHOLD
    {
        public int retcode;
        public int threshold;
    }

    public struct STB_FR_ANGLE
    {
        public int retcode;
        public int min_UD_angle;
        public int max_UD_angle;
        public int min_LR_angle;
        public int max_LR_angle;
    }
    public struct STB_FR_COMPLETE_FRAME_COUNT
    {
        public int retcode;
        public int frame_count;
    }

    public struct STB_FR_MIN_RATIO
    {
        public int retcode;
        public int min_ratio;
    }

    public STB(string library_name, int exec_func)
    {
        this.hstb = STBLibWrapper.STBLib.STB_CreateHandle((uint)exec_func);
    }

    /// <summary>
    /// <para>Executes stabilization process.</para>
    /// <para>In:</para>
    /// <para>   - each frame result by frame_res(C_FRAME_RESULT) input argument</para>
    /// <para>Out:</para>
    /// <para>   - face count (return)</para>
    /// <para>   - body count (return)</para>
    /// <para>   - stabilized face result by faces_res(C_FACE_RESULTS) output argument</para>
    /// <para>   - stabilized body result by bodies_res(C_BODY_RESULTS) output argument</para>
    /// </summary>
    /// <param name="frame_res">input one frame result for STBLib.
    /// Set the information of face central coordinate, size and
    /// direction to stabilize age, gender and face recognition.</param>
    /// <param name="faces_res">output result stabilized face data.</param>
    /// <param name="bodies_res">output result stabilized body data.</param>
    /// <returns>struct of (stb_return, face_count, body_count)
    /// stb_return (int): return value of STB library
    /// face_count (int): face count
    /// body_count (int): body count
    /// </returns>   
    public STB_EXECUTE_RET execute(STBLibWrapper.STB_FRAME_RESULT frameresult, STBLibWrapper.STB_FACE[] faces_res, STBLibWrapper.STB_BODY[] bodies_res)
    {

        STB_EXECUTE_RET ret;

        ret.face_count = 0;
        ret.body_count = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_SetFrameResult(this.hstb, ref frameresult);
        if (ret.retcode != STB_RET_NORMAL)
        {
            return ret;
        }

        ret.retcode = STBLibWrapper.STBLib.STB_Execute(this.hstb);
        if (ret.retcode != STB_RET_NORMAL)
        {
            return ret;
        }

        ret.retcode = STBLibWrapper.STBLib.STB_GetFaces(this.hstb, out ret.face_count, faces_res);

        if (ret.retcode != STB_RET_NORMAL)
        {
            ret.face_count = 0;
            ret.body_count = 0;
            return ret;
        }

        ret.retcode = STBLibWrapper.STBLib.STB_GetBodies(this.hstb, out ret.body_count, bodies_res);

        if (ret.retcode != STB_RET_NORMAL)
        {
            ret.face_count = 0;
            ret.body_count = 0;
            return ret;
        }

        return ret;

    }

    public STB_VERSION_RET get_stb_version()
    {
        STB_VERSION_RET ret;
        ret.retcode = 0;
        ret.major_version = 0;
        ret.minor_version = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetVersion(out ret.major_version, out ret.minor_version);

        return ret;
    }

    public int clear_stb_frame_results()
    {
        return STBLibWrapper.STBLib.STB_ClearFrameResults(this.hstb);
    }

    public int set_stb_tr_retry_count(int max_retry_count)
    {
        return STBLibWrapper.STBLib.STB_SetTrRetryCount(this.hstb, max_retry_count);
    }


    public STB_GETRETRYCOUNT_RET get_stb_tr_retry_count()
    {
        STB_GETRETRYCOUNT_RET ret;

        ret.retcode = 0;
        ret.max_retry_count = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetTrRetryCount(this.hstb, out ret.max_retry_count);

        return ret;
    }

    public int set_stb_tr_steadiness_param(int pos_steadiness_param, int size_steadiness_param)
    {
        return STBLibWrapper.STBLib.STB_SetTrSteadinessParam(this.hstb,
                                                            pos_steadiness_param,
                                                            size_steadiness_param);
    }

    public STB_TR_STEADINESS_PARAM_RET get_stb_tr_steadiness_param()
    {
        STB_TR_STEADINESS_PARAM_RET ret;

        ret.retcode = 0;
        ret.pos_steadiness_param = 0;
        ret.size_steadiness_param = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetTrSteadinessParam(this.hstb,
                                                                    out ret.pos_steadiness_param,
                                                                    out ret.size_steadiness_param);

        return ret;
    }

    public STB_TR_STEADINESS_PARAM stb_tr_steadiness_param()
    {
        STB_TR_STEADINESS_PARAM ret;

        ret.retcode = 0;
        ret.pos_steadiness_param_value = 0;
        ret.size_steadiness_param_value = 0;


        ret.retcode = STBLibWrapper.STBLib.STB_GetTrSteadinessParam(this.hstb,
                                                                    out ret.pos_steadiness_param_value,
                                                                    out ret.size_steadiness_param_value);

        return ret;
    }

    public int set_stb_pe_threshold_use(int threshold)
    {
        return STBLibWrapper.STBLib.STB_SetPeThresholdUse(this.hstb, threshold);
    }


    public STB_PE_THRESHOLD get_stb_pe_threshold_use()
    {
        STB_PE_THRESHOLD ret;
        ret.retcode = 0;
        ret.threshold = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetPeThresholdUse(this.hstb, out ret.threshold);

        return ret;
    }

    public int set_stb_pe_angle_use(int min_UD_angle, int max_UD_angle,
                                    int min_LR_angle, int max_LR_angle)
    {
        return STBLibWrapper.STBLib.STB_SetPeAngleUse(this.hstb,
                                             min_UD_angle, max_UD_angle,
                                             min_LR_angle, max_LR_angle);
    }

    public STB_PE_ANGLE get_stb_pe_angle_use()
    {
        STB_PE_ANGLE ret;

        ret.retcode = 0;
        ret.min_UD_angle = 0;
        ret.max_UD_angle = 0;
        ret.min_LR_angle = 0;
        ret.max_LR_angle = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetPeAngleUse(this.hstb,
                                                             out ret.min_UD_angle,
                                                             out ret.max_UD_angle,
                                                             out ret.min_LR_angle,
                                                             out ret.max_LR_angle);

        return ret;
    }

    public int set_stb_pe_complete_frame_count(int frame_count)
    {
        return STBLibWrapper.STBLib.STB_SetPeCompleteFrameCount(this.hstb, frame_count);
    }

    public STB_FRAMERESULT get_stb_pe_complete_frame_count()
    {
        STB_FRAMERESULT ret;

        ret.retcode = 0;
        ret.frame_count = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetPeCompleteFrameCount(this.hstb, out ret.frame_count);

        return ret;
    }

    public int set_stb_fr_threshold_use(int threshold)
    {
        return STBLibWrapper.STBLib.STB_SetFrThresholdUse(this.hstb, threshold);
    }

    public STB_FR_THRESHOLD get_stb_fr_threshold_use()
    {
        STB_FR_THRESHOLD ret;

        ret.retcode = 0;
        ret.threshold = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetFrThresholdUse(this.hstb, out ret.threshold);

        return ret;
    }

    public int set_stb_fr_angle_use(int min_UD_angle, int max_UD_angle, int min_LR_angle,
                                                                int max_LR_angle)
    {
        return STBLibWrapper.STBLib.STB_SetFrAngleUse(this.hstb,
                                            min_UD_angle, max_UD_angle,
                                            min_LR_angle, max_LR_angle);
    }

    public STB_FR_ANGLE get_stb_fr_angle_use()
    {
        STB_FR_ANGLE ret;
        ret.retcode = 0;
        ret.min_UD_angle = 0;
        ret.max_UD_angle = 0;
        ret.min_LR_angle = 0;
        ret.max_LR_angle = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetFrAngleUse(this.hstb,
                                                             out ret.min_UD_angle,
                                                             out ret.max_UD_angle,
                                                             out ret.min_LR_angle,
                                                             out ret.max_LR_angle);
        return ret;
    }

    public int set_stb_fr_complete_frame_count(int frame_count)
    {
        return STBLibWrapper.STBLib.STB_SetFrCompleteFrameCount(this.hstb, frame_count);
    }

    public STB_FR_COMPLETE_FRAME_COUNT get_stb_fr_complete_frame_count()
    {
        STB_FR_COMPLETE_FRAME_COUNT ret;
        ret.retcode = 0;
        ret.frame_count = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetFrCompleteFrameCount(this.hstb, out ret.frame_count);

        return ret;
    }

    public int set_stb_fr_min_ratio(int min_ratio)
    {
        return STBLibWrapper.STBLib.STB_SetFrMinRatio(this.hstb, min_ratio);
    }

    public STB_FR_MIN_RATIO get_stb_fr_min_ratio()
    {
        STB_FR_MIN_RATIO ret;
        ret.retcode = 0;
        ret.min_ratio = 0;

        ret.retcode = STBLibWrapper.STBLib.STB_GetFrMinRatio(this.hstb, out ret.min_ratio);
        return ret;
    }

    public void stb_delete()
    {
        if (this.hstb != null)
            STBLibWrapper.STBLib.STB_DeleteHandle(this.hstb);
    }
}

