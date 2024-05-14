using Narratore.Data;
using Narratore.Localization;
using UnityEngine;


namespace Narratore
{
    public class NNYDataLoader : DataLoader
    {
        public NNYDataLoader(DataLoaderConfig config,
                            bool isDebug) : base(isDebug, config)
        {
        }


        protected override IDataSource<DeviceType> SdkDeviceSource() => new GPDeviceTypeDataSource();
        protected override IDataSource<LangKey> SdkLangSource() => new GPLangDataSource();
        protected override IDataSource<int> CustomIntSource() => new GPPlayerIntDataSource(_prefsIntSource);
    }
}

