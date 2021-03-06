﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Kapselt die Daten der vCard-Property CLASS, die in vCard 3.0 die Geheimhaltungsstufe der 
    /// vCard definiert.
    /// </summary>
    public sealed class AccessProperty : VCardProperty<VCdAccess>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="AccessProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein Member der <see cref="VCdAccess"/>-Enumeration.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public AccessProperty(VCdAccess value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
        }

        internal AccessProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = VCdAccessConverter.Parse(vcfRow.Value);
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.Append(Value.ToVCardString());
        }


        /// <summary>
        /// True, wenn das <see cref="AccessProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => false;
    }
}
