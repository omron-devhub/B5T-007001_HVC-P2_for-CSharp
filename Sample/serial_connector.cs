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
using System.IO.Ports;
using System.Diagnostics;
using System.Windows.Forms;

public class SerialConnector : Connector
{
    // Serial connector class
    public bool _is_connected;
    public int com_port;
    public int baudrate;
    public int timeout;
    public SerialPort port;

    public SerialConnector()
    {
        this._is_connected = false;
    }

    public override bool connect(int com_port, int baudrate, int timeout)
    {
        this.com_port = com_port;
        this.baudrate = baudrate;
        this.timeout = timeout;

        this.port = new SerialPort("COM" + this.com_port.ToString(), this.baudrate, Parity.None, 8, StopBits.One);
        this.port.ReadTimeout = this.timeout;
        this.port.Parity = Parity.None;
        this.port.Encoding = Encoding.UTF8;

        try
        {
            this.port.Open();
            this._is_connected = true;
        }
        catch
        {
            throw new Exception("  Can not connect serial port !" + " COM" + this.com_port.ToString() + Environment.NewLine);
        }

        return true;
    }

    public override void disconnect()
    {
        this.port.Close();
        this._is_connected = false;
    }

    public override void clear_recieve_buffer()
    {
        this.port.DiscardInBuffer();
    }

    public override void send_data(byte[] data, int send_length)
    {
        if (this._is_connected == false)
        {
            throw new Exception("  Serial port has not connected yet!" + Environment.NewLine);
        }

        this.port.DiscardInBuffer();
        this.port.Write(data, 0, send_length);
    }

    public override byte[] receive_data(int read_byte_size, out int read_size)
    {
        var buffer = new byte[read_byte_size];
        var read_cnt = 0;
        var buf_index = 0;
        var sw = new Stopwatch();

        if (this._is_connected == false)
        {
            throw new Exception("  Serial port has not connected yet!" + Environment.NewLine);
        }

        sw.Reset();
        sw.Start();

        while (true)
        {
            
            try
            {
                read_cnt = this.port.BytesToRead;

                if (read_cnt > read_byte_size)
                {
                    read_cnt = read_byte_size;
                }
                if (read_cnt > 0)
                {
                    this.port.Read(buffer, buf_index, read_cnt);
                    buf_index += read_cnt;
                    read_byte_size -= read_cnt;

                    if (read_byte_size <= 0)
                    {
                        break;
                    }

                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                }

                if (sw.ElapsedMilliseconds > this.timeout)
                {
                    // timeout
                    break;
                }

            }
            catch
            {
                throw new Exception("  Serial port recieve Error!" + Environment.NewLine);
            }
        }
        read_size = buf_index;

        return buffer;
    }
}
