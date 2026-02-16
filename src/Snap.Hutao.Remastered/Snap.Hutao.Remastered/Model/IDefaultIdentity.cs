// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Remastered.Model;

public interface IDefaultIdentity<out TIdentity>
{
    TIdentity Id { get; }
}