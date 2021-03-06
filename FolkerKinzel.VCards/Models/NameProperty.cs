﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Encodings.QuotedPrintable;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die vCard-Property N, die den Namen des vCard-Subjekts speichert.
    /// </summary>
    public sealed class NameProperty : VCardProperty<Name>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="NameProperty"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public NameProperty(
            IEnumerable<string?>? lastName = null,
            IEnumerable<string?>? firstName = null,
            IEnumerable<string?>? middleName = null,
            IEnumerable<string?>? prefix = null,
            IEnumerable<string?>? suffix = null,
            string? propertyGroup = null)
        {
            Value = new Name(lastName, firstName, middleName, prefix, suffix);
            Group = propertyGroup;
        }


        /// <summary>
        /// Initialisiert ein neues <see cref="NameProperty"/>-Objekt.
        /// </summary>
        /// <param name="lastName">Nachname</param>
        /// <param name="firstName">Vorname</param>
        /// <param name="middleName">zweiter Vorname</param>
        /// <param name="prefix">Namenspräfix (z.B. "Prof. Dr.")</param>
        /// <param name="suffix">Namenssuffix (z.B. "jr.")</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe, der die Property zugehören soll.</param>
        public NameProperty(
            string? lastName,
            string? firstName = null,
            string? middleName = null,
            string? prefix = null,
            string? suffix = null,
            string? propertyGroup = null)
        {
            Value = new Name(
                lastName is null ? null : new string?[] { lastName },
                firstName is null ? null : new string?[] { firstName },
                middleName is null ? null : new string?[] { middleName },
                prefix is null ? null : new string?[] { prefix },
                suffix is null ? null : new string?[] { suffix });
            Group = propertyGroup;
        }


        internal NameProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            if (vcfRow.Value == null)
            {
                Value = new Name();
            }
            else
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(vcfRow.Value));

                vcfRow.DecodeQuotedPrintable();

                Value = new Name(vcfRow.Value, info.Builder, version);
            }
        }


        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            base.PrepareForVcfSerialization(serializer);

            Debug.Assert(serializer != null);
            Debug.Assert(Value != null); // value ist nie null

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
            Debug.Assert(Value != null); // value ist nie null

            var builder = serializer.Builder;
            int valueStartIndex = builder.Length;


            Value.AppendVCardString(serializer);

            if (Parameters.Encoding == VCdEncoding.QuotedPrintable)
            {
                string toEncode = builder.ToString(valueStartIndex, builder.Length - valueStartIndex);
                builder.Length = valueStartIndex;

                builder.Append(QuotedPrintableConverter.Encode(toEncode, valueStartIndex));
            }
        }

        /// <summary>
        /// True, wenn das <see cref="NameProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => Value.IsEmpty;

    }
}
