// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Snap.Hutao.Remastered.API.Model.Plugin;

namespace Snap.Hutao.Remastered.UI.Xaml.Data.Converter;

public sealed partial class ValueTypeVisibilityConverter : IValueConverter
{
    public string TargetType { get; set; } = string.Empty;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is EditableSettingItem settingItem)
        {
            Type actualType = settingItem.ValueType;
            string actualTypeName = actualType?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(TargetType))
            {
                return (actualType != typeof(string) &&
                       actualType != typeof(int) &&
                       actualType != typeof(bool)) 
                       ? Visibility.Visible : Visibility.Collapsed;
            }

            return actualTypeName == TargetType ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
