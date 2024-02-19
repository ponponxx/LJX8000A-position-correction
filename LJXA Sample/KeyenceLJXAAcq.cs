//Copyright (c) 2020 KEYENCE CORPORATION. All rights reserved.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace LJXASample
{
    public struct SetParam
    {
        public int YLineNum;
        public float YPitchUm;
        public int TimeoutMs;
        public int UseExternalBatchStart;
    };

    public struct GetParam
    {
        public int LuminanceEnabled;
        public int XPointNum;
        public int YLinenumAcquired;
        public float XPitchUm;
        public float YPitchUm;
        public float ZPitchUm;
    };

    public static class KeyenceLJXAAcq
    {
        // Static variable
        private static readonly LJX8IF_ETHERNET_CONFIG[] _ethernetConfig = new LJX8IF_ETHERNET_CONFIG[NativeMethods.DeviceCount];
        private static readonly int[] _highSpeedPortNo = new int[NativeMethods.DeviceCount];
        private static readonly int[] _imageAvailable = new int[NativeMethods.DeviceCount];
        private static readonly int[] _lastImageSizeHeight = new int[NativeMethods.DeviceCount];
        private static readonly GetParam[] _getParam = new GetParam[NativeMethods.DeviceCount];
        private static readonly List<ushort>[] _heightBuf = new List<ushort>[NativeMethods.DeviceCount];
        private static readonly List<ushort>[] _luminanceBuf = new List<ushort>[NativeMethods.DeviceCount];
        private static readonly HighSpeedDataCallBackForSimpleArray Callback = MyCallbackFunc;

        public static int OpenDevice(int deviceId, LJX8IF_ETHERNET_CONFIG ethernetConfig, int highSpeedPortNo)
        {
            int errCode = NativeMethods.LJX8IF_EthernetOpen(deviceId, ref ethernetConfig);

            _ethernetConfig[deviceId] = ethernetConfig;
            _highSpeedPortNo[deviceId] = highSpeedPortNo;
            Console.WriteLine(@"[@(OpenDevice) Open device](0x{0:x})", errCode);

            return errCode;
        }

        public static void CloseDevice(int deviceId)
        {
            NativeMethods.LJX8IF_FinalizeHighSpeedDataCommunication(deviceId);
            NativeMethods.LJX8IF_CommunicationClose(deviceId);
            Console.WriteLine(@"[@(CloseDevice) Close device]");
        }


        public static int Acquire(int deviceId, List<ushort> heightImage, List<ushort> luminanceImage,
            SetParam setParam, ref GetParam getParam)
        {
            int yDataNum = setParam.YLineNum;
            int timeoutMs = setParam.TimeoutMs;
            int useExternalBatchStart = setParam.UseExternalBatchStart;
            ushort zUnit = 0;

            _heightBuf[deviceId] = new List<ushort>();
            _luminanceBuf[deviceId] = new List<ushort>();

            //Initialize
            int errCode = NativeMethods.LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(deviceId, ref _ethernetConfig[deviceId],
                (ushort)_highSpeedPortNo[deviceId], Callback, (uint)yDataNum, (uint)deviceId);
            Console.WriteLine(@"[@(Acquire) Initialize HighSpeed](0x{0:x})", errCode);

            //PreStart
            var startReq = new LJX8IF_HIGH_SPEED_PRE_START_REQUEST { bySendPosition = Convert.ToByte(2) };
            var profileInfo = new LJX8IF_PROFILE_INFO();

            errCode = NativeMethods.LJX8IF_PreStartHighSpeedDataCommunication(deviceId, ref startReq, ref profileInfo);
            Console.WriteLine(@"[@(Acquire) PreStart](0x{0:x})", errCode);

            //zUnit 
            errCode = NativeMethods.LJX8IF_GetZUnitSimpleArray(deviceId, ref zUnit);
            if (errCode != 0 || zUnit == 0)
            {
                Console.WriteLine(@"Failed to acquire zUnit.");
                return errCode;
            }

            //Start HighSpeed
            _imageAvailable[deviceId] = 0;
            _lastImageSizeHeight[deviceId] = 0;

            errCode = NativeMethods.LJX8IF_StartHighSpeedDataCommunication(deviceId);
            Console.WriteLine(@"[@(Acquire) Start HighSpeed](0x{0:x})", errCode);

            //StartMeasure(Batch Start)
            if (useExternalBatchStart > 0)
            {
            }
            else
            {
                errCode = NativeMethods.LJX8IF_StartMeasure(deviceId);
                Console.WriteLine(@"[@(Acquire) Measure Start(Batch Start)](0x{0:x})", errCode);
            }

            // Acquire. Polling to confirm complete.
            // Or wait until a timeout occurs.
            Console.WriteLine(@" [@(Acquire) acquiring image...]");
            DateTime startDt = DateTime.UtcNow;
            TimeSpan ts;
            while (true)
            {
                ts = DateTime.UtcNow - startDt;
                if (timeoutMs < ts.TotalMilliseconds)
                {
                    break;
                }
                if (_imageAvailable[deviceId] == 1) break;
            }

            if (_imageAvailable[deviceId] != 1)
            {
                Console.WriteLine(@" [@(Acquire) timeout]");

                //Stop HighSpeed
                errCode = NativeMethods.LJX8IF_StopHighSpeedDataCommunication(deviceId);
                Console.WriteLine(@"[@(Acquire) Stop HighSpeed](0x{0:x})", errCode);

                return (int)Rc.ErrTimeout;
            }
            Console.WriteLine(@" [@(Acquire) done]");

            //Stop HighSpeed
            errCode = NativeMethods.LJX8IF_StopHighSpeedDataCommunication(deviceId);
            Console.WriteLine(@"[@(Acquire) Stop HighSpeed](0x{0:x})", errCode);

            //---------------------------------------------------------------------
            //  Organize parameters related to acquired image 
            //---------------------------------------------------------------------

            _getParam[deviceId].LuminanceEnabled = profileInfo.byLuminanceOutput;
            _getParam[deviceId].XPointNum = profileInfo.nProfileDataCount;
            _getParam[deviceId].YLinenumAcquired = _lastImageSizeHeight[deviceId];
            _getParam[deviceId].XPitchUm = profileInfo.lXPitch / 100.0f;
            _getParam[deviceId].YPitchUm = setParam.YPitchUm;
            _getParam[deviceId].ZPitchUm = zUnit / 100.0f;

            getParam = _getParam[deviceId];

            //---------------------------------------------------------------------
            //  Copy internal buffer to user buffer
            //---------------------------------------------------------------------
            heightImage.AddRange(_heightBuf[deviceId]);

            if (profileInfo.byLuminanceOutput > 0)
            {
                luminanceImage.AddRange(_luminanceBuf[deviceId]);
            }
            NativeMethods.LJX8IF_FinalizeHighSpeedDataCommunication(deviceId);
            return (int)Rc.Ok;

        }

        public static int StartAsync(int deviceId, SetParam setParam)
        {
            int errCode;
            ushort zUnit = 0;

            int yDataNum = setParam.YLineNum;
            int useExternalBatchStart = setParam.UseExternalBatchStart;

            _heightBuf[deviceId] = new List<ushort>();
            _luminanceBuf[deviceId] = new List<ushort>();

            //Initialize
            errCode = NativeMethods.LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(deviceId, ref _ethernetConfig[deviceId],
                (ushort)_highSpeedPortNo[deviceId], Callback, (uint)yDataNum, (uint)deviceId);
            Console.WriteLine(@"[@(StartAsync) Initialize HighSpeed](0x{0:x})", errCode);

            //PreStart
            var startReq = new LJX8IF_HIGH_SPEED_PRE_START_REQUEST { bySendPosition = Convert.ToByte(2) };
            var profileInfo = new LJX8IF_PROFILE_INFO();

            errCode = NativeMethods.LJX8IF_PreStartHighSpeedDataCommunication(deviceId, ref startReq, ref profileInfo);
            Console.WriteLine(@"[@(StartAsync) PreStart](0x{0:x})", errCode);

            //zUnit 
            errCode = NativeMethods.LJX8IF_GetZUnitSimpleArray(deviceId, ref zUnit);
            if (errCode != 0 || zUnit == 0)
            {
                Console.WriteLine(@"Failed to acquire zUnit.");
                return errCode;
            }

            //Start HighSpeed
            _imageAvailable[deviceId] = 0;
            _lastImageSizeHeight[deviceId] = 0;

            errCode = NativeMethods.LJX8IF_StartHighSpeedDataCommunication(deviceId);
            Console.WriteLine(@"[@(StartAsync) Start HighSpeed](0x{0:x})", errCode);

            //StartMeasure(Batch Start)
            if (useExternalBatchStart > 0)
            {
            }
            else
            {
                errCode = NativeMethods.LJX8IF_StartMeasure(deviceId);
                Console.WriteLine(@"[@(StartAsync) Measure Start(Batch Start)](0x{0:x})", errCode);
            }

            //---------------------------------------------------------------------
            //  Organize parameters related to acquired image 
            //---------------------------------------------------------------------

            _getParam[deviceId].LuminanceEnabled = profileInfo.byLuminanceOutput;
            _getParam[deviceId].XPointNum = profileInfo.nProfileDataCount;

            // _getParam(lDeviceId).YLineNumAcquired : This parameter is unknown at this stage. Set later in "AcquireAsync" function.

            _getParam[deviceId].XPitchUm = profileInfo.lXPitch / 100.0f;
            _getParam[deviceId].YPitchUm = setParam.YPitchUm;
            _getParam[deviceId].ZPitchUm = zUnit / 100.0f;

            return (int)Rc.Ok;
        }

        public static int AcquireAsync(int deviceId, List<ushort> heightImage, List<ushort> luminanceImage, SetParam setParam, ref GetParam getParam)
        {
            int errCode;

            if (_imageAvailable[deviceId] != 1)
            {
                return (int)Rc.ErrTimeout;
            }
            Console.WriteLine(@" [@(AcquireAsync) done]");

            //Stop HighSpeed
            errCode = NativeMethods.LJX8IF_StopHighSpeedDataCommunication(deviceId);
            Console.WriteLine(@"[@(AcquireAsync) Stop HighSpeed](0x{0:x})", errCode);

            //---------------------------------------------------------------------
            //  Organaize parameters related to acquired image 
            //---------------------------------------------------------------------
            //The rest parameters are preset in "StartAsync" function.

            _getParam[deviceId].YLinenumAcquired = _lastImageSizeHeight[deviceId];
            getParam = _getParam[deviceId];

            //---------------------------------------------------------------------
            //  Copy internal buffer to user buffer
            //---------------------------------------------------------------------
            heightImage.AddRange(_heightBuf[deviceId]);

            if (_getParam[deviceId].LuminanceEnabled > 0)
            {
                luminanceImage.AddRange(_luminanceBuf[deviceId]);
            }
            return (int)Rc.Ok;
        }

        private static void MyCallbackFunc(IntPtr profileHeaderArray, IntPtr heightProfileArray, IntPtr luminanceProfileArray, uint luminanceEnable, uint profileDataCount, uint count, uint notify, uint user)
        {
            if ((notify != 0) && (notify & 0x10000) != 0) return;
            if (count == 0) return;
            if (_imageAvailable[user] == 1) return;

            var syncObject = new object();
            lock (syncObject)
            {
                int bufferSize = (int)profileDataCount * (int)count;
                ushort[] buffer = new ushort[bufferSize];
                CopyUShort(heightProfileArray, buffer, bufferSize);
                _heightBuf[user].AddRange(buffer);

                if (luminanceEnable == 1)
                {
                    CopyUShort(luminanceProfileArray, buffer, bufferSize);
                    _luminanceBuf[user].AddRange(buffer);
                }
                _imageAvailable[user] = 1;
                _lastImageSizeHeight[user] = (int)count;
            }
        }

        [DllImport("kernel32.dll", SetLastError = false)]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, UIntPtr length);

        private static void CopyUShort(IntPtr source, ushort[] destination, int length)
        {
            var gch = GCHandle.Alloc(destination, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(destination, 0);
                var bytesToCopy = Marshal.SizeOf(destination[0]) * length;

                CopyMemory(ptr, source, (UIntPtr)bytesToCopy);
            }
            finally
            {
                gch.Free();
            }
        }
    }
}
