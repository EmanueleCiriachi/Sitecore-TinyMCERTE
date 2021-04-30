using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyMCERTE {
    public static class Utils {
        public static TinyEditorConfigurationResult LoadTinyEditorConfiguration() {

            TinyEditorConfigurationResult configurationResult = new TinyEditorConfigurationResult();

            using (new SecurityEnabler()) {
                //Compatibility with Sitecore builtin Telerik RTE:
                //"so" - default parameter left for RTE
                //"so_mce" - new parameter to setup profile for so_mce
                //if you need to setup profile, you need to type &so_mce=/sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Full Classic Profile
                string queryString = WebUtil.GetQueryString("so_mce", Sitecore.Configuration.Settings.GetSetting("TinyEditor.DefaultProfile"));

                Assert.IsNotNull((object)queryString, "source");

                Database coreDB = Sitecore.Data.Database.GetDatabase("core");
                Assert.IsNotNull((object)coreDB, "database");
                Sitecore.Data.Items.Item profile1 = coreDB.GetItem(queryString);

                if (profile1 != null) {
                    TinyEditorConfiguration editorConfiguration = TinyEditorConfiguration.Create(profile1);
                    configurationResult = editorConfiguration.Apply();
                } else {
                    Sitecore.Data.Items.Item profile2 = coreDB.GetItem(Settings.HtmlEditor.DefaultProfile);
                    if (profile2 != null) {
                        TinyEditorConfiguration editorConfiguration = TinyEditorConfiguration.Create(profile2);
                        configurationResult = editorConfiguration.Apply();
                    }
                }
            }

            return configurationResult;
        }
    }
}
