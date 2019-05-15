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

public class HVCP2Wrapper
{
    public int RESPONSE_HEADER_SIZE = 6;
    public int SYNC_CODE = 0xFE;

    // UART baudrate definition.  : for set_uart_baudrate()
    public int HVC_UART_BAUD_9600 = 0x00;  //   9600 baud
    public int HVC_UART_BAUD_38400 = 0x01;  //  38400 baud
    public int HVC_UART_BAUD_115200 = 0x02;  // 115200 baud
    public int HVC_UART_BAUD_230400 = 0x03;  // 230400 baud
    public int HVC_UART_BAUD_460800 = 0x04;  // 460800 baud
    public int HVC_UART_BAUD_921600 = 0x05;  // 921600 baud

    // HVC command header fixed part definition
    public List<byte> HVC_CMD_HDR_GETVERSION = new List<byte> { 0xFE, 0x00, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_SET_CAMERA_ANGLE = new List<byte> { 0xFE, 0x01, 0x01, 0x00 };
    public List<byte> HVC_CMD_HDR_GET_CAMERA_ANGLE = new List<byte> { 0xFE, 0x02, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_EXECUTE = new List<byte> { 0xFE, 0x04, 0x03, 0x00 };
    public List<byte> HVC_CMD_HDR_SET_THRESHOLD = new List<byte> { 0xFE, 0x05, 0x08, 0x00 };
    public List<byte> HVC_CMD_HDR_GET_THRESHOLD = new List<byte> { 0xFE, 0x06, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_SET_DETECTION_SIZE = new List<byte> { 0xFE, 0x07, 0x0C, 0x00 };
    public List<byte> HVC_CMD_HDR_GET_DETECTION_SIZE = new List<byte> { 0xFE, 0x08, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_SET_FACE_ANGLE = new List<byte> { 0xFE, 0x09, 0x02, 0x00 };
    public List<byte> HVC_CMD_HDR_GET_FACE_ANGLE = new List<byte> { 0xFE, 0x0A, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_SET_UART_BAUDRATE = new List<byte> { 0xFE, 0x0E, 0x01, 0x00 };
    public List<byte> HVC_CMD_HDR_REGISTER_DATA = new List<byte> { 0xFE, 0x10, 0x03, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_DELETE_DATA = new List<byte> { 0xFE, 0x11, 0x03, 0x00 };
    public List<byte> HVC_CMD_HDR_DELETE_USER = new List<byte> { 0xFE, 0x12, 0x02, 0x00 };
    public List<byte> HVC_CMD_HDR_DELETE_ALL_DATA = new List<byte> { 0xFE, 0x13, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_USER_DATA = new List<byte> { 0xFE, 0x15, 0x02, 0x00 };
    public List<byte> HVC_CMD_HDR_SAVE_ALBUM = new List<byte> { 0xFE, 0x20, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_LOAD_ALBUM = new List<byte> { 0xFE, 0x21, 0x04, 0x00 };
    public List<byte> HVC_CMD_HDR_SAVE_ALBUM_ON_FLASH = new List<byte> { 0xFE, 0x22, 0x00, 0x00 };
    public List<byte> HVC_CMD_HDR_REFORMAT_FLASH = new List<byte> { 0xFE, 0x30, 0x00, 0x00 };

    public Connector _connector;

    public struct GET_VERSION_RET
    {
        public int response_code;
        public string hvc_type;
        public int major;
        public int minor;
        public int release;
        public int revision;
    }
    public struct GET_CAMERA_ANGLE_RET
    {
        public int response_code;
        public int camera_angle;
    }
    public struct get_threshold_value
    {
        public int response_code;
        public int body_thresh;
        public int hand_thresh;
        public int face_thresh;
        public int recognition_thresh;
    }
    public struct get_detection_size_value
    {
        public int response_code;
        public int min_body;
        public int max_body;
        public int min_hand;
        public int max_hand;
        public int min_face;
        public int max_face;
    }

    public struct get_get_face_angle_value
    {
        public int response_code;
        public int yaw_angle;
        public int roll_angle;
    }

    public struct get_user_data_value
    {
        public int response_code;
        public byte[] data_list;
    }

    public struct save_album_value
    {
        public int response_code;
        public byte[] data;
    }

    public struct send_result
    {
        public int response_code;
        public int data_len;
        public byte[] data;
    }
    public struct head_result
    {
        public int response_code;
        public int data_len;
    }

    public HVCP2Wrapper(Connector connector)
    {
        this._connector = connector;
    }

    public bool connect(int com_port, int baudrate, int timeout)
    {
        // Connects to HVC-P2 by COM port via USB or UART interface.
        if (baudrate == 0)
        {
            throw new Exception("  Invalid baudrate:" + baudrate.ToString());
        }

        return this._connector.connect(com_port, baudrate, timeout);
    }

    public void disconnect()
    {
        // Disconnects to HVC-P2.
        this._connector.disconnect();
    }

    public GET_VERSION_RET get_version()
    {
        var cmd = HVC_CMD_HDR_GETVERSION;
        GET_VERSION_RET ret;

        var send_ret = this._send_command(cmd.ToArray(), cmd.ToArray().Length);
        ret.response_code = send_ret.response_code;

        if (ret.response_code == 0x00)
        {
            // Success
            // Copy 12Byte
            ret.hvc_type = System.Text.Encoding.ASCII.GetString(send_ret.data, 0, 12);

            ret.major = send_ret.data[12];
            ret.minor = send_ret.data[13];
            ret.release = send_ret.data[14];
            ret.revision = BitConverter.ToInt32(send_ret.data, 15);
        }
        else
        {
            ret.hvc_type = null;
            ret.major = 0;
            ret.minor = 0;
            ret.release = 0;
            ret.revision = 0;
        }

        return ret;
    }

    public int set_camera_angle(int camera_angle)
    {
        // Sets camera angle.
        var cmd =  new List<byte>(HVC_CMD_HDR_SET_CAMERA_ANGLE);
        cmd.Add((byte)camera_angle);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);
        return sendResult.response_code;
    }

    public GET_CAMERA_ANGLE_RET get_camera_angle()
    {
        // Gets camera angle.
        var cmd = HVC_CMD_HDR_GET_CAMERA_ANGLE;
        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        GET_CAMERA_ANGLE_RET retvalue;

        retvalue.response_code = sendResult.response_code;

        if (retvalue.response_code == 0)
        {
            retvalue.camera_angle = (byte)sendResult.data[0];
        }
        else
        {
            retvalue.camera_angle = 0;
        }

        return retvalue;
    }


    public int execute(int exec_func, int out_img_type, HVCResult frame_result, GrayscaleImage img)
    {
        var cmd = new List<byte>(HVC_CMD_HDR_EXECUTE);
        // Executes specified functions. e.g. Face detection, Age estimation, etc

        //  Adds face flag if using facial estimation function
        if ((exec_func & (p2def.EX_DIRECTION | p2def.EX_AGE | p2def.EX_GENDER | p2def.EX_GAZE | p2def.EX_BLINK | p2def.EX_EXPRESSION)) != 0x00)
        {
            exec_func |= p2def.EX_FACE + p2def.EX_DIRECTION;
        }

        cmd.AddRange(BitConverter.GetBytes((UInt16)(exec_func)));
        cmd.Add((byte)out_img_type);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        if (sendResult.response_code == 0x00)
        {
            // Success
            int rc = frame_result.read_from_buffer(exec_func, sendResult.data_len, sendResult.data);

            if (out_img_type != 0x00)
            {
                img.width = BitConverter.ToInt16(sendResult.data, rc);
                img.height = BitConverter.ToInt16(sendResult.data, (rc + 2));
                img.data = sendResult.data.Skip(rc + 4).Take(img.width * img.height).ToArray();
            }
        }
        return sendResult.response_code;
    }

    public int set_threshold(int body_thresh, int hand_thresh, int face_thresh,
                              int recognition_thresh)
    {
        var cmd = new List<byte>(HVC_CMD_HDR_SET_THRESHOLD);
        // Sets the thresholds value for Human body detection, Hand detection,
        // Face detection and/or Recongnition.

        cmd.AddRange(BitConverter.GetBytes((UInt16)(body_thresh)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(hand_thresh)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(face_thresh)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(recognition_thresh)));

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public get_threshold_value get_threshold()
    {
        // Gets the thresholds value for Human body detection, Hand detection,
        // Face detection and/or Recongnition.
        get_threshold_value ret_value;
        var cmd = HVC_CMD_HDR_GET_THRESHOLD;
        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);
        ret_value.response_code = sendResult.response_code;

        if (sendResult.response_code == 0)
        {
            // Success
            ret_value.body_thresh = BitConverter.ToInt16(sendResult.data, 0);
            ret_value.hand_thresh = BitConverter.ToInt16(sendResult.data, 2);
            ret_value.face_thresh = BitConverter.ToInt16(sendResult.data, 4);
            ret_value.recognition_thresh = BitConverter.ToInt16(sendResult.data, 6);
        }
        else
        {
            // error
            ret_value.body_thresh = 0;
            ret_value.hand_thresh = 0;
            ret_value.face_thresh = 0;
            ret_value.recognition_thresh = 0;
        }

        return ret_value;
    }

    public int set_detection_size(int min_body, int max_body, int min_hand, int max_hand,
                              int min_face, int max_face)
    {
        var cmd = new List<byte>(HVC_CMD_HDR_SET_DETECTION_SIZE);
        // Sets the detection size for Human body detection, Hand detection
        // and/or Face detection

        cmd.AddRange(BitConverter.GetBytes((UInt16)(min_body)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(max_body)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(min_hand)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(max_hand)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(min_face)));
        cmd.AddRange(BitConverter.GetBytes((UInt16)(max_face)));

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;

    }

    public get_detection_size_value get_detection_size()
    {
        var cmd = HVC_CMD_HDR_GET_DETECTION_SIZE;
        // Gets the detection size for Human body detection, Hand detection
        // and/or Face detection
        get_detection_size_value ret_value;

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);
        ret_value.response_code = sendResult.response_code;

        if (sendResult.response_code == 0x00)
        {
            // Success
            ret_value.min_body = BitConverter.ToInt16(sendResult.data, 0);
            ret_value.max_body = BitConverter.ToInt16(sendResult.data, 2);
            ret_value.min_hand = BitConverter.ToInt16(sendResult.data, 4);
            ret_value.max_hand = BitConverter.ToInt16(sendResult.data, 6);
            ret_value.min_face = BitConverter.ToInt16(sendResult.data, 8);
            ret_value.max_face = BitConverter.ToInt16(sendResult.data, 10);
        }
        else
        {
            // error
            ret_value.min_body = 0;
            ret_value.max_body = 0;
            ret_value.min_hand = 0;
            ret_value.max_hand = 0;
            ret_value.min_face = 0;
            ret_value.max_face = 0;
        }

        return ret_value;
    }

    public int set_face_angle(int yaw_angle, int roll_angle)
    {
        var cmd = new List<byte>(HVC_CMD_HDR_SET_FACE_ANGLE);

        // Sets the face angle, i.e. the yaw angle range and the roll angle
        // range for Face detection.
        cmd.Add((byte)yaw_angle);
        cmd.Add((byte)roll_angle);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public get_get_face_angle_value get_face_angle()
    {
        var cmd = HVC_CMD_HDR_GET_FACE_ANGLE;
        get_get_face_angle_value ret_value;

        // Gets the face angle range for Face detection
        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);
        ret_value.response_code = sendResult.response_code;

        if (sendResult.response_code == 0x00)
        {
            // Success
            ret_value.yaw_angle = sendResult.data[0];
            ret_value.roll_angle = sendResult.data[1];
        }
        else
        {
            // error
            ret_value.yaw_angle = 0;
            ret_value.roll_angle = 0;
        }

        return ret_value;
    }

    public int set_uart_baudrate(int baudrate)
    {
        var cmd = new List<byte>(HVC_CMD_HDR_SET_UART_BAUDRATE);

        // Sets the UART baudrate.

        // if baudrate not in AVAILABLE_BAUD:
        //     raise ValueError("Invalid baudrate:{0!r}".format(baudrate))
        cmd.Add((byte)baudrate);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public int register_data(int user_id, int data_id, GrayscaleImage img)
    {
        // Registers data for Recognition and gets a normalized image.
        var cmd = new List<byte>(HVC_CMD_HDR_REGISTER_DATA);

        cmd.AddRange(BitConverter.GetBytes((UInt16)(user_id)));
        cmd.Add((byte)data_id);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        if (sendResult.response_code == 0x00)
        {
            // Success
            img.width = BitConverter.ToInt16(sendResult.data, 0);
            img.height = BitConverter.ToInt16(sendResult.data, 2);
            img.data = sendResult.data.Skip(4).Take(img.width * img.height).ToArray();
        }
        return sendResult.response_code;
    }

    public int delete_data(int user_id, int data_id)
    {
        // Deletes a specified registered data.
        var cmd = new List<byte>(HVC_CMD_HDR_DELETE_DATA);

        cmd.AddRange(BitConverter.GetBytes((UInt16)(user_id)));
        cmd.Add((byte)data_id);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }


    public int delete_user(int user_id)
    {
        // Deletes a specified registered user. 
        var cmd = new List<byte>(HVC_CMD_HDR_DELETE_USER);

        cmd.AddRange(BitConverter.GetBytes((UInt16)(user_id)));

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public int delete_all_data()
    {
        // Deletes all the registered data.
        var cmd = HVC_CMD_HDR_DELETE_ALL_DATA;

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public get_user_data_value get_user_data(int user_id)
    {
        get_user_data_value ret_user_data_value;

        // Gets the registration info of a specified user.
        var cmd = new List<byte>(HVC_CMD_HDR_USER_DATA);

        cmd.AddRange(BitConverter.GetBytes((UInt16)(user_id)));

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        ret_user_data_value.response_code = sendResult.response_code;

        if (sendResult.response_code == 0x00)
        {
            // Success
            ret_user_data_value.data_list = sendResult.data.Take(2).ToArray();
        }
        else
        {
            ret_user_data_value.data_list = new byte[2];
        }

        return ret_user_data_value;
    }

    public save_album_value save_album()
    {
        save_album_value ret_value;

        //  Saves the album on the host side.
        var cmd = HVC_CMD_HDR_SAVE_ALBUM;

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        ret_value.response_code = sendResult.response_code;

        if (ret_value.response_code == 0x00)
        {
            // Success
            ret_value.data = sendResult.data.Take(sendResult.data_len).ToArray();
        }
        else
        {
            ret_value.data = new byte[0];
        }

        return ret_value;
    }

    public int load_album(byte[] album)
    {
        // Loads the album from the host side to the device.
        var cmd = new List<byte>(HVC_CMD_HDR_LOAD_ALBUM);

        var a_length = BitConverter.GetBytes(album.Length);

        cmd.AddRange(a_length);
        cmd.AddRange(album);

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public int save_album_to_flash()
    {
        // Saves the album on the flash ROM
        var cmd = HVC_CMD_HDR_SAVE_ALBUM_ON_FLASH;

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public int reformat_flash()
    {
        // Reformats the album save area on the flash ROM.
        var cmd = HVC_CMD_HDR_REFORMAT_FLASH;

        var sendResult = this._send_command(cmd.ToArray(), cmd.ToArray().Length);

        return sendResult.response_code;
    }

    public head_result _receive_header()
    {
        head_result retvalue;

        var read_size = 0;
        byte[] buf;

        buf = this._connector.receive_data(RESPONSE_HEADER_SIZE, out read_size);
        if (read_size != RESPONSE_HEADER_SIZE)
        {
            throw new Exception("  Response header size is not enough.");
        }

        if (buf[0] != SYNC_CODE)
        {
            throw new Exception("  Invalid Sync code.");
        }

        retvalue.response_code = buf[1];
        retvalue.data_len = BitConverter.ToInt32(buf, 2);

        return retvalue;
    }

    public byte[] _receive_data(int data_len)
    {
        var read_size = 0;
        byte[] buf;

        buf = this._connector.receive_data(data_len, out read_size);
        if (read_size != data_len)
        {
            throw new Exception("  Response data size is not enough.");
        }
        return buf;
    }

    public send_result _send_command(byte[] data, int send_length)
    {
        send_result retvalue;

        this._connector.clear_recieve_buffer();
        this._connector.send_data(data, send_length);
        head_result head_data = _receive_header();

        retvalue.response_code = head_data.response_code;
        retvalue.data_len = head_data.data_len;

        if (head_data.response_code == 0x00)
        {
            // Success
            if (head_data.data_len > 0)
            {
                retvalue.data = _receive_data(head_data.data_len);
            }
            else
            {
                retvalue.data = new byte[0];
            }
        }
        else
        {
            //  error
            retvalue.data = null;
        }
        return retvalue;
    }
}
