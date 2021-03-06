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
    /// Repräsentiert die vCard-Property ORG, die Informationen über die Organisation speichert, der das vCard-Objekt zugeordnet ist.
    /// </summary>
    public sealed class OrganizationProperty : VCardProperty<Organization>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="OrganizationProperty"/>-Objekt.
        /// </summary>
        /// <param name="organizationName">Name der Organisation.</param>
        /// <param name="organizationalUnits">Namen der Unterorganisationen.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public OrganizationProperty(string? organizationName, IEnumerable<string?>? organizationalUnits = null, string? propertyGroup = null)
        {
            Value = new Organization(organizationName, organizationalUnits);
            Group = propertyGroup;
        }

        internal OrganizationProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            Debug.Assert(vcfRow != null);

            vcfRow.DecodeQuotedPrintable();

            Value = new Organization(vcfRow.Value, info.Builder, version);
        }

        [InternalProtected]
        internal override void PrepareForVcfSerialization(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();

            base.PrepareForVcfSerialization(serializer);

            Debug.Assert(serializer != null);
            Debug.Assert(Value != null); // value ist nie null

            if (serializer.Version == VCdVersion.V2_1 && Value.NeedsToBeQpEncoded)
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
        /// True, wenn das <see cref="OrganizationProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => Value.IsEmpty; // Value ist nie null

    }
}
