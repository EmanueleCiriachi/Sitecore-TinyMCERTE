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

If you are not installing the full Sitecore Package available at http://emanueleciriachi.net/tinymce-sitecore-usage-configuration/, the following instructions must be followed:


-download and install the "TinyMCE for Sitecore - Core Items-1.0.zip" package; this will install the required items in the Code database.

-copy to your web project the folder "\sitecore\shell\Controls\Lib\tinymce"

-copy to your web project the folder "\sitecore\shell\Controls\TinyMCE Editor"

-copy to your web project the folder "\App_Config\Include\TinyEditor.config"


and of course, deploy the TinyMCERTE.dll file.
