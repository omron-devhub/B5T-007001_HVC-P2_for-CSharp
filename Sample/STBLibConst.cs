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

namespace STBLibWrapper
{
    /// <summary>
    /// STB Common definitions
    /// </summary>
    public static class STBCommonDef
    {
        /// <summary>Executed flag</summary>
        /// <summary>[LSB]bit0: Body Tracking</summary>
        public const uint STB_FUNC_BD = 0x00000001U;
        /// <summary>[LSB]bit2: Face Tracking</summary>
        public const uint STB_FUNC_DT = 0x00000004U;
        /// <summary>[LSB]bit3: Face Direction</summary>
        public const uint STB_FUNC_PT = 0x00000008U;
        /// <summary>[LSB]bit4: Age Estimation</summary>
        public const uint STB_FUNC_AG = 0x00000010U;
        /// <summary>[LSB]bit5: Gender Estimation</summary>
        public const uint STB_FUNC_GN = 0x00000020U;
        /// <summary>[LSB]bit6: Gaze Estimation</summary>
        public const uint STB_FUNC_GZ = 0x00000040U;
        /// <summary>[LSB]bit7: Blink Estimation</summary>
        public const uint STB_FUNC_BL = 0x00000080U;
        /// <summary>[MSB]bit0: Expression Estimatio</summary>
        public const uint STB_FUNC_EX = 0x00000100U;
        /// <summary>[MSB]bit0: Expression Estimatio</summary>
        public const uint STB_FUNC_FR = 0x00000200U;

        /// <summary>STB library's error code</summary>
        /// <summary>Successful completion</summary>
        public const int STB_NORMAL = 0;
        /// <summary>Initialization error</summary>
        public const int STB_ERR_INITIALIZE = -2;
        /// <summary>Argument error</summary>
        public const int STB_ERR_INVALIDPARAM = -3;
        /// <summary>Handle error</summary>
        public const int STB_ERR_NOHANDLE = -7;
        /// <summary>When the processing condition is not satisfied</summary>
        public const int STB_ERR_PROCESSCONDITION = -8;

        public const int STB_TRUE = 1;
        public const int STB_FALSE = 0;
    } 
}
