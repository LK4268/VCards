﻿using FolkerKinzel.VCards.Intls.Converters;
using FolkerKinzel.VCards.Intls.Extensions;
using FolkerKinzel.VCards.Intls.Serializers.EnumValueCollectors;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FolkerKinzel.VCards.Models.PropertyParts;


namespace FolkerKinzel.VCards.Intls.Serializers
{
    internal class ParameterSerializer3_0 : ParameterSerializer
    {
        private AddressTypesCollector? _addressTypesCollector;
        private TelTypesCollector? _telTypesCollector;
        private ImppTypesCollector? _imppTypesCollector;


        private PropertyClassTypesCollector PropertyClassTypesCollector { get; }
            = new PropertyClassTypesCollector();

        private readonly List<string> StringCollectionList = new List<string>();
        private readonly List<Action<ParameterSerializer3_0>> ActionList = new List<Action<ParameterSerializer3_0>>(2);



        private AddressTypesCollector AddressTypesCollector
        {
            get
            {
                _addressTypesCollector ??= new AddressTypesCollector();
                return _addressTypesCollector;
            }
        }


        private TelTypesCollector TelTypesCollector
        {
            get
            {
                _telTypesCollector ??= new TelTypesCollector();
                return _telTypesCollector;
            }
        }

        private ImppTypesCollector ImppTypesCollector
        {
            get
            {
                _imppTypesCollector ??= new EnumValueCollectors.ImppTypesCollector();
                return _imppTypesCollector;
            }
        }

        private readonly Action<ParameterSerializer3_0> CollectPropertyClassTypes =
        serializer =>
            serializer.PropertyClassTypesCollector.CollectValueStrings(
                serializer.ParaSection.PropertyClass, serializer.StringCollectionList);


        private readonly Action<ParameterSerializer3_0> CollectTelTypes =
        serializer =>
        {
            const TelTypes DEFINED_TELTYPES = TelTypes.Voice | TelTypes.Fax | TelTypes.Msg |
            TelTypes.Cell | TelTypes.Pager | TelTypes.BBS | TelTypes.Modem | TelTypes.Car | TelTypes.ISDN |
            TelTypes.Video | TelTypes.PCS;

            serializer.TelTypesCollector.CollectValueStrings(
                    serializer.ParaSection.TelephoneType & DEFINED_TELTYPES, serializer.StringCollectionList);
        };


        private readonly Action<ParameterSerializer3_0> CollectAddressTypes =
        serializer =>
            serializer.AddressTypesCollector.CollectValueStrings(
                serializer.ParaSection.AddressType, serializer.StringCollectionList);

        private readonly Action<ParameterSerializer3_0> CollectImppTypes =
        serializer =>
            serializer.ImppTypesCollector.CollectValueStrings(
                serializer.ParaSection.InstantMessengerType, serializer.StringCollectionList);


        private readonly Action<ParameterSerializer3_0> CollectKeyType = serializer => serializer.DoCollectKeyType();

        private readonly Action<ParameterSerializer3_0> CollectImageType = serializer => serializer.DoCollectImageType();

        private readonly Action<ParameterSerializer3_0> CollectEmailType = serializer => serializer.DoCollectEmailType();

        private readonly Action<ParameterSerializer3_0> CollectSoundType = serializer => serializer.DoCollectSoundType();

        private readonly Action<ParameterSerializer3_0> CollectMediaType = serializer => serializer.DoCollectMediaType();


        public ParameterSerializer3_0(VcfOptions options) : base(options) { }



        #region Build

        protected override void BuildAdrPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectAddressTypes);


            AppendType(isPref);
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }


        protected override void BuildAgentPara()
        {
            if (ParaSection.DataType == VCdDataType.Uri)
            {
                AppendValue(VCdDataType.Uri);
            }
        }


        protected override void BuildBdayPara()
        {
            Debug.Assert(ParaSection.DataType != VCdDataType.Text);

            AppendValue(ParaSection.DataType);
        }

        protected override void BuildCategoriesPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildClassPara()
        {
            // keine Parameter
        }

        protected override void BuildEmailPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectEmailType);

            AppendType(isPref);
        }

        protected override void BuildFnPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildGeoPara()
        {
            // keine Parameter
        }

        protected override void BuildImppPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectImppTypes);

            AppendType(isPref);
        }


        protected override void BuildKeyPara()
        {
            ActionList.Clear();
            ActionList.Add(this.CollectKeyType);

            if (ParaSection.DataType == VCdDataType.Text)
            {
                AppendValue(VCdDataType.Text);
            }
            else
            {
                AppendBase64Encoding();
            }

            AppendType(false);
        }


        protected override void BuildLabelPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectAddressTypes);


            AppendType(isPref);
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }


        protected override void BuildLogoPara()
        {
            ActionList.Clear();
            ActionList.Add(CollectImageType);


            if (ParaSection.DataType == VCdDataType.Uri)
            {
                AppendValue(VCdDataType.Uri);
            }
            else
            {
                AppendBase64Encoding();
            }

            AppendType(false);
        }


        protected override void BuildMailerPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildNPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildNamePara()
        {
            // keine Parameter
        }

        protected override void BuildNicknamePara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildNotePara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildOrgPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildPhotoPara()
        {
            ActionList.Clear();
            ActionList.Add(CollectImageType);


            if (ParaSection.DataType == VCdDataType.Uri)
            {
                AppendValue(VCdDataType.Uri);
            }
            else
            {
                AppendBase64Encoding();
            }

            AppendType(false);
        }

        protected override void BuildProdidPara()
        {
            // keine Parameter
        }

        protected override void BuildProfilePara()
        {
            // keine Parameter
        }

        protected override void BuildRevPara()
        {
            // DateTime is default
            //AppendValue(VCdDataType.DateTime);
        }

        protected override void BuildRolePara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildSortStringPara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildSoundPara()
        {
            ActionList.Clear();
            ActionList.Add(CollectSoundType);


            if (ParaSection.DataType == VCdDataType.Uri)
            {
                AppendValue(VCdDataType.Uri);
            }
            else
            {
                AppendBase64Encoding();
            }

            AppendType(false);
        }


        protected override void BuildSourcePara()
        {
            AppendValue(ParaSection.DataType);
            AppendContext();
        }


        protected override void BuildTelPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectTelTypes);

            AppendType(isPref);
        }

        protected override void BuildTitlePara()
        {
            AppendValue(ParaSection.DataType);
            AppendLanguage();
            AppendNonStandardParameters();
        }

        protected override void BuildTzPara()
        {
            // keine Parameter
        }

        protected override void BuildUidPara()
        {
            // keine Parameter
        }

        protected override void BuildUrlPara()
        {
            // keine Parameter
        }

        protected override void BuildXMessengerPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectTelTypes);

            AppendType(isPref);
        }

        protected override void BuildNonStandardPropertyPara(bool isPref)
        {
            ActionList.Clear();
            ActionList.Add(CollectPropertyClassTypes);
            ActionList.Add(CollectMediaType);

            AppendValue(ParaSection.DataType);

            if (ParaSection.Encoding == VCdEncoding.Base64)
            {
                AppendBase64Encoding();
            }

            AppendType(isPref);
            AppendLanguage();
            AppendContext();
            AppendNonStandardParameters();
        }

        #endregion

        #region Append

        private void AppendValue(VCdDataType? dataType)
        {
            const VCdDataType DEFINED_DATA_TYPES =
                VCdDataType.Uri | VCdDataType.Text | VCdDataType.Date | VCdDataType.Time | VCdDataType.DateTime |
                VCdDataType.Integer | VCdDataType.Boolean | VCdDataType.Float | VCdDataType.Binary |
                VCdDataType.PhoneNumber | VCdDataType.VCard | VCdDataType.UtcOffset;

            string? s = (dataType & DEFINED_DATA_TYPES).ToVCardString();

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.VALUE, s);
            }
        }


        private void AppendContext()
        {
            string? s = ParaSection.Context;

            if (s != null)
            {
                AppendParameter(ParameterSection.ParameterKey.CONTEXT, s);
            }
        }


        private void AppendBase64Encoding()
        {
            AppendParameter(ParameterSection.ParameterKey.ENCODING, "b");
        }


        private void AppendLanguage()
        {
            var lang = ParaSection.Language;
            if (lang != null)
            {
                AppendParameter(ParameterSection.ParameterKey.LANGUAGE, Mask(lang));
            }
        }

        private void AppendType(bool isPref)
        {
            this.StringCollectionList.Clear();

            for (int i = 0; i < this.ActionList.Count; i++)
            {
                ActionList[i](this);
            }

            if (isPref)
            {
                StringCollectionList.Add(ParameterSection.TypeValue.PREF);
            }

            if (this.StringCollectionList.Count != 0)
            {
                AppendParameter(ParameterSection.ParameterKey.TYPE, ConcatValues());
            }

            string ConcatValues()
            {
                this.Worker.Clear();
                int count = this.StringCollectionList.Count;

                Debug.Assert(count != 0);

                for (int i = 0; i < count - 1; i++)
                {
                    Worker.Append(StringCollectionList[i]).Append(',');
                }

                Worker.Append(StringCollectionList[count - 1]);
                return Worker.ToString();
            }
        }

        #endregion


        #region Collect

        private void DoCollectKeyType()
        {
            string? s = MimeTypeConverter.KeyTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                StringCollectionList.Add(Mask(s));
            }
        }


        private void DoCollectImageType()
        {
            string? s = MimeTypeConverter.ImageTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                StringCollectionList.Add(Mask(s));
            }

        }


        private void DoCollectSoundType()
        {
            string? s = MimeTypeConverter.SoundTypeValueFromMimeType(ParaSection.MediaType);

            if (s != null)
            {
                StringCollectionList.Add(Mask(s));
            }

        }


        private void DoCollectEmailType()
        {
            StringCollectionList.Add(ParaSection.EmailType ?? EmailType.SMTP);
        }


        private void DoCollectMediaType()
        {
            string? m = ParaSection.MediaType;

            if (m != null)
            {
                StringCollectionList.Add(Mask(m));
            }
        }

        #endregion


        private string Mask(string? s)
        {
            Worker.Clear().Append(s).Mask(VCdVersion.V3_0);
            //.Replace(@"\", @"\\") // Reihenfolge beachten!
            //.Replace(Environment.NewLine, @"\n")
            //.Replace(",", @"\,")
            //.Replace(";", @"\;")
            //.Replace(":", @"\:");

            return Worker.ToString();
        }

    }
}
