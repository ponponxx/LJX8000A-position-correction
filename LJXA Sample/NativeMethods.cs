//Copyright (c) 2020 KEYENCE CORPORATION. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace LJXASample
{
    #region enum

    /// <summary>
    /// Return value definition
    /// </summary>
    public enum Rc
    {
        /// <summary>Normal termination</summary>
        Ok = 0x0000,
        /// <summary>Failed to open the device</summary>
        ErrOpenDevice = 0x1000,
        /// <summary>Device not open</summary>
        ErrNoDevice,
        /// <summary>Command send error</summary>
        ErrSend,
        /// <summary>Response reception error</summary>
        ErrReceive,
        /// <summary>Timeout</summary>
        ErrTimeout,
        /// <summary>No free space</summary>
        ErrNomemory,
        /// <summary>Parameter error</summary>
        ErrParameter,
        /// <summary>Received header format error</summary>
        ErrRecvFmt,
        /// <summary>Not open error (for high-speed communication)</summary>
        ErrHispeedNoDevice = 0x1009,
        /// <summary>Already open error (for high-speed communication)</summary>
        ErrHispeedOpenYet,
        /// <summary>Already performing high-speed communication error (for high-speed communication)</summary>
        ErrHispeedRecvYet,
        /// <summary>Insufficient buffer size</summary>
        ErrBufferShort,
    }

    #endregion

    #region Structure

    /// <summary>
    /// Ethernet settings structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_ETHERNET_CONFIG
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] abyIpAddress;
        public ushort wPortNo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserve;
    };

    /// <summary>
    /// Profile information structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_PROFILE_INFO
    {
        public byte byProfileCount;
        public byte reserve1;
        public byte byLuminanceOutput;
        public byte reserve2;
        public short nProfileDataCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserve3;
        public int lXStart;
        public int lXPitch;
    };

    /// <summary>
    /// Profile header information structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_PROFILE_HEADER
    {
        public uint reserve;
        public uint dwTriggerCount;
        public int lEncoderCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] reserve2;
    };

    /// <summary>
    /// Profile footer information structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_PROFILE_FOOTER
    {
        public uint reserve;
    };

    /// <summary>
    /// Get profile response structure (batch measurement: off)
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_GET_PROFILE_RESPONSE
    {
        public uint dwCurrentProfileNo;
        public uint dwOldestProfileNo;
        public uint dwGetTopProfileNo;
        public byte byGetProfileCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] reserve;
    };

    /// <summary>
    /// High-speed communication start preparation request structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LJX8IF_HIGH_SPEED_PRE_START_REQUEST
    {
        public byte bySendPosition;     // Send start position
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] reserve;      // Reservation 
    };

    #endregion

    #region Method
    /// <summary>
    /// Callback function for high-speed communication simple array
    /// </summary>
    /// <param name="pProfileHeaderArray">Received header data array pointer</param>
    /// <param name="pHeightProfileArray">Received profile data array pointer</param>
    /// <param name="pLuminanceProfileArray">Received luminance profile data array pointer</param>
    /// <param name="dwLuminanceEnable">The value indicating whether luminance data output is enable or not</param>
    /// <param name="dwProfileDataCount">The data count of one profile</param>
    /// <param name="dwCount">Number of profiles</param>
    /// <param name="dwNotify">Finalization condition</param>
    /// <param name="dwUser">Thread ID</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void HighSpeedDataCallBackForSimpleArray(IntPtr pProfileHeaderArray, IntPtr pHeightProfileArray, IntPtr pLuminanceProfileArray, uint dwLuminanceEnable, uint dwProfileDataCount, uint dwCount, uint dwNotify, uint dwUser);

    /// <summary>
    /// Function definitions
    /// </summary>
    internal class NativeMethods
    {
        /// <summary>
        /// Number of connectable devices
        /// </summary>
        internal static int DeviceCount
        {
            get { return 6; }
        }

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_Initialize();

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_Finalize();

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_EthernetOpen(int lDeviceId, ref LJX8IF_ETHERNET_CONFIG pEthernetConfig);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_CommunicationClose(int lDeviceId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_StartMeasure(int lDeviceId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(
        int lDeviceId, ref LJX8IF_ETHERNET_CONFIG pEthernetConfig, ushort wHighSpeedPortNo,
        HighSpeedDataCallBackForSimpleArray pCallBackSimpleArray, uint dwProfileCount, uint dwThreadId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_PreStartHighSpeedDataCommunication(
        int lDeviceId, ref LJX8IF_HIGH_SPEED_PRE_START_REQUEST pReq,
        ref LJX8IF_PROFILE_INFO pProfileInfo);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_StartHighSpeedDataCommunication(int lDeviceId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_StopHighSpeedDataCommunication(int lDeviceId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_FinalizeHighSpeedDataCommunication(int lDeviceId);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_GetZUnitSimpleArray(int lDeviceId, ref ushort pwZUnit);

        [DllImport("LJX8_IF.dll")]
        internal static extern int LJX8IF_CircularImage(long lXPointNum, long lProfileNum,
        float fCircleCenterUm, float fThetaPitchDeg, float fTiltCorrectDeg, float fXPitchUm, float fZUnit,
        byte byImageType, long lDestXPointNum, long lDestYPointNumP, long lDestYPointNumM, IntPtr pSourceImage, IntPtr pDestImage);

        [DllImport("LJX8_IF.dll")]
        internal static extern long LJX8IF_CombineImage(byte pPixel,long lXNumX, long lNumY, long ISizeX,long ISizeY,
            IntPtr plOpSort,IntPtr pbOpInvX,IntPtr pbOpInvY,IntPtr plOpCutX,IntPtr plOpGeoX,IntPtr plOpGeoY,
        IntPtr pSourceImage, IntPtr pDstImage);

    }
    #endregion
}
