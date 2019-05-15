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

public class DetectionResult
{
    // General purpose detection result
    public int pos_x;
    public int pos_y;
    public int size;
    public int conf;

    public DetectionResult(int pos_x = 0, int pos_y = 0, int size = 0, int conf = 0)
    {
        this.pos_x = pos_x;
        this.pos_y = pos_y;
        this.size = size;
        this.conf = conf;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("");
        sb.Append("X:").Append(this.pos_x.ToString());
        sb.Append(" Y:").Append(this.pos_y.ToString());
        sb.Append(" Size:").Append(this.size.ToString());
        sb.Append(" Conf:").Append(this.conf.ToString());

        return sb.ToString();
    }
}

public class FaceResult : DetectionResult
{
    // Detection result for face
    public DirectionResult direction;
    public AgeResult age;
    public GenderResult gender;
    public GazeResult gaze;
    public BlinkResult blink;
    public ExpressionResult expression;
    public RecognitionResult recognition;

    public FaceResult(int pos_x = 0, int pos_y = 0, int size = 0, int conf = 0)
        : base(pos_x, pos_y, size, conf)
    {
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

        sb.AppendLine(base.ToString());

        if (this.direction != null)
            sb.Append("\t\t").AppendLine(this.direction.ToString());
        if (this.age != null)
            sb.Append("\t\t").AppendLine(this.age.ToString());
        if (this.gender != null)
            sb.Append("\t\t").AppendLine(this.gender.ToString());
        if (this.gaze != null)
            sb.Append("\t\t").AppendLine(this.gaze.ToString());
        if (this.blink != null)
            sb.Append("\t\t").AppendLine(this.blink.ToString());
        if (this.expression != null)
            sb.Append("\t\t").AppendLine(this.expression.ToString());
        if (this.recognition != null)
            sb.Append("\t\t").AppendLine(this.recognition.ToString());

        return sb.ToString();
    }
}
public class DirectionResult
{
    // Result for Facial direction estimation
    public int LR;
    public int UD;
    public int roll;
    public int conf;

    public DirectionResult(int LR = 0, int UD = 0, int roll = 0, int conf = 0)
    {
        this.LR = LR;
        this.UD = UD;
        this.roll = roll;
        this.conf = conf;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Direction     ");

        sb.Append("LR:").Append(this.LR.ToString());
        sb.Append(" UD:").Append(this.UD.ToString());
        sb.Append(" Roll:").Append(this.roll.ToString());
        sb.Append(" Conf:").Append(this.conf.ToString());

        return sb.ToString();
    }
}

public class AgeResult
{
    // Result of Age estimation
    public int age;
    public int conf;

    public AgeResult(int age = 0, int conf = 0)
    {
        this.age = age;
        this.conf = conf;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Age           ");

        if (this.age == p2def.EST_NOT_POSSIBLE)
        {
            sb.Append("Age:- ");
        }
        else
        {
            sb.Append("Age:").Append(this.age.ToString()).Append(" ");
        }

        sb.Append("Conf:").Append(this.conf.ToString());

        return sb.ToString();
    }
}

public class GenderResult
{
    public const int GENDER_UNKNOWN = -1;
    public const int GENDER_FEMALE = 0;
    public const int GENDER_MALE = 1;

    public Dictionary<int, string> gender_dic = new Dictionary<int, string>()
    {
                { GENDER_UNKNOWN, "Unknown" },
                { GENDER_FEMALE, "Female" },
                { GENDER_MALE, "Male" },
    };
    // Result of Gender estimation
    public int gender;
    public int conf;

    public GenderResult(int gender = 0, int conf = 0)
    {
        this.gender = gender;
        this.conf = conf;
    }

    public override string ToString()
    {
        int _dic_key;

        if (this.gender == p2def.EST_NOT_POSSIBLE)
        {
            _dic_key = GENDER_UNKNOWN;
        }
        else
        {
            _dic_key = this.gender;
        }

        var sb = new StringBuilder("Gender        ");
        sb.Append("Gender:").Append(gender_dic[_dic_key].ToString());
        sb.Append(" Conf:").Append(this.conf.ToString());

        return sb.ToString();
    }
}

public class GazeResult
{
    // Result of Gaze estimation
    public int gazeLR;
    public int gazeUD;

    public GazeResult(int gazeLR = 0, int gazeUD = 0)
    {
        this.gazeLR = gazeLR;
        this.gazeUD = gazeUD;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Gaze          ");
        sb.Append("LR:").Append(this.gazeLR.ToString());
        sb.Append(" UD:").Append(this.gazeUD.ToString());

        return sb.ToString();
    }
}

public class BlinkResult
{
    // Result of Blink estimation
    public int ratioR;
    public int ratioL;

    public BlinkResult(int ratioR = 0, int ratioL = 0)
    {
        this.ratioR = ratioR;
        this.ratioL = ratioL;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Blink         ");
        sb.Append("R:").Append(this.ratioR.ToString());
        sb.Append(" L:").Append(this.ratioL.ToString());

        return sb.ToString();
    }
}

public class ExpressionResult
{
    // Result of Expression estimation
    public int neutral;
    public int happiness;
    public int surprise;
    public int anger;
    public int sadness;
    public int neg_pos;

    public const int EXP_UNKNOWN = -1;
    public const int EXP_NEUTRAL = 0;
    public const int EXP_HAPPINESS = 1;
    public const int EXP_SURPRISE = 2;
    public const int EXP_ANGER = 3;
    public const int EXP_SADNESS = 4;

    public Dictionary<int, string> exp_dic = new Dictionary<int, string>()
    {
                { EXP_UNKNOWN, "Unknown" },
                { EXP_NEUTRAL, "Neutral" },
                { EXP_HAPPINESS, "Happiness" },
                { EXP_SURPRISE, "Surprise" },
                { EXP_ANGER, "Anger" },
                { EXP_SADNESS, "Sadness" },
    };

    public struct TOP1_RET
    {
        public string exp_str;
        public int max_score;
    }

    public ExpressionResult(int neutral = 0, int happiness = 0, int surprise = 0,
                                 int anger = 0, int sadness = 0, int neg_pos = 0, int degree = 0)
    {
        this.neutral = neutral;
        this.happiness = happiness;
        this.surprise = surprise;
        this.anger = anger;
        this.sadness = sadness;
        this.neg_pos = neg_pos;
    }

    public TOP1_RET get_top1()
    {
        int max_score;
        int max_idx;
        TOP1_RET ret_top1value;

        var x = new List<int> { this.neutral, this.happiness, this.surprise, this.anger, this.sadness };

        max_score = x.Max();
        if (max_score == p2def.EST_NOT_POSSIBLE)
        {
            max_idx = EXP_UNKNOWN;
        }
        else
        {
            max_idx = x.IndexOf(max_score);
        }

        ret_top1value.exp_str = exp_dic[max_idx];
        ret_top1value.max_score = max_score;

        return ret_top1value;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("Expression    ");

        if (this.neutral == p2def.EST_NOT_POSSIBLE)
        {
            sb.Append("Exp:- Score:- (Neutral:- Happiness:- Surprise:- Anger:- Sadness:- NegPos:-");
        }
        else
        {
            TOP1_RET top1_value = this.get_top1();


            sb.Append("Exp:").Append(top1_value.exp_str.ToString());
            sb.Append(" Score:").Append(top1_value.max_score.ToString()).AppendLine();
            sb.Append("                    Neutral:").Append(this.neutral.ToString()).AppendLine();
            sb.Append("                    Happiness:").Append(this.happiness.ToString()).AppendLine();
            sb.Append("                    Surprise:").Append(this.surprise.ToString()).AppendLine();
            sb.Append("                    Anger:").Append(this.anger.ToString()).AppendLine();
            sb.Append("                    Sadness:").Append(this.sadness.ToString()).AppendLine();
            sb.Append("                    NegPos:").Append(this.neg_pos.ToString());
        }

        return sb.ToString();
    }
}


public class RecognitionResult
{
    // Result of Recognition
    public int uid;
    public int score;

    public RecognitionResult(int uid = 0, int score = 0)
    {
        this.uid = uid;
        this.score = score;
    }

    public override string ToString()
    {
        var sb = new StringBuilder("");

        if (this.uid == p2def.RECOG_NO_DATA_IN_ALBUM)
        {
            sb.Append("Recognition   No data is registered in the album.");
        }
        else if (this.uid == p2def.RECOG_NOT_POSSIBLE)
        {
            sb.Append("Recognition   Uid:- Score:").Append(this.score);
        }
        else if (this.uid == -1)
        {
            sb.Append("Recognition   Uid:Unknown Score:").Append(this.score);
        }
        else
        {
            sb.Append("Recognition   Uid:").Append(this.uid);
            sb.Append(" Score:").Append(this.score);
        }

        return sb.ToString();
    }
}
