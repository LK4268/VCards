﻿using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Diagnostics;
using System.Text;

namespace FolkerKinzel.VCards.Intls.Extensions
{
    static class StringBuilderExtension
    {
        internal const string NEWLINE_REPLACEMENT = @"\n";

        internal static StringBuilder UnMask(this StringBuilder builder, VCdVersion version)
        {
            Debug.Assert(builder != null);

            if (version == VCdVersion.V2_1)
            {
                builder.Replace(@"\;", ";");
                return builder;
            }

            if (version >= VCdVersion.V4_0)
            {
                builder
                    .Replace(@"\\", @"\");
            }


            builder
                .Replace(@"\n", Environment.NewLine)
                .Replace(@"\N", Environment.NewLine)
                .Replace(@"\,", ",")
                .Replace(@"\;", ";")
                //.Replace(@"\:", ":")
                ;

            return builder;
        }

        //internal static StringBuilder AppendJoinedStrings(this StringBuilder builder, char joinChar, IList<string> strings)
        //{
        //    Debug.Assert(builder != null);
        //    Debug.Assert(strings != null);

        //    for (int i = 0; i < strings.Count - 1; i++)
        //    {
        //        builder.Append(strings[i]).Append(joinChar);
        //    }

        //    builder.Append(strings[strings.Count - 1]);

        //    return builder;
        //}


        internal static StringBuilder Mask(this StringBuilder builder, VCdVersion version)
        {
            Debug.Assert(builder != null);

            if (version == VCdVersion.V2_1)
            {
                builder.Replace(";", @"\;");
                return builder;
            }

            if (version >= VCdVersion.V4_0)
            {
                builder
                    .Replace(@"\", @"\\");
            }


            builder
                .Replace(Environment.NewLine, NEWLINE_REPLACEMENT)
                .Replace(",", @"\,")
                .Replace(";", @"\;");


            //if (version == VCdVersion.V3_0 && options.IsSet(VcfOptions.MaskColonInVcard_3_0))
            //{
            //    builder.Replace(":", @"\:");
            //}

            return builder;
        }


        internal static StringBuilder MaskNewLine(this StringBuilder builder)
        {
            builder
                    .Replace(@"\", @"\\")
                    .Replace(Environment.NewLine, NEWLINE_REPLACEMENT);

            return builder;
        }


        internal static StringBuilder Trim(this StringBuilder builder)
        {
            Debug.Assert(builder != null);

            while (builder.Length >= 1 && char.IsWhiteSpace(builder[0]))
            {
                builder.Remove(0, 1);
            }

            while (builder.Length >= 1 && char.IsWhiteSpace(builder[builder.Length - 1]))
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }


        internal static StringBuilder RemoveQuotes(this StringBuilder builder)
        {
            Debug.Assert(builder != null);

            while (builder.Length >= 1 && (builder[0] == '\"' || builder[0] == '\''))
            {
                builder.Remove(0, 1);
            }

            while (builder.Length >= 1 && (builder[builder.Length - 1] == '\"' || builder[builder.Length - 1] == '\''))
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }


        internal static StringBuilder ToLowerInvariant(this StringBuilder builder)
        {
            Debug.Assert(builder != null);

            for (int i = 0; i < builder.Length; i++)
            {
                builder[i] = char.ToLowerInvariant(builder[i]);
            }

            return builder;
        }
    }
}
