# RankOne

RankOne is a SEO analyzation tool that helps your content editors optimize their content according to the latest SEO standards.

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