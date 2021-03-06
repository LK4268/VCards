﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert vCard-Properties, deren Inhalt aus Text besteht.
    /// </summary>
    public class TextProperty : VCardProperty<string?>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="TextProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/>.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public TextProperty(string? value, string? propertyGroup = null)
        {
            Value = string.IsNullOrWhiteSpace(value) ? null : value;
            Group = propertyGroup;
        }

        internal TextProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version) : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.DecodeQuotedPrintable();

            if (version != VCdVersion.V2_1)
            {
                vcfRow.UnMask(info, version);
            }

            Value = vcfRow.Value;
        }

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            base.PrepareForVcfSerialization(serializer);

            if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded())
            {
                this.Parameters.Encoding = VCdEncoding.QuotedPrintable;
                this.Parameters.Charset = VCard.DEFAULT_CHARSET;
            }
        }

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            var builder = serializer.Builder;


            if (serializer.Version == VCdVersion.V2_1)
            {
                if (this.Parameters.Encoding == VCdEncoding.QuotedPrintable)
                {
                    builder.Append(QuotedPrintableConverter.Encode(Value, builder.Length));
                }
                else
                {
                    builder.Append(Value);
                }
            }
            else
            {
                var worker = serializer.Worker;

                worker.Clear().Append(Value).Mask(serializer.Version);
                builder.Append(worker);
            }



        }
    }
}
