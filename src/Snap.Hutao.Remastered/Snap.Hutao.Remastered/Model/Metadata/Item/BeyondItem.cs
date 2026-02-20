using Snap.Hutao.Remastered.Model.Intrinsic;
using Snap.Hutao.Remastered.Model.Metadata.Abstraction;
using Snap.Hutao.Remastered.Model.Metadata.Converter;
using Snap.Hutao.Remastered.Model.Primitive;
using Snap.Hutao.Remastered.ViewModel.GachaLog;

namespace Snap.Hutao.Remastered.Model.Metadata.Item;

public partial class BeyondItem : 
    IDefaultIdentity<BeyondItemId>,
    ISummaryItemConvertible,
    IStatisticsItemConvertible
{
    public required BeyondItemId Id { get; init; }

    public required QualityType RankLevel { get; init; }

    public required string Icon { get; init; }

    public required string Name { get; set; }

    [JsonIgnore]
    public QualityType Quality { get => RankLevel; }

    public SummaryItem ToSummaryItem(int lastPull, in DateTimeOffset time, bool isUp)
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Icon = BeyondItemNameIconConverter.IconNameToUri(Icon),
            Badge = BeyondItemNameIconConverter.IconNameToUri(Icon),
            Time = time,
            Quality = RankLevel,
            LastPull = lastPull,
            IsUp = isUp,
        };
    }

    public StatisticsItem ToStatisticsItem(int count)
    {
        return new()
        {
            Id = Id,
            Name = Name,
            Icon = BeyondItemNameIconConverter.IconNameToUri(Icon),
            Badge = BeyondItemNameIconConverter.IconNameToUri(Icon),
            Quality = RankLevel,
            Count = count,
        };
    }
}
