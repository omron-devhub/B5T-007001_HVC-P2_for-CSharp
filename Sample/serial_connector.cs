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
using System.Runtime.InteropServices;


public class Win32Serial : Connector
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll")]
    public static extern bool GetCommState(IntPtr hFile, ref DCB lpDCB);

    [DllImport("kernel32.dll")]
    public static extern bool SetCommState(IntPtr hFile, [In] ref DCB lpDCB);

    [DllImport("kernel32.dll")]
    public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    public static extern bool PurgeComm(IntPtr hFile, uint dwFlags);


}

[StructLayout(LayoutKind.Sequential)]
public struct DCB
{
    public uint DCBlength;
    public uint BaudRate;
    public uint fBinary;
    public uint fParity;
    public uint fOutxCtsFlow;
    public uint fOutxDsrFlow;
    public uint fDtrControl;
    public uint fDsrSensitivity;
    public uint fTXContinueOnXoff;
    public uint fOutX;
    public uint fInX;
    public uint fErrorChar;
    public uint fNull;
    public uint fRtsControl;
    public uint fAbortOnError;
    public uint fDummy2;
    public ushort wReserved;
    public ushort XonLim;
    public ushort XoffLim;
    public byte ByteSize;
    public byte Parity;
    public byte StopBits;
    public char XonChar;
    public char XoffChar;
    public char ErrorChar;
    public char EofChar;
    public char EvtChar;
    public ushort wReserved1;
}

public class SerialConnector : Connector
{
    const uint PURGE_RXABORT = 0x0002; // Terminates all outstanding overlapped read operations and returns immediately, even if the read operations have not been completed.
    const uint PURGE_RXCLEAR = 0x0008; // Clears the input buffer (if the device driver has one).
    const uint PURGE_TXABORT = 0x0001; // Terminates all outstanding overlapped write operations and returns immediately, even if the write operations have not been completed.
    const uint PURGE_TXCLEAR = 0x0004; // Clears the output buffer (if the device driver has one).

    // Serial connector class
    public bool _is_connected;
    public int com_port;
    public int baudrate;
    public int timeout;

    IntPtr hSerial;

    public SerialConnector()
    {
        this._is_connected = false;
    }

    public override bool connect(int com_port, int baudrate, int timeout)
    {
        this.com_port = com_port;
        this.baudrate = baudrate;
        this.timeout = timeout;


        hSerial = Win32Serial.CreateFile("COM" + this.com_port.ToString(), 0xC0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
        if (hSerial.ToInt32() == -1)
        {
            Console.WriteLine("Error opening serial port");
            throw new Exception("  Can not connect serial port !" + " COM" + this.com_port.ToString() + Environment.NewLine);
        }

        DCB dcb = new DCB();
        if (!Win32Serial.GetCommState(hSerial, ref dcb))
        {
            Console.WriteLine("Error getting comm state");
            Win32Serial.CloseHandle(hSerial);
            return false;
        }

        dcb.BaudRate = (uint)baudrate;
        dcb.ByteSize = 8;
        dcb.Parity = 0;
        dcb.StopBits = 1;

        if (!Win32Serial.SetCommState(hSerial, ref dcb))
        {
            Console.WriteLine("Error setting comm state");
            Win32Serial.CloseHandle(hSerial);
            return false;
        }

        try
        {
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
        Win32Serial.CloseHandle(hSerial);
        this._is_connected = false;
    }

    public override void clear_recieve_buffer()
    {
        bool result = Win32Serial.PurgeComm(hSerial, PURGE_RXCLEAR);
        if (!result)
        {
            // Handle the error
            Console.WriteLine("Failed to clear the buffer.");
        }
        else
        {
            Console.WriteLine("Buffer cleared successfully.");
        }
    }

    public void clear_transmit_buffer()
    {
        bool result = Win32Serial.PurgeComm(hSerial, PURGE_TXCLEAR);
        if (!result)
        {
            // Handle the error
            Console.WriteLine("Failed to clear the buffer.");
        }
        else
        {
            Console.WriteLine("Buffer cleared successfully.");
        }
    }

    public override void send_data(byte[] data, int send_length)
    {
        if (this._is_connected == false)
        {
            throw new Exception("  Serial port has not connected yet!" + Environment.NewLine);
        }

        this.clear_recieve_buffer();
        uint bytesWritten;
        if (Win32Serial.WriteFile(hSerial, data, (uint)data.Length, out bytesWritten, IntPtr.Zero))
        {
            Console.WriteLine("Wrote {0} bytes", bytesWritten);
        }
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
                uint bytesRead;
                var bufferTemp = new byte[1024 * 100];
                Win32Serial.ReadFile(hSerial, bufferTemp, (uint)read_byte_size, out bytesRead, IntPtr.Zero);

                read_cnt = (int)bytesRead;
                Console.WriteLine("{0}", read_cnt);

                if (read_cnt > read_byte_size)
                {
                    read_cnt = read_byte_size;
                }
                if (read_cnt > 0)
                {
                    Array.ConstrainedCopy(bufferTemp, 0 , buffer, buf_index, read_cnt);
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
