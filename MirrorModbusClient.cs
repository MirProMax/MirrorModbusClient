using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyModbus;
using System.Security.Permissions;
using UnityEditor.PackageManager;

namespace MirrorModbus
{
    public class MirrorModbusClient : ModbusClient
    {

        //重写构造函数
        public MirrorModbusClient(string inIP, int inPort) : base(inIP, inPort)
        {

        }


        //读取单个40000的寄存器
        public (int, int) read4_Single_Nature(int inAddress_Nature)
        {
            int checkCode;
            int offset = 40000;
            int outValue = 0;

            try
            {
                int address = inAddress_Nature - offset;
                if (address < 1)
                {
                    address = 1;
                }

                outValue = ReadHoldingRegisters(address - 1, 1)[0];
                checkCode = 1;
            }
            catch (Exception e)
            {
                Debug.Log($"MirrorModbusClient: Read4: {e}");
                checkCode = -1;
            }

            return (checkCode, outValue);
        }


        public (int, int[]) read4_Batch_Original(int inStart_Nature, int? inLength_Nature = null)
        {
            int checkCode;
            int[] outArray = { };

            try
            {
                int offset = 40000;
                int arrayStart_Nature = inStart_Nature - offset;

                if (arrayStart_Nature < 1)
                {
                    arrayStart_Nature = 1;
                }

                int length_Nature = (int)((!inLength_Nature.HasValue || inLength_Nature < 1) ? 1 : inLength_Nature);

                outArray = ReadHoldingRegisters(arrayStart_Nature - 1, length_Nature);

                checkCode = 1;
            }
            catch (Exception e)
            {
                Debug.Log($"MirrorModbusClient: Read4Batch: {e}");
                checkCode = -1;
            }

            return (checkCode, outArray);
        }

        //返回值的第[0]个元素用于判断成败,0以上为成功,-1为失败
        //输入两个参数,起始地址和结束地址,从40001开始
        public (int, int[]) read4_Batch_Nature(int inStart_Nature, int? inStop_Nature = null)
        {
            int checkCode;
            int[] outArray = { -1 };

            try
            {
                int offset = 40000;

                int arrayStart_Nature = inStart_Nature - offset;

                //如果停止位没有值,则数组的停止地址等于数组的起始地址(没有从1开始,而不是40001开始),否则数组停止位的值是4000*-40000;
                //如果输入结束地址(4000*)小于起始地址,则数组结束的地址也是数组的起始地址.
                int arrayStop_Nature = (int)((!inStop_Nature.HasValue || inStop_Nature < inStart_Nature) ? arrayStart_Nature : inStop_Nature.Value - offset);

                if (arrayStart_Nature < 1)
                {
                    arrayStart_Nature = 1;
                }

                int length_Nature = arrayStop_Nature - arrayStart_Nature + 1;

                (int, int[]) midArray = read4_Batch_Original(arrayStart_Nature - 1, length_Nature);
                checkCode = midArray.Item1;
                outArray = midArray.Item2;
            }
            catch (Exception e)
            {
                Debug.Log($"MirrorModbusClient: Read4Batch: {e}");
                checkCode = -1;
            }

            return (checkCode, outArray);
        }




        //写入单个40000
        //写入后,返回值为0以上为成功,-1为失败
        public int write4_Single(int inAddress, int inValue)
        {
            try
            {
                int offset = 40000;
                int address = inAddress - offset;
                if (address < 1)
                {
                    return -1;
                }

                WriteSingleRegister(address - 1, inValue);
                return 1;
            }
            catch (Exception e)
            {
                Debug.Log($"MirrorModbusClient: Write4: {e}");
                return -1;
            }
        }


        //批量写入40000
        //Dictionary<inAddress, inValue>
        public int write4_Batch(Dictionary<int, int> inD)
        {
            int resultCode_Single;
            int resultCode_Output = 0;

            foreach (KeyValuePair<int, int> element in inD)
            {
                resultCode_Single = write4_Single(element.Key, element.Value);

                if (resultCode_Single <= -1)
                {
                    resultCode_Output -= 1;
                }
            }

            return resultCode_Output;
        }

    }
}