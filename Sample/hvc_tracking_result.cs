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

public class hvc_tracking_result
{
    public const int STB_STATUS_NO_DATA = -1;
    public const int STB_STATUS_CALCULATING = 0;
    public const int STB_STATUS_COMPLETE = 1;
    public const int STB_STATUS_FIXED = 2;
    public const int STB_TRID_NOT_TRACKED = -1;

    public const int EX_AGE = 0x10;
    public const int EX_GENDER = 0x20;
    public const int EX_RECOGNITION = 0x200;

    public static Dictionary<int, string> status_dic = new Dictionary<int, string>()
    {
                { STB_STATUS_CALCULATING, "CALCULATING" },
                { STB_STATUS_COMPLETE, "COMPLETE" },
                { STB_STATUS_FIXED, "FIXED" },
                { STB_STATUS_NO_DATA, "NO_DATA" },
    };
}

public class TrackingAgeResult : AgeResult
{
    public int tracking_status;

    public TrackingAgeResult(int age, int conf, int tracking_status = hvc_tracking_result.STB_STATUS_NO_DATA)
        : base(age, conf)
    {
        this.tracking_status = tracking_status;
    }

    public override string ToString()
    {
        string str;

        if (this.tracking_status == hvc_tracking_result.STB_STATUS_NO_DATA)
        {
            str = "Age           Age:-    Conf:-";
        }
        else
        {
            str = base.ToString();
        }

        return str + " Status:" + hvc_tracking_result.status_dic[this.tracking_status].ToString();
    }
}

public class TrackingGenderResult : GenderResult
{
    public int tracking_status;

    public TrackingGenderResult(int gender = 0, int conf = 0, int tracking_status = hvc_tracking_result.STB_STATUS_NO_DATA)
        : base(gender, conf)
    {
        this.tracking_status = tracking_status;
    }

    public override string ToString()
    {
        string str;

        if (this.tracking_status == hvc_tracking_result.STB_STATUS_NO_DATA)
        {
            str = "Gender        Gender:- Conf:-";
        }
        else
        {
            str = base.ToString();
        }

        return str + " Status:" + hvc_tracking_result.status_dic[this.tracking_status].ToString();
    }
}

public class TrackingRecognitionResult : RecognitionResult
{
    public int tracking_status;

    public TrackingRecognitionResult(int uid = 0, int score = 0, int tracking_status = hvc_tracking_result.STB_STATUS_NO_DATA)
        : base(uid, score)
    {
        this.tracking_status = tracking_status;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("");

        if (this.uid == p2def.RECOG_NO_DATA_IN_ALBUM)
        {
            sb.Append("Recognition   No data is registered in the album.");
        }
        else if (this.uid == p2def.RECOG_NOT_POSSIBLE) // Recognition was not possible.
        {
            sb.Append("Recognition   Uid:- Score:").Append(this.score.ToString());
            sb.Append(" Status:").Append(hvc_tracking_result.status_dic[this.tracking_status].ToString());
        }
        else if (this.tracking_status == hvc_tracking_result.STB_STATUS_NO_DATA)
        {
            sb.Append("Recognition   Uid:- Score:- Status:").Append(hvc_tracking_result.status_dic[this.tracking_status].ToString());
        }
        else if (this.uid == -1) // Unknown user.
        {
            sb.Append("Recognition   Uid:Unknown Score:").Append(this.score.ToString());
            sb.Append(" Status:").Append(hvc_tracking_result.status_dic[this.tracking_status].ToString());
        }
        else
        {
            sb.Append("Recognition   Uid:").Append(this.uid.ToString());
            sb.Append(" Score:").Append(this.score.ToString());
            sb.Append(" Status:").Append(hvc_tracking_result.status_dic[this.tracking_status].ToString());
        }

        return sb.ToString();
    }
}

public class FaceList : List<TrackingFaceResult>
{
    public void append_C_FACE_RES35(int exec_func, int face_count, STBLibWrapper.STB_FACE[] face_res35)
    {
        // Appends the result of STB output to this face list.
        for (var i = 0; i < face_count; i++)
        {
            var tr_f = new TrackingFaceResult((int)face_res35[i].center.x,
                                      (int)face_res35[i].center.y,
                                      (int)face_res35[i].nSize,
                                      face_res35[i].conf,
                                      face_res35[i].nDetectID,
                                      face_res35[i].nTrackingID);

            if ((exec_func & hvc_tracking_result.EX_AGE) == hvc_tracking_result.EX_AGE)
            {
                tr_f.age = new TrackingAgeResult( face_res35[i].age.value,
                                                  face_res35[i].age.conf,
                                                  (int)face_res35[i].age.status);
            }

            if ((exec_func & hvc_tracking_result.EX_GENDER) == hvc_tracking_result.EX_GENDER)
            {
                tr_f.gender = new TrackingGenderResult(face_res35[i].gender.value,
                                                       face_res35[i].gender.conf,
                                                       (int)face_res35[i].gender.status);
            }

            if ((exec_func & hvc_tracking_result.EX_RECOGNITION) == hvc_tracking_result.EX_RECOGNITION)
            {
                tr_f.recognition = new TrackingRecognitionResult(face_res35[i].recognition.value,
                                                                 face_res35[i].recognition.conf,
                                                                 (int)face_res35[i].recognition.status);
            }
            // We do not use the functions(Face direction, Gaze, Blink and
            // Expression estimation) for STBLib.
            // So the part of that functions is not implemented here.

            this.Add(tr_f);
        }
    }

    public void append_direction_list(List<FaceResult> faces)
    {
        for (var i = 0; i < faces.Count; i++)
        {
            this[i].direction = faces[i].direction;
        }
    }

    public void append_gaze_list(List<FaceResult> faces)
    {
        for (var i = 0; i < faces.Count; i++)
        {
            this[i].gaze = faces[i].gaze;
        }
    }

    public void append_blink_list(List<FaceResult> faces)
    {
        for (var i = 0; i < faces.Count; i++)
        {
            this[i].blink = faces[i].blink;
        }
    }

    public void append_expression_list(List<FaceResult> faces)
    {
        for (var i = 0; i < faces.Count; i++)
        {
            this[i].expression = faces[i].expression;
        }
    }
}

public class BodyList : List<TrackingResult>
{

    public void append_BODY_RES35(int exec_func, int body_count, STBLibWrapper.STB_BODY[] body_res35)
    {
        for (var i = 0; i < body_count; i++)
        {
            var tr_b = new TrackingResult((int)body_res35[i].center.x,
                                                     (int)body_res35[i].center.y,
                                                     (int)body_res35[i].nSize,
                                                     (int)body_res35[i].conf,
                                                     body_res35[i].nDetectID,
                                                     body_res35[i].nTrackingID);
            this.Add(tr_b);
        }
    }
}

public class HandList : List<TrackingResult>
{
    public void append_hand_list(List<DetectionResult> hands)
    {
        //  Hand detection result
        for (var i = 0; i < hands.Count; i++)
        {
            var hand_res = new TrackingResult(hands[i].pos_x, hands[i].pos_y, hands[i].size, hands[i].conf, i, -1);
            this.Add(hand_res);
        }
    }
}

public class TrackingResult
{
    public int pos_x;
    public int pos_y;
    public int size;
    public int conf;
    public int detection_id;
    public int tracking_id;

    public TrackingResult(int pos_x, int pos_y, int size, int conf, int detection_id, int tracking_id)
    {
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        this.size = size;
        this.conf = conf;
        this.detection_id = detection_id;
        this.tracking_id = tracking_id;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("");

        if (this.tracking_id != -1)
        {
            sb.Append("  TrackingID:").AppendLine(this.tracking_id.ToString());
        }
        else
        {
            sb.AppendLine();
        }

        sb.Append("      Detection     ");
        sb.Append("X:").Append(this.pos_x.ToString());
        sb.Append(" Y:").Append(this.pos_y.ToString());
        sb.Append(" Size:").Append(this.size.ToString());
        sb.Append(" Conf:").Append(this.conf.ToString());

        return sb.ToString();
    }
}

public class TrackingFaceResult : TrackingResult
{
    public TrackingResult trackingresult;
    public DirectionResult direction;
    public AgeResult age;
    public GenderResult gender;
    public GazeResult gaze;
    public BlinkResult blink;
    public ExpressionResult expression;
    public RecognitionResult recognition;

    public TrackingFaceResult(int pos_x = 0, int pos_y = 0, int size = 0, int conf = 0, int detection_id = 0, int tracking_id = 0)
        : base(pos_x, pos_y, size, conf, detection_id, tracking_id)
    {
        this.trackingresult = new TrackingResult(pos_x, pos_y, size, conf, detection_id, tracking_id);
        this.direction = null;
        this.age = null;
        this.gender = null;
        this.gaze = null;
        this.blink = null;
        this.expression = null;
        this.recognition = null;
    }

    public new string ToString()
    {
        var sb = new StringBuilder("");

        sb.Append(base.ToString()).AppendLine();
        if (this.direction != null)
            sb.Append("      ").AppendLine(this.direction.ToString());
        if (this.age != null)
           sb.Append("      ").AppendLine(this.age.ToString());
        if (this.gender != null)
            sb.Append("      ").AppendLine(this.gender.ToString());
        if (this.gaze != null)
            sb.Append("      ").AppendLine(this.gaze.ToString());
        if (this.blink != null)
            sb.Append("      ").AppendLine(this.blink.ToString());
        if (this.expression != null)
           sb.Append("      ").AppendLine(this.expression.ToString());
        if (this.recognition != null)
            sb.Append("      ").AppendLine(this.recognition.ToString());
        return sb.ToString();
    }
}

public class HVCTrackingResult
{
    // Class storing tracking result
    public FaceList faces;
    public BodyList bodies;
    public HandList hands;

    public HVCTrackingResult()
    {
        this.faces = new FaceList();
        this.bodies = new BodyList();
        this.hands = new HandList();
    }

    public override string ToString()
    {
        var sb = new StringBuilder("  Face Count = ");
        sb.AppendLine(this.faces.Count().ToString());

        for (var i = 0; i < this.faces.Count(); i++)
            sb.Append("  [").Append(i.ToString()).Append("]").Append(this.faces[i].ToString());

        sb.Append("  Body Count = ").AppendLine(this.bodies.Count().ToString());
        for (var i = 0; i < this.bodies.Count; i++)
            sb.Append("  [").Append(i.ToString()).Append("]").AppendLine(this.bodies[i].ToString());

        sb.Append("  Hand Count = ").AppendLine(this.hands.Count().ToString());
        for (var i = 0; i < this.hands.Count(); i++)
            sb.Append("  [").Append(i.ToString()).Append("]").AppendLine(this.hands[i].ToString());

        return sb.ToString();
    }

    public void clear()
    {
        this.faces.Clear();
        this.bodies.Clear();
        this.hands.Clear();
    }

    public void appned_FRAME_RESULT(HVCResult frame_result)
    {
        // Body detection result
        for (var i = 0; i < frame_result.bodies.Count(); i++)
        {
            var body_res = new TrackingResult(frame_result.bodies[i].pos_x,
                frame_result.bodies[i].pos_y,
                frame_result.bodies[i].size,
                frame_result.bodies[i].conf,
                i,
                hvc_tracking_result.STB_TRID_NOT_TRACKED
                );
            this.bodies.Add(body_res);
        }

        // Hand detection result
        for (var i = 0; i < frame_result.hands.Count(); i++)
        {
            var hand_res = new TrackingResult(frame_result.hands[i].pos_x,
                frame_result.hands[i].pos_y,
                frame_result.hands[i].size,
                frame_result.hands[i].conf,
                i,
                hvc_tracking_result.STB_TRID_NOT_TRACKED);

            this.hands.Add(hand_res);
        }

        // Face detection result
        for (var i = 0; i < frame_result.faces.Count(); i++)
        {
            var face_res = new TrackingFaceResult(frame_result.faces[i].pos_x,
                frame_result.faces[i].pos_y,
                frame_result.faces[i].size,
                frame_result.faces[i].conf,
                i,
                hvc_tracking_result.STB_TRID_NOT_TRACKED);

            // Face direction result
            if (frame_result.faces[i].direction != null)
            {
                face_res.direction = new DirectionResult(frame_result.faces[i].direction.LR,
                                                     frame_result.faces[i].direction.UD,
                                                     frame_result.faces[i].direction.roll,
                                                     frame_result.faces[i].direction.conf);
            }
            //  Age estimation result
            if (frame_result.faces[i].age != null)
            {
                face_res.age = new AgeResult(frame_result.faces[i].age.age, frame_result.faces[i].age.conf);
            }
            // Gender estimation result
            if (frame_result.faces[i].gender != null)
            {
                face_res.gender = new GenderResult(frame_result.faces[i].gender.gender, frame_result.faces[i].gender.conf);
            }
            // Gaze estimation result
            if (frame_result.faces[i].gaze != null)
            {
                face_res.gaze = new GazeResult(frame_result.faces[i].gaze.gazeLR, frame_result.faces[i].gaze.gazeUD);
            }
            // Blink estimation result
            if (frame_result.faces[i].blink != null)
            {
                face_res.blink = new BlinkResult(frame_result.faces[i].blink.ratioR, frame_result.faces[i].blink.ratioL);
            }
            // Expression estimation result
            if (frame_result.faces[i].expression != null)
            {
                face_res.expression = new ExpressionResult(frame_result.faces[i].expression.neutral,
                                         frame_result.faces[i].expression.happiness,
                                         frame_result.faces[i].expression.surprise,
                                         frame_result.faces[i].expression.anger,
                                         frame_result.faces[i].expression.sadness,
                                         frame_result.faces[i].expression.neg_pos);
            }
            // Face recognition result
            if (frame_result.faces[i].recognition != null)
            {
                face_res.recognition = new RecognitionResult(frame_result.faces[i].recognition.uid,
                                                          frame_result.faces[i].recognition.score);
            }

            // Appends to face list.
            this.faces.Add(face_res);
        }
    }
}