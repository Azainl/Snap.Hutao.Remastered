// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snap.Hutao.Remastered.Web.Hoyolab.Takumi.GameRecord.RoleCombat;

namespace Snap.Hutao.Remastered.Model.Entity.Configuration;

public sealed class RoleCombatEntryConfiguration : IEntityTypeConfiguration<RoleCombatEntry>
{
    public void Configure(EntityTypeBuilder<RoleCombatEntry> builder)
    {
        builder.Property(e => e.RoleCombatData)
            .HasColumnType(SqliteTypeNames.Text)
            .HasConversion<JsonTextValueConverter<RoleCombatData>>();
    }
}