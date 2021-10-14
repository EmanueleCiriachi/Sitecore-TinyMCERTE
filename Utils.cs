using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Web;

namespace TinyMCERTE {
    public static class Utils {
        public static TinyEditorConfigurationResult LoadTinyEditorConfiguration() {

            var configurationResult = new TinyEditorConfigurationResult();

            using (new SecurityEnabler()) {
                //Compatibility with Sitecore builtin Telerik RTE:
                //"so" - default parameter left for RTE
                //"so_mce" - new parameter to setup profile for so_mce
                //if you need to setup profile, you need to type &so_mce=/sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Full Classic Profile
                var queryString = WebUtil.GetQueryString("so_mce", Sitecore.Configuration.Settings.GetSetting("TinyEditor.DefaultProfile"));

                Assert.IsNotNull((object)queryString, "source");

                var coreDb = Sitecore.Data.Database.GetDatabase("core");
                Assert.IsNotNull((object)coreDb, "database");
                var profile1 = coreDb.GetItem(queryString);

                if (profile1 != null) {
                    var editorConfiguration = TinyEditorConfiguration.Create(profile1);
                    configurationResult = editorConfiguration.Apply();
                }
            }

            return configurationResult;
        }
    }
}
