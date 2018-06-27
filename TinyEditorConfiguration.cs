using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Reflection;
using Sitecore.Resources;
using Sitecore.Security.Authentication;
using Sitecore.Shell.Controls.RichTextEditor;
using Sitecore.Web.UI;
using Sitecore.Xml;
using System;
using System.Globalization;
using System.Xml;

namespace TinyMCERTE {
    public class TinyEditorConfiguration {
        /// <summary>The profile.</summary>
        private readonly Sitecore.Data.Items.Item profile;
        /// <summary>The language.</summary>
        private string language;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Controls.RichTextEditor.TinyEditorConfiguration" /> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public TinyEditorConfiguration(Sitecore.Data.Items.Item profile)
        {
          Assert.ArgumentNotNull((object) profile, ExtensionMethods.nameof(() => profile));
          this.profile = profile;
          this.Result = new TinyEditorConfigurationResult();
        }

        /// <summary>Gets the profile root.</summary>
        /// <value>The profile root.</value>
        public Sitecore.Data.Items.Item Profile
        {
          get
          {
            return this.profile;
          }
        }

        /// <summary>Gets the result.</summary>
        /// <value>The result.</value>
        protected TinyEditorConfigurationResult Result { get; private set; }

        /// <summary>Setups the Rich Text Editor.</summary>
        /// <param name="editor">The editor.</param>
        /// <returns>The editor configuration result.</returns>
        public TinyEditorConfigurationResult Apply() {
            if (Profile != null) {
                if (Profile.Fields["Editor Toolbar"] != null) {
                    Result.EditorToolbar = Profile.Fields["Editor Toolbar"].Value;
                }
                if (Profile.Fields["Editor plugins"] != null) {
                    Result.EditorPlugins = Profile.Fields["Editor plugins"].Value;
                }
                if (Profile.Fields["Editor Init Callback"] != null) {
                    Result.EditorInitCallback = Profile.Fields["Editor Init Callback"].Value;
                }
                if (Profile.Fields["Editor Menubar"] != null) {
                    Result.EditorMenubar = Profile.Fields["Editor Menubar"].Value;
                }
                if (Profile.Fields["Editor Branding"] != null) {
                    Result.EditorBranding = Profile.Fields["Editor Branding"].Value;
                }
            }

            return this.Result;
        }

        /// <summary>Creates the specified profile.</summary>
        /// <param name="profile">The profile.</param>
        /// <returns>The editor configuration.</returns>
        public static TinyEditorConfiguration Create(Sitecore.Data.Items.Item profile) {
            Assert.ArgumentNotNull((object)profile, ExtensionMethods.nameof(() => profile));

            string configurationType = Sitecore.Configuration.Settings.GetSetting("TinyEditor.DefaultConfigurationType");
            TinyEditorConfiguration editorConfiguration = ReflectionUtil.CreateObject(configurationType, new object[1]
              {
                (object) profile
              }) as TinyEditorConfiguration;

            Assert.IsNotNull((object)editorConfiguration, "editor configuration");
            return editorConfiguration;
        }
    }
}
