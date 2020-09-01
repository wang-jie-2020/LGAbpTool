using System;
using AbpDtoGenerator.Models;

namespace AbpDtoGenerator.GeneratorModels
{
    public class ViewModel
    {
        public SPAModel SPAClient { get; set; }

        public ServerModel Server { get; set; }

        public BasicOptionCfg MainWindowsOptionCfg { get; set; }
    }
}