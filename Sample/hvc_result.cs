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

public class HVCResult
{
    public List<FaceResult> faces;
    public List<DetectionResult> bodies;
    public List<DetectionResult> hands;

    public HVCResult()
    {
        this.faces = new List<FaceResult>();
        this.bodies = new List<DetectionResult>();
        this.hands = new List<DetectionResult>();
    }

    public int read_from_buffer(int exec_func, int data_len, byte[] data)
    {
        var cur = 0;

        var body_count = data[cur];
        var hand_count = data[cur + 1];
        var face_count = data[cur + 2];

        cur += 4; // for reserved

        // Human body detection
        for (var i = 0; i < body_count; i++)
        {
            var x = BitConverter.ToInt16(data, cur);
            var y = BitConverter.ToInt16(data, cur + 2);
            var size = BitConverter.ToInt16(data, cur + 4);
            var conf = BitConverter.ToInt16(data, cur + 6);

            var res = new DetectionResult(x, y, size, conf);

            this.bodies.Add(res);

            cur += 8;
        }

        // Hand detection
        for (var i = 0; i < hand_count; i++)
        {
            var x = BitConverter.ToInt16(data, cur);
            var y = BitConverter.ToInt16(data, cur + 2);
            var size = BitConverter.ToInt16(data, cur + 4);
            var conf = BitConverter.ToInt16(data, cur + 6);

            var res = new DetectionResult(x, y, size, conf);

            this.hands.Add(res);
            cur += 8;
        }

        // Face detection
        for (var i = 0; i < face_count; i++)
        {
            var x = BitConverter.ToInt16(data, cur);
            var y = BitConverter.ToInt16(data, cur + 2);
            var size = BitConverter.ToInt16(data, cur + 4);
            var conf = BitConverter.ToInt16(data, cur + 6);

            var res = new FaceResult(x, y, size, conf);

            cur += 8;

            // Face direction
            if ((exec_func & p2def.EX_DIRECTION) == p2def.EX_DIRECTION)
            {
                var LR = BitConverter.ToInt16(data, cur);
                var UD = BitConverter.ToInt16(data, cur + 2);
                var roll = BitConverter.ToInt16(data, cur + 4);
                var direction_conf = BitConverter.ToInt16(data, cur + 6);

                res.direction = new DirectionResult(LR, UD, roll, direction_conf);
                cur += 8;
            }

            // Age estimation
            if ((exec_func & p2def.EX_AGE) == p2def.EX_AGE)
            {
                var age = data[cur];
                var age_conf = BitConverter.ToInt16(data, cur + 1);
                res.age = new AgeResult(age, age_conf);
                cur += 3;
            }

            // Gender estimation
            if ((exec_func & p2def.EX_GENDER) == p2def.EX_GENDER)
            {
                var gen = data[cur];
                var gen_conf = BitConverter.ToInt16(data, cur + 1);
                res.gender = new GenderResult(gen, gen_conf);
                cur += 3;
            }

            // Gaze estimation
            if ((exec_func & p2def.EX_GAZE) == p2def.EX_GAZE)
            {
                int Gaze_LR = data[cur];
                int Gaze_UD = data[cur + 1];

                if (Gaze_LR > 127)
                {
                    Gaze_LR -= 256;
                }

                if (Gaze_UD > 127)
                {
                    Gaze_UD -= 256;
                }

                res.gaze = new GazeResult(Gaze_LR, Gaze_UD);
                cur += 2;
            }

            // Blink estimation
            if ((exec_func & p2def.EX_BLINK) == p2def.EX_BLINK)
            {
                var L = BitConverter.ToInt16(data, cur);
                var R = BitConverter.ToInt16(data, cur + 2);
                res.blink = new BlinkResult(L, R);
                cur += 4;
            }

            // Expression estimation
            if ((exec_func & p2def.EX_EXPRESSION) == p2def.EX_EXPRESSION)
            {
                var neu = data[cur];
                var hap = data[cur + 1];
                var sur = data[cur + 2];
                var ang = data[cur + 3];
                var sad = data[cur + 4];
                int neg = data[cur + 5];

                if (neg > 127)
                {
                    neg -= 256;
                }
                res.expression = new ExpressionResult(neu, hap, sur, ang, sad, neg);
                cur += 6;
            }

            // Face recognition
            if ((exec_func & p2def.EX_RECOGNITION) == p2def.EX_RECOGNITION)
            {
                var uid = BitConverter.ToInt16(data, cur);
                var score = BitConverter.ToInt16(data, cur + 2);
                res.recognition = new RecognitionResult(uid, score);
                cur += 4;
            }

            this.faces.Add(res);
        }

        return cur;
    }

    public void export_to_C_FRAME_RESULT(ref STBLibWrapper.STB_FRAME_RESULT frame_result)
    {
        // Human body detection result
        frame_result.bodys.nCount = this.bodies.Count();

        for (var i = 0; i < this.bodies.Count(); i++)
        {
            frame_result.bodys.body[i].center.nX = this.bodies[i].pos_x;
            frame_result.bodys.body[i].center.nY = this.bodies[i].pos_y;
            frame_result.bodys.body[i].nSize = this.bodies[i].size;
            frame_result.bodys.body[i].nConfidence = this.bodies[i].conf;
        }

        // Face detection result
        frame_result.faces.nCount = this.faces.Count();
        for (var i = 0; i < this.faces.Count(); i++)
        {
            frame_result.faces.face[i].center.nX = this.faces[i].pos_x;
            frame_result.faces.face[i].center.nY = this.faces[i].pos_y;
            frame_result.faces.face[i].nSize = this.faces[i].size;
            frame_result.faces.face[i].nConfidence = this.faces[i].conf;

            // Face direction result
            if (this.faces[i].direction != null)
            {
                frame_result.faces.face[i].direction.nLR = this.faces[i].direction.LR;
                frame_result.faces.face[i].direction.nUD = this.faces[i].direction.UD;
                frame_result.faces.face[i].direction.nRoll = this.faces[i].direction.roll;
                frame_result.faces.face[i].direction.nConfidence = this.faces[i].direction.conf;
            }
            // Age estimation result
            if (this.faces[i].age != null)
            {
                frame_result.faces.face[i].age.nAge = this.faces[i].age.age;
                frame_result.faces.face[i].age.nConfidence = this.faces[i].age.conf;
            }
            // Gender estimation result
            if (this.faces[i].gender != null)
            {
                frame_result.faces.face[i].gender.nGender = this.faces[i].gender.gender;
                frame_result.faces.face[i].gender.nConfidence = this.faces[i].gender.conf;
            }
            // Gaze estimation result
            if (this.faces[i].gaze != null)
            {
                frame_result.faces.face[i].gaze.nLR = this.faces[i].gaze.gazeLR;
                frame_result.faces.face[i].gaze.nUD = this.faces[i].gaze.gazeUD;
            }
            // Blink estimation result
            if (this.faces[i].blink != null)
            {
                frame_result.faces.face[i].blink.nLeftEye = this.faces[i].blink.ratioL;
                frame_result.faces.face[i].blink.nRightEye = this.faces[i].blink.ratioR;
            }
            // Expression estimation result
            if (this.faces[i].expression != null)
            {
                frame_result.faces.face[i].expression.anScore = new int[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Max];

                frame_result.faces.face[i].expression.anScore[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Neutral] = this.faces[i].expression.neutral;
                frame_result.faces.face[i].expression.anScore[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Happiness] = this.faces[i].expression.happiness;
                frame_result.faces.face[i].expression.anScore[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Surprise] = this.faces[i].expression.surprise;
                frame_result.faces.face[i].expression.anScore[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Anger] = this.faces[i].expression.anger;
                frame_result.faces.face[i].expression.anScore[(int)STBLibWrapper.STB_OKAO_EXPRESSION.STB_Expression_Sadness] = this.faces[i].expression.sadness;
            }
            // Recognition result
            if (this.faces[i].recognition != null)
            {
                frame_result.faces.face[i].recognition.uID = this.faces[i].recognition.uid;
                frame_result.faces.face[i].recognition.nScore = this.faces[i].recognition.score;
            }
        }
    }

    public override string ToString()
    {
        var i = 0;
        var sb = new StringBuilder("  Face count= ");

        sb.AppendLine(this.faces.Count().ToString());

        for (i = 0; i < this.faces.Count(); i++)
            sb.Append("    [").Append(i.ToString()).Append("]  ").Append(this.faces[i].ToString());

        sb.Append("Body count= ").AppendLine(this.bodies.Count().ToString());
        for (i = 0; i < this.bodies.Count(); i++)
            sb.Append("  [").Append(i.ToString()).Append("]  ").Append(this.bodies[i].ToString());

        sb.Append("Hand count= ").AppendLine(this.hands.Count().ToString());
        for (i = 0; i < this.hands.Count(); i++)
            sb.Append("  [").Append(i.ToString()).Append("]  ").Append(this.hands[i].ToString());

        return sb.ToString();
    }
}
