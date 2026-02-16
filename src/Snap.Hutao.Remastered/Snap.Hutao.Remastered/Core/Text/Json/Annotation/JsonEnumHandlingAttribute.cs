// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Hutao.Remastered.Core.Text.Json.Converter;
using System.Text.Json.Serialization.Metadata;

namespace Snap.Hutao.Remastered.Core.Text.Json.Annotation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class JsonEnumHandlingAttribute : Attribute
{
    private static readonly Type UnsafeEnumConverterType = typeof(UnsafeEnumConverter<>);

    private readonly JsonEnumHandling readAs;
    private readonly JsonEnumHandling writeAs;

    public JsonEnumHandlingAttribute(JsonEnumHandling readAndWriteAs)
    {
        readAs = readAndWriteAs;
        writeAs = readAndWriteAs;
    }

    public JsonEnumHandlingAttribute(JsonEnumHandling readAs, JsonEnumHandling writeAs)
    {
        this.readAs = readAs;
        this.writeAs = writeAs;
    }

    public JsonConverter CreateConverter(JsonPropertyInfo info)
    {
        Type converterType = UnsafeEnumConverterType.MakeGenericType(info.PropertyType);
        JsonConverter? converter = Activator.CreateInstance(converterType, readAs, writeAs) as JsonConverter;
        ArgumentNullException.ThrowIfNull(converter);
        return converter;
    }
}