# RankOne

# Doc Type Grid Editor

[![NuGet release](https://img.shields.io/nuget/v/RankOne.svg)](https://www.nuget.org/packages/RankOne)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/backoffice-extensions/rankone-seo-toolkit)

RankOne is a collection of SEO tools for Umbraco that aim to optimize your content according to the latest SEO standards.

RankOne currently contains 5 different tools for Umbraco:
- A dashboard which will allow you to view the seo score of all pages in your Umbraco installation
- A doctype composition with title, meta description and meta keywords with a corresponding partial view
- Dashboard property editor: Score based list of seo improvements
- Summary property editor: A list of seo improvements
- Result preview property editor: A google search result preview

![](https://www.github.com/markwemekamp/RankOne-Umbraco-SEO-Tool/raw/master/docs/assets/img/dashboard.png)


## How to install the fields composition ##

1. Add or update a doctype
2. On the top right select "Compositions"
3. Select RankOne - Seo Compositions
4. A new tab will be added added to your doctype
5. Now the only thing you'll need to do is output the properties in your template. For this you can use the partial view "RankOne - Seo Properties", which is included in the  package or you can use your own implementation. The alias names are: seoTitle, seoMetaDescription and seoMetaKeywords.

## How to install the dashboard ##

1. Open Dashboard.config with a text editor, this file is located in the Config directory
2. After the <code>&lt;dashBoard&gt;</code> tag, add the following xml

```xml
    <section alias="RankOneSEODashboardSection">
      <areas>
        <area>content</area>
      </areas>
      <tab caption="RankOne - SEO">
        <control showOnce="true" addPanel="true" panelCaption="">
          /App_Plugins/RankOne/dashboards/SiteDashboard.html
        </control>
      </tab>
    </section>
```

## How to install property editors ##

1. Create a package using the grunt package task
2. Install the package
3. Create a doctype or composition
4. Create a new tab
5. Create a new property with the RankOne - Dashboard datatype
6. Save it all and you're ready to go

## How to add a focus keyword ##

1. In the content tree click the node(s) where the dashboard is installed on
2. Go to the seo tab
3. On the right hand corner click on the cog
4. Fill in your focus keyword and click save
4. Click save and publish on the node to save the keyword and reload the dashboard

## Supported versions ##
Umbraco 7.4+

## Changelog ##

1.2 Added seo doctype composition, removed headinganalyzer, minor bug and localization fixes

### Install Dependencies ###
*Requires Node.js to be installed and in your system path*

    npm install -g grunt-cli && npm install -g grunt
    npm install

### Build ###
    grunt

   Builds the project to `/dist/`.  These files can be dropped into an Umbraco 7 site, or you can build directly to a site using:

    grunt --target="C:\inetpub\umbraco"

You can also watch for changes using:

    grunt watch
    grunt watch --target="C:\inetpub\umbraco"


Add `--touch` to either command to automatically touch the web.config on a deploy

Create an Umbraco package

    grunt package

Create a nuget package

    grunt nuget
