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





