﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert die in vCard 4.0 eingeführte Property KIND, die die Art des Objekts beschreibt, das durch die vCard repräsentiert wird.
    /// </summary>
    public sealed class KindProperty : VCardProperty<VCdKind>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein neues <see cref="KindProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein Member der <see cref="VCdKind"/>-Enumeration.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public KindProperty(VCdKind value, string? propertyGroup = null)
        {
            Value = value;
            Group = propertyGroup;
        }

        internal KindProperty(VcfRow vcfRow) : base(vcfRow.Parameters, vcfRow.Group)
        {
            Value = VCdKindConverter.Parse(vcfRow.Value);
        }


        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            serializer.Builder.Append(Value.ToVCardString());
        }


        /// <summary>
        /// True, wenn das <see cref="KindProperty"/>-Objekt keine Daten enthält.
        /// </summary>
        public override bool IsEmpty => false;
    }
}
