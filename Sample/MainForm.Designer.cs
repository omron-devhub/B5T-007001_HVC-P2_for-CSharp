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

namespace WindowsFormsApplication1
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StablirizationCheck = new System.Windows.Forms.CheckBox();
            this.ComPortName = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.LogViewArea = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CpyLogViewArea = new System.Windows.Forms.ToolStripMenuItem();
            this.ClrLogViewArea = new System.Windows.Forms.ToolStripMenuItem();
            this.BodyDetectionCheck = new System.Windows.Forms.CheckBox();
            this.HandDetectionCheck = new System.Windows.Forms.CheckBox();
            this.FaceDetectionCheck = new System.Windows.Forms.CheckBox();
            this.FaceDirectionCheck = new System.Windows.Forms.CheckBox();
            this.AgeDetectionCheck = new System.Windows.Forms.CheckBox();
            this.GenderDetectionCheck = new System.Windows.Forms.CheckBox();
            this.GazeDetectionCheck = new System.Windows.Forms.CheckBox();
            this.BlinkDetectionCheck = new System.Windows.Forms.CheckBox();
            this.ExpressionDetectionCheck = new System.Windows.Forms.CheckBox();
            this.RecognitionDetectionCheck = new System.Windows.Forms.CheckBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.SeparateLine = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StablirizationCheck
            // 
            this.StablirizationCheck.AutoSize = true;
            this.StablirizationCheck.Checked = true;
            this.StablirizationCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StablirizationCheck.Location = new System.Drawing.Point(149, 416);
            this.StablirizationCheck.Name = "StablirizationCheck";
            this.StablirizationCheck.Size = new System.Drawing.Size(86, 16);
            this.StablirizationCheck.TabIndex = 1;
            this.StablirizationCheck.Text = "Stabilization";
            this.StablirizationCheck.UseVisualStyleBackColor = true;
            // 
            // ComPortName
            // 
            this.ComPortName.FormattingEnabled = true;
            this.ComPortName.Location = new System.Drawing.Point(13, 412);
            this.ComPortName.Name = "ComPortName";
            this.ComPortName.Size = new System.Drawing.Size(121, 20);
            this.ComPortName.TabIndex = 2;
            // 
            // btnRegister
            // 
            this.btnRegister.Enabled = false;
            this.btnRegister.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRegister.Location = new System.Drawing.Point(305, 461);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(286, 23);
            this.btnRegister.TabIndex = 5;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // LogViewArea
            // 
            this.LogViewArea.BackColor = System.Drawing.SystemColors.HighlightText;
            this.LogViewArea.ContextMenuStrip = this.contextMenuStrip1;
            this.LogViewArea.Font = new System.Drawing.Font("MS Gothic", 12F);
            this.LogViewArea.Location = new System.Drawing.Point(13, 13);
            this.LogViewArea.Multiline = true;
            this.LogViewArea.Name = "LogViewArea";
            this.LogViewArea.ReadOnly = true;
            this.LogViewArea.Size = new System.Drawing.Size(578, 390);
            this.LogViewArea.TabIndex = 6;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CpyLogViewArea,
            this.ClrLogViewArea});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 48);
            // 
            // CpyLogViewArea
            // 
            this.CpyLogViewArea.Name = "CpyLogViewArea";
            this.CpyLogViewArea.Size = new System.Drawing.Size(106, 22);
            this.CpyLogViewArea.Text = "Copy";
            this.CpyLogViewArea.Click += new System.EventHandler(this.CpyLogViewArea_Click);
            // 
            // ClrLogViewArea
            // 
            this.ClrLogViewArea.Name = "ClrLogViewArea";
            this.ClrLogViewArea.Size = new System.Drawing.Size(106, 22);
            this.ClrLogViewArea.Text = "Clear";
            this.ClrLogViewArea.Click += new System.EventHandler(this.ClrLogViewArea_Click);
            // 
            // BodyDetectionCheck
            // 
            this.BodyDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.BodyDetectionCheck.AutoSize = true;
            this.BodyDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BodyDetectionCheck.Location = new System.Drawing.Point(597, 15);
            this.BodyDetectionCheck.Name = "BodyDetectionCheck";
            this.BodyDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.BodyDetectionCheck.TabIndex = 8;
            this.BodyDetectionCheck.Text = "Body Detection";
            this.BodyDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // HandDetectionCheck
            // 
            this.HandDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.HandDetectionCheck.AutoSize = true;
            this.HandDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HandDetectionCheck.Location = new System.Drawing.Point(597, 43);
            this.HandDetectionCheck.Name = "HandDetectionCheck";
            this.HandDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.HandDetectionCheck.TabIndex = 8;
            this.HandDetectionCheck.Text = "Hand Detection";
            this.HandDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // FaceDetectionCheck
            // 
            this.FaceDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.FaceDetectionCheck.AutoSize = true;
            this.FaceDetectionCheck.Checked = true;
            this.FaceDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FaceDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FaceDetectionCheck.Location = new System.Drawing.Point(597, 71);
            this.FaceDetectionCheck.Name = "FaceDetectionCheck";
            this.FaceDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.FaceDetectionCheck.TabIndex = 8;
            this.FaceDetectionCheck.Text = "Face Detection";
            this.FaceDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // FaceDirectionCheck
            // 
            this.FaceDirectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.FaceDirectionCheck.AutoSize = true;
            this.FaceDirectionCheck.Checked = true;
            this.FaceDirectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FaceDirectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FaceDirectionCheck.Location = new System.Drawing.Point(597, 99);
            this.FaceDirectionCheck.Name = "FaceDirectionCheck";
            this.FaceDirectionCheck.Size = new System.Drawing.Size(99, 22);
            this.FaceDirectionCheck.TabIndex = 8;
            this.FaceDirectionCheck.Text = "Face Direction";
            this.FaceDirectionCheck.UseVisualStyleBackColor = true;
            // 
            // AgeDetectionCheck
            // 
            this.AgeDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.AgeDetectionCheck.AutoSize = true;
            this.AgeDetectionCheck.Checked = true;
            this.AgeDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AgeDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AgeDetectionCheck.Location = new System.Drawing.Point(597, 127);
            this.AgeDetectionCheck.Name = "AgeDetectionCheck";
            this.AgeDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.AgeDetectionCheck.TabIndex = 8;
            this.AgeDetectionCheck.Text = "Age           ";
            this.AgeDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // GenderDetectionCheck
            // 
            this.GenderDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.GenderDetectionCheck.AutoSize = true;
            this.GenderDetectionCheck.Checked = true;
            this.GenderDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenderDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GenderDetectionCheck.Location = new System.Drawing.Point(597, 155);
            this.GenderDetectionCheck.Name = "GenderDetectionCheck";
            this.GenderDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.GenderDetectionCheck.TabIndex = 8;
            this.GenderDetectionCheck.Text = "Gender        ";
            this.GenderDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // GazeDetectionCheck
            // 
            this.GazeDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.GazeDetectionCheck.AutoSize = true;
            this.GazeDetectionCheck.Checked = true;
            this.GazeDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GazeDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.GazeDetectionCheck.Location = new System.Drawing.Point(597, 183);
            this.GazeDetectionCheck.Name = "GazeDetectionCheck";
            this.GazeDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.GazeDetectionCheck.TabIndex = 8;
            this.GazeDetectionCheck.Text = "Gaze          ";
            this.GazeDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // BlinkDetectionCheck
            // 
            this.BlinkDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.BlinkDetectionCheck.AutoSize = true;
            this.BlinkDetectionCheck.Checked = true;
            this.BlinkDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BlinkDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BlinkDetectionCheck.Location = new System.Drawing.Point(598, 211);
            this.BlinkDetectionCheck.Name = "BlinkDetectionCheck";
            this.BlinkDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.BlinkDetectionCheck.TabIndex = 8;
            this.BlinkDetectionCheck.Text = "Blink         ";
            this.BlinkDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // ExpressionDetectionCheck
            // 
            this.ExpressionDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.ExpressionDetectionCheck.AutoSize = true;
            this.ExpressionDetectionCheck.Checked = true;
            this.ExpressionDetectionCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ExpressionDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ExpressionDetectionCheck.Location = new System.Drawing.Point(598, 239);
            this.ExpressionDetectionCheck.Name = "ExpressionDetectionCheck";
            this.ExpressionDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.ExpressionDetectionCheck.TabIndex = 8;
            this.ExpressionDetectionCheck.Text = "Expression    ";
            this.ExpressionDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // RecognitionDetectionCheck
            // 
            this.RecognitionDetectionCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.RecognitionDetectionCheck.AutoSize = true;
            this.RecognitionDetectionCheck.Font = new System.Drawing.Font("MS Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.RecognitionDetectionCheck.Location = new System.Drawing.Point(597, 267);
            this.RecognitionDetectionCheck.Name = "RecognitionDetectionCheck";
            this.RecognitionDetectionCheck.Size = new System.Drawing.Size(99, 22);
            this.RecognitionDetectionCheck.TabIndex = 8;
            this.RecognitionDetectionCheck.Text = "Recognition   ";
            this.RecognitionDetectionCheck.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            this.btnExecute.Enabled = false;
            this.btnExecute.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnExecute.Location = new System.Drawing.Point(13, 461);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(286, 23);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // SeparateLine
            // 
            this.SeparateLine.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SeparateLine.Location = new System.Drawing.Point(13, 447);
            this.SeparateLine.MaximumSize = new System.Drawing.Size(578, 2);
            this.SeparateLine.MinimumSize = new System.Drawing.Size(578, 2);
            this.SeparateLine.Name = "SeparateLine";
            this.SeparateLine.Size = new System.Drawing.Size(578, 2);
            this.SeparateLine.TabIndex = 10;
            this.SeparateLine.Text = "label1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(707, 505);
            this.Controls.Add(this.SeparateLine);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.RecognitionDetectionCheck);
            this.Controls.Add(this.ExpressionDetectionCheck);
            this.Controls.Add(this.BlinkDetectionCheck);
            this.Controls.Add(this.GazeDetectionCheck);
            this.Controls.Add(this.GenderDetectionCheck);
            this.Controls.Add(this.AgeDetectionCheck);
            this.Controls.Add(this.FaceDirectionCheck);
            this.Controls.Add(this.FaceDetectionCheck);
            this.Controls.Add(this.HandDetectionCheck);
            this.Controls.Add(this.BodyDetectionCheck);
            this.Controls.Add(this.LogViewArea);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.ComPortName);
            this.Controls.Add(this.StablirizationCheck);
            this.Name = "Main";
            this.Text = "Sample";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox StablirizationCheck;
        private System.Windows.Forms.ComboBox ComPortName;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.TextBox LogViewArea;
        private System.Windows.Forms.CheckBox BodyDetectionCheck;
        private System.Windows.Forms.CheckBox HandDetectionCheck;
        private System.Windows.Forms.CheckBox FaceDetectionCheck;
        private System.Windows.Forms.CheckBox FaceDirectionCheck;
        private System.Windows.Forms.CheckBox AgeDetectionCheck;
        private System.Windows.Forms.CheckBox GenderDetectionCheck;
        private System.Windows.Forms.CheckBox GazeDetectionCheck;
        private System.Windows.Forms.CheckBox BlinkDetectionCheck;
        private System.Windows.Forms.CheckBox ExpressionDetectionCheck;
        private System.Windows.Forms.CheckBox RecognitionDetectionCheck;
        private System.Windows.Forms.Label SeparateLine;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CpyLogViewArea;
        private System.Windows.Forms.ToolStripMenuItem ClrLogViewArea;
    }
}

