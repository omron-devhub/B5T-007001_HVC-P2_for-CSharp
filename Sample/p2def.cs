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

public class p2def
{
    // Exceute function flag definition
    public const int EX_NONE = 0x00;
    public const int EX_BODY = 0x01;
    public const int EX_HAND = 0x02;
    public const int EX_FACE = 0x04;
    public const int EX_DIRECTION = 0x08;
    public const int EX_AGE = 0x10;
    public const int EX_GENDER = 0x20;
    public const int EX_GAZE = 0x40;
    public const int EX_BLINK = 0x80;
    public const int EX_EXPRESSION = 0x100;
    public const int EX_RECOGNITION = 0x200;
    public const int EX_ALL = EX_BODY + EX_HAND + EX_FACE + EX_DIRECTION + EX_AGE + EX_GENDER
        + EX_GAZE + EX_BLINK + EX_EXPRESSION + EX_RECOGNITION;

    // STB library ON/OFF flag
    public const bool USE_STB_ON = true;
    public const bool USE_STB_OFF = false;

    // output image type definition
    public const int OUT_IMG_TYPE_NONE = 0x00;
    public const int OUT_IMG_TYPE_QVGA = 0x01;
    public const int OUT_IMG_TYPE_QQVGA = 0x02;

    // HVC camera angle definition.
    public const int HVC_CAM_ANGLE_0 = 0x00;
    public const int HVC_CAM_ANGLE_90 = 0x01;
    public const int HVC_CAM_ANGLE_180 = 0x02;
    public const int HVC_CAM_ANGLE_270 = 0x03;

    // Face angel definitions.
    public const int HVC_FACE_ANGLE_YAW_30 = 0x00;  // Yaw angle:-30 to +30 degree (Frontal face)
    public const int HVC_FACE_ANGLE_YAW_60 = 0x01;  // Yaw angle:-60 to +60 degree (Half-Profile face)
    public const int HVC_FACE_ANGLE_YAW_90 = 0x02;  // Yaw angle:-90 to +90 degree (Profile face)
    public const int HVC_FACE_ANGLE_ROLL_15 = 0x00;  // Roll angle:-15 to +15 degree
    public const int HVC_FACE_ANGLE_ROLL_45 = 0x01;  // Roll angle:-45 to +45 degree

    // Available serial baudrate sets
    public int[] AVAILABLE_BAUD = { 9600, 38400, 115200, 230400, 460800, 921600 };

    public static int DEFAULT_BAUD = 9600;

    // Response code
    public const int RESPONSE_CODE_PLURAL_FACE = 0x02; // Number of faces that can be registerd is 0
    public const int RESPONSE_CODE_NO_FACE = 0x01; // Number of detected faces is 2 or more
    public const int RESPONSE_CODE_NORMAL = 0x00; // Normal end
    public const int RESPONSE_CODE_UNDEFINED = 0xFF; // Undefined error
    public const int RESPONSE_CODE_INTERNAL = 0xFE; // Intenal error
    public const int RESPONSE_CODE_INVALID_CMD = 0xFD; // Improper command

    // Estimation common result status
    public const int EST_NOT_POSSIBLE = -128;

    // Expression result
    public const int EXP_UNKNOWN = -1;
    public const int EXP_NEUTRAL = 0;
    public const int EXP_HAPPINESS = 1;
    public const int EXP_SURPRISE = 2;
    public const int EXP_ANGER = 3;
    public const int EXP_SADNESS = 4;

    // Gender result
    public const int GENDER_UNKNOWN = -1;
    public const int GENDER_FEMALE = 0;
    public const int GENDER_MALE = 1;

    // Recognition result
    public const int RECOG_NOT_POSSIBLE = -128;
    public const int RECOG_NO_DATA_IN_ALBUM = -127;
}
