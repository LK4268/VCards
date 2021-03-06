﻿using FolkerKinzel.VCards.Intls;
using FolkerKinzel.VCards.Intls.Attributes;
using FolkerKinzel.VCards.Intls.Deserializers;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace FolkerKinzel.VCards.Models
{
    /// <summary>
    /// Repräsentiert vCard-Properties, die eine Sammlung von Text-Inhalten speichern.
    /// </summary>
    public class StringCollectionProperty : VCardProperty<ReadOnlyCollection<string>>, IVCardData, IVcfSerializable, IVcfSerializableData
    {
        /// <summary>
        /// Initialisiert ein <see cref="StringCollectionProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Eine Sammlung von <see cref="string"/>s.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public StringCollectionProperty(IEnumerable<string?>? value, string? propertyGroup = null)
        {
            Group = propertyGroup;

            if (value == null) return;

            // Die Überprüfungen könnten eine allgemeine Verwendbarkeit der Klasse einschränken:
            var arr = value.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Trim()).ToArray();

            if (arr.Length == 0) return;

            this.Value = new ReadOnlyCollection<string>(arr);
        }

        /// <summary>
        /// Initialisiert ein <see cref="StringCollectionProperty"/>-Objekt.
        /// </summary>
        /// <param name="value">Ein <see cref="string"/> oder <c>null</c>.</param>
        /// <param name="propertyGroup">(optional) Bezeichner der Gruppe von Properties, der die Property zugehören soll.</param>
        public StringCollectionProperty(string? value, string? propertyGroup = null) :
            this(value is null ? null : new string?[] { value }, propertyGroup)
        { }


        internal StringCollectionProperty(VcfRow vcfRow, VCardDeserializationInfo info, VCdVersion version)
            : base(vcfRow.Parameters, vcfRow.Group)
        {
            vcfRow.DecodeQuotedPrintable();
            var arr = SplitValue();

            if (arr.Length == 0) return;

            this.Value = new ReadOnlyCollection<string>(arr);




            //////////////////////////////////////////////////////

            string[] SplitValue()
            {
                string? value = vcfRow.Value;
                var builder = info.Builder;
                var list = value.SplitValueString(',', StringSplitOptions.RemoveEmptyEntries);


                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = UnescapeString(list[i]);
                }

                return list.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray();


                string UnescapeString(string val)
                {
                    builder.Clear();
                    builder.Append(val);
                    builder.UnMask(version);
                    return builder.ToString();
                }
            }
        }

        [InternalProtected]
        internal override void AppendValue(VcfSerializer serializer)
        {
            InternalProtectedAttribute.Run();
            Debug.Assert(serializer != null);

            if (Value is null) return;

            Debug.Assert(Value.Count != 0);

            var worker = serializer.Worker;
            var builder = serializer.Builder;
            string s;

            for (int i = 0; i < Value.Count - 1; i++)
            {
                s = Value[i];

                Debug.Assert(!string.IsNullOrEmpty(s));

                worker.Clear().Append(s).Mask(serializer.Version);
                builder.Append(worker).Append(',');
            }

            s = Value[Value.Count - 1];

            Debug.Assert(!string.IsNullOrEmpty(s));

            worker.Clear().Append(s).Mask(serializer.Version);
            builder.Append(worker);
        }


        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="StringCollectionProperty"/>-Objekts. (Nur zum 
        /// Debuggen.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="StringCollectionProperty"/>-Objekts.</returns>
        public override string ToString()
        {
            string s = "";

            if (Value is null) return s;

            Debug.Assert(Value.Count != 0);

            for (int i = 0; i < Value.Count - 1; i++)
            {
                s += Value[i];
                s += ", ";
            }

            s += Value[Value.Count - 1];

            return s;
        }

    }
}
