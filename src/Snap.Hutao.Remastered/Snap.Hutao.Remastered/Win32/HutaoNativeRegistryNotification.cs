// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Win32.Foundation;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace Snap.Hutao.Remastered.Win32;

public sealed unsafe class HutaoNativeRegistryNotification
{
    private readonly ObjectReference<Vftbl> objRef;

    public HutaoNativeRegistryNotification(ObjectReference<Vftbl> objRef)
    {
        this.objRef = objRef;
    }

    public void Start(HutaoNativeRegistryNotificationCallback callback, nint userData)
    {
        Marshal.ThrowExceptionForHR(objRef.Vftbl.Start(objRef.ThisPtr, callback, userData));
    }

    [Guid(HutaoNativeMethods.IID_IHutaoNativeRegistryNotification)]
    public readonly struct Vftbl
    {
#pragma warning disable CS0649
        public readonly IUnknownVftbl IUnknownVftbl;
        public readonly delegate* unmanaged[Stdcall]<nint, HutaoNativeRegistryNotificationCallback, nint, HRESULT> Start;
#pragma warning restore CS0649
    }
}