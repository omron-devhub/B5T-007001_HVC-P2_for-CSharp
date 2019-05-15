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
using System.Runtime.InteropServices;

namespace STBLibWrapper
{

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_POINT
    {
        public int nX;
        public int nY;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_DIRECTION
    {
        public int nLR;
        public int nUD;
        public int nRoll;
        public int nConfidence;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_AGE
    {
        public int nAge;
        public int nConfidence;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_GENDER
    {
        public int nGender;
        public int nConfidence;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_GAZE
    {
        public int nLR;
        public int nUD;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_BLINK
    {
        public int nLeftEye;
        public int nRightEye;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_EXPRESSION
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)STB_OKAO_EXPRESSION.STB_Expression_Max)]
        public int[] anScore;
        public int nDegree;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_RECOGNITION
    {
        public int uID;
        public int nScore;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_DETECTION
    {
        public STB_POINT center;
        public int       nSize;
        public int       nConfidence;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_FACE
    {
        public STB_POINT center;
        public int nSize;
        public int nConfidence;
        public STB_FRAME_RESULT_DIRECTION direction;
        public STB_FRAME_RESULT_AGE age;
        public STB_FRAME_RESULT_GENDER gender;
        public STB_FRAME_RESULT_GAZE gaze;
        public STB_FRAME_RESULT_BLINK blink;
        public STB_FRAME_RESULT_EXPRESSION expression;
        public STB_FRAME_RESULT_RECOGNITION recognition;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_BODYS
    {
        public int nCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
        public STB_FRAME_RESULT_DETECTION[] body;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT_FACES
    {
        public int nCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 35)]
        public STB_FRAME_RESULT_FACE[] face;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FRAME_RESULT
    {
        public STB_FRAME_RESULT_BODYS bodys;
        public STB_FRAME_RESULT_FACES faces;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct STB_RES
    {
        public STB_STATUS status;
        public int        conf;
        public int        value;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_GAZE
    {
        public STB_STATUS status;
        public int conf;
        public int UD;
        public int LR;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_DIR
    {
        public STB_STATUS status;
        public int conf;
        public int yaw;
        public int pitch;
        public int roll;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_BLINK
    {
        public STB_STATUS status;
        public int ratioL;
        public int ratioR;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_POS
    {
        public uint x;
        public uint y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_FACE
    {
        public int  nDetectID;
        public int  nTrackingID;
        public STB_POS center;
        public uint nSize;
        public int  conf;
        public STB_DIR direction;
        public STB_RES age;
        public STB_RES gender;
        public STB_GAZE gaze;
        public STB_BLINK blink;
        public STB_RES expression;
        public STB_RES recognition;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STB_BODY
    {
        public int     nDetectID;
        public int     nTrackingID;
        public STB_POS center;
        public uint    nSize;
        public int     conf;
    }
}
