# Sitecore-TinyMCERTE

[![Build status](https://ci.appveyor.com/api/projects/status/mj3633bo4rwfdgts?svg=true)](https://ci.appveyor.com/project/Antonytm/sitecore-tinymcerte)
[![](https://sonarcloud.io/api/project_badges/measure?project=TinyMCERTE&metric=coverage)](https://sonarcloud.io/component_measures?id=TinyMCERTE&metric=coverage)
[![](https://sonarcloud.io/api/project_badges/measure?project=TinyMCERTE&metric=code_smells)](https://sonarcloud.io/component_measures?id=TinyMCERTE&metric=code_smells) 
[![](https://sonarcloud.io/api/project_badges/measure?project=TinyMCERTE&metric=bugs)](https://sonarcloud.io/component_measures?id=TinyMCERTE&metric=bugs)
[![](https://sonarcloud.io/api/project_badges/measure?project=TinyMCERTE&metric=vulnerabilities)](https://sonarcloud.io/project/issues?id=TinyMCERTE&resolved=false&types=VULNERABILITY)
![GitHub top language](https://img.shields.io/github/languages/top/antonytm/sitecore-tinymcerte)

[![](https://sonarcloud.io/api/project_badges/quality_gate?project=TinyMCERTE)](https://sonarcloud.io/dashboard/index/TinyMCERTE)

## Description

This is a Sitecore Control that uses the Tiny MCE Editor in place of the default one

## How to install

1. Download Sitecore update package: a) from [Github releases](https://github.com/Antonytm/Sitecore-TinyMCERTE/releases) if you want stable version; b) from [AppVeyor](https://ci.appveyor.com/project/Antonytm/sitecore-tinymcerte) if you want latest version
2. Install it using update installation wizard /sitecore/admin/UpdateInstallationWizard.aspx
3. Set new field types on tempates that you want

## How to use

Module introduces 2 new field types: `TinyMCERTE` and `Rich Text with TinyMCERTE`. 
`TinyMCERTE` allows to edit field only with *TinyMCE* editor. `Rich Text with TinyMCERTE` remains compatibility with `Rich Text` field. You will have same abilities to edit *Rich Text* field, but additionaly your will get ability to run *TinyMCE* editor.

`TinyMCERTE` and `Rich Text with TinyMCERTE` fields are compatible with `Rich Text` field. All these fields use *HTML* format to save data. If you are not happy with editing abilities of any of this field types, you can change field type without losing data in it.

## Configuration

All information below relates to both `TinyMCERTE` and `Rich Text with TinyMCERTE` fields. When configuration for thsese fields differs, it is described separately.

### Editing profile

[Similar to classic](https://doc.sitecore.com/SdnArchive/Reference/Sitecore%205,-d-,3/Field%20Reference/Standard%20Data%20Types/Rich%20Text.html) `Rich Text` field, `TinyMCERTE` and `Rich Text with TinyMCERTE` fields has ability to set editing profiles.

You need to set `Source` field value on the template to do it. Format of setting value is: {1}&so_mce={2}. Where:
 * {1} - HTML Editor Profile for *Rich Text* controls. (If applied, because `TinyMCERTE` doesn't use it)
 * {2} - HTML Editor Profile for *TinyMCE* controls.

All available *TinyMCE* editing profiles are located in *core* database under path `/sitecore/system/Settings/TinyMCE Editor Profiles`. Available values provided with intallation package:
 * /sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Basic Profile
 * /sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Default Profile 
 * /sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Full Classic Profile
 * /sitecore/system/Settings/TinyMCE Editor Profiles/TinyMCE Standard Common Profile

You can create your own editor profiles or configure existing ones depending of your needs. Each profile has next abilities for configuration:
 * *Editor Toolbar* - provides ability to configure buttons that should be [available on the toolbar](https://www.tiny.cloud/docs/advanced/available-toolbar-buttons/). 
 * *Editor plugins* - provides ability to configure usage of different [availabe TinyMCE plugins](https://www.tiny.cloud/docs/plugins/opensource/anchor/)
 * *Editor Init Callback* - allows to [run your custom JavaScript](https://www.tiny.cloud/docs-3x/reference/configuration/Configuration3x@init_instance_callback/) on the start of TinyMCE editor. It is required for ability to use some plugins or extend editor by yourself.
 * *Editor Menubar* - controls [enabling/disabling of menu bar](https://www.tiny.cloud/docs/configure/editor-appearance/#exampledisablingremovingthemenubar).
 * *Editor Branding* - [turns on and off link "Powered by Tiny"](https://www.tiny.cloud/docs/configure/editor-appearance/#branding) displayed in the status bar. 
 * *Editor Style Formats* - [enables you to add more advanced style formats](https://www.tiny.cloud/docs-3x/reference/configuration/Configuration3x@style_formats/) for text and other elements.

## Local development setup

1. Clone repository
1. Open command line
1. Change dir to ".\Sitecore-TinyMCERTE\sitecore\shell\Controls\Lib"
1. Run `npm install`. It will install TinyMCE npm package and all dependencies.
1. Install and configure Unicorn on your Sitecore instance
1. Edit `Sitecore-TinyMCERTE\App_Config\Include\TinyEditor.Serialization.config` file. Change `physicalRootPath` to correspond your local location of source folder
1. Use `FolderProfile` publishing profile to publish your project (Create your own profile based on this one or edit existing ones)
1. Sync TinyMCERTE Unicorn configuration after publishing of project




