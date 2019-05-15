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

    public enum STB_OKAO_EXPRESSION
    {
        STB_Expression_Neutral,
        STB_Expression_Happiness,
        STB_Expression_Surprise,
        STB_Expression_Anger,
        STB_Expression_Sadness,
        STB_Expression_Max
    }

    public enum STB_STATUS
    {
        STB_STATUS_NO_DATA     = -1,
        STB_STATUS_CALCULATING =  0,
        STB_STATUS_COMPLETE    =  1,
        STB_STATUS_FIXED       =  2
    }

    public enum STB_EXPRESSION
    {
        STB_EX_UNKNOWN   =  -1,
        STB_EX_NEUTRAL   =   0,
        STB_EX_HAPPINESS,
        STB_EX_SURPRISE,
        STB_EX_ANGER,
        STB_EX_SADNESS,
        STB_EX_MAX
    }

}
