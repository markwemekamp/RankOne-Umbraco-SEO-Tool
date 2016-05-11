# RankOne

RankOne is a collection of SEO tools for Umbraco that aim to optimize your content according to the latest SEO standards.

RankOne currently contains 3 different property editors for Umbraco:
- Dashboard: Score based list of seo improvements
- Summary: A list of seo improvements
- Result preview: A google search result preview

## Supported versions ##
Umbraco 7.4+

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

    grunt package

Creates an Umbraco package

    grunt nuget

Creates a nuget package
