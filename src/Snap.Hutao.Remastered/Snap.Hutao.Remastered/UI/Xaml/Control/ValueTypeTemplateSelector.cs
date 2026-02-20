// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Snap.Hutao.Remastered.API.Model.Plugin;

namespace Snap.Hutao.Remastered.UI.Xaml.Control;

public sealed partial class ValueTypeTemplateSelector : DataTemplateSelector
{
    public DataTemplate? StringTemplate { get; set; }

    public DataTemplate? Int32Template { get; set; }

    public DataTemplate? BooleanTemplate { get; set; }

    public DataTemplate? DefaultTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is EditableSettingItem settingItem)
        {
            Type valueType = settingItem.ValueType;

            if (valueType == typeof(string))
            {
                return StringTemplate ?? DefaultTemplate ?? base.SelectTemplateCore(item, container);
            }
            else if (valueType == typeof(int))
            {
                return Int32Template ?? DefaultTemplate ?? base.SelectTemplateCore(item, container);
            }
            else if (valueType == typeof(bool))
            {
                return BooleanTemplate ?? DefaultTemplate ?? base.SelectTemplateCore(item, container);
            }
        }

        return DefaultTemplate ?? base.SelectTemplateCore(item, container);
    }
}
