// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Model.Intrinsic;

namespace Snap.Hutao.Remastered.Model.Metadata.Abstraction;

public interface INameQualityAccess : INameAccess
{
    QualityType Quality { get; }
}