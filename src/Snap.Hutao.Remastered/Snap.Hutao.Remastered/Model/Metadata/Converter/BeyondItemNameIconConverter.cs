using Snap.Hutao.Remastered.Core.ExceptionService;
using Snap.Hutao.Remastered.Model.Intrinsic;
using Snap.Hutao.Remastered.Web.Endpoint.Hutao;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snap.Hutao.Remastered.Model.Metadata.Converter;

public static class BeyondItemNameIconConverter
{

    public static Uri IconNameToUri(string name)
    {
        return StaticResourcesEndpoints.StaticRaw("BeyondItemIcon", $"{name}.png").ToUri();
    }
}
