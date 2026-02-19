using Snap.Hutao.Remastered.Model.Metadata.Item;
using Snap.Hutao.Remastered.Model.Primitive;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Snap.Hutao.Remastered.Service.Metadata.ContextAbstraction.ImmutableDictionary;

public interface IMetadataDictionaryIdBeyondItemSource
{
    ImmutableDictionary<BeyondItemId, BeyondItem> IdBeyondItemMap { get; set; }
}
