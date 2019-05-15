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

namespace STBLibWrapper
{
    public static class STBLib
    {
        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#1")]
        public static extern int STB_GetVersion(out sbyte major, out sbyte minor);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#2")]
        public static extern IntPtr STB_CreateHandle(uint usefuncflag);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#3")]
        public static extern int STB_DeleteHandle(IntPtr hstb);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#10")]
        public static extern int STB_ClearFrameResults(IntPtr hstb);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#101")]
        public static extern int STB_SetFrameResult(IntPtr hstb, ref STB_FRAME_RESULT frameresult);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#111")]
        public static extern int STB_Execute(IntPtr hstb);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#112")]
        public static extern int STB_GetFaces(IntPtr hstb, out uint facecount, [In,Out]STB_FACE[] face);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#113")]
        public static extern int STB_GetBodies(IntPtr hstb, out uint facecount, [In, Out]STB_BODY[] body);


        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#301")]
        public static extern int STB_SetTrRetryCount(IntPtr hstb, int maxretrycount);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#302")]
        public static extern int STB_GetTrRetryCount(IntPtr hstb, out int maxretrycount);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#303")]
        public static extern int STB_SetTrSteadinessParam(IntPtr hstb, int possteadinessparam, int sizesteadinessparam);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#304")]
        public static extern int STB_GetTrSteadinessParam(IntPtr hstb, out int possteadinessparam, out int sizesteadinessparam);



        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#401")]
        public static extern int STB_SetPeThresholdUse(IntPtr hstb, int threshold);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#402")]
        public static extern int STB_GetPeThresholdUse(IntPtr hstb, out int threshold);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#403")]
        public static extern int STB_SetPeAngleUse(IntPtr hstb, int minudangle, int maxudangle, int minlrangle, int maxlrangle );

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#404")]
        public static extern int STB_GetPeAngleUse(IntPtr hstb, out int minudangle, out int maxudangle, out int minlrangle, out int maxlrangle);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#405")]
        public static extern int STB_SetPeCompleteFrameCount(IntPtr hstb, int framecount);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#406")]
        public static extern int STB_GetPeCompleteFrameCount(IntPtr hstb, out int framecount);



        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#501")]
        public static extern int STB_SetFrThresholdUse(IntPtr hstb, int threshold);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#502")]
        public static extern int STB_GetFrThresholdUse(IntPtr hstb, out int threshold);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#503")]
        public static extern int STB_SetFrAngleUse(IntPtr hstb, int minudangle, int maxudangle, int minlrangle, int maxlrangle);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#504")]
        public static extern int STB_GetFrAngleUse(IntPtr hstb, out int minudangle, out int maxudangle, out int minlrangle, out int maxlrangle);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#505")]
        public static extern int STB_SetFrCompleteFrameCount(IntPtr hstb, int framecount);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#506")]
        public static extern int STB_GetFrCompleteFrameCount(IntPtr hstb, out int framecount);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#507")]
        public static extern int STB_SetFrMinRatio(IntPtr hstb, int minratio);

        [DllImport("libSTB.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "#508")]
        public static extern int STB_GetFrMinRatio(IntPtr hstb, out int minratio);

    }
}
