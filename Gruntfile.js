module.exports = function(grunt) {
    require('load-grunt-tasks')(grunt);
    var path = require('path');

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        pkgMeta: grunt.file.readJSON('config/meta.json'),
        dest: grunt.option('target') || 'dist',
        basePath: path.join('<%= dest %>', 'App_Plugins', '<%= pkgMeta.directory %>'),

        watch: {
            options: {
                spawn: false,
                atBegin: true
            },
            dll: {
                files: ['/RankOne.SEO.Tool/**/*.dll'],
                tasks: ['copy:dll']
            }
        },

        clean: {
            build: '<%= grunt.config("basePath").substring(0, 4) == "dist" ? "dist/**/*" : "null" %>',
            tmp: ['tmp']
        },

        less: {
            dist: {
                options: {
                    paths: ["src/RankOne.SEO.Tool/less"],
                },
                files: {
                    '<%= basePath %>/css/styles.css': 'src/RankOne.SEO.Tool/less/styles.less',
                }
            }
        },

        concat: {
            dist: {
                dest: '<%= basePath %>/js/scripts.js',
                src: 'src/RankOne.SEO.Tool/**/*.js'
            }
        },

        ngtemplates: {
            app: {
                cwd: 'src/RankOne.SEO.Tool/Web/UI/App_Plugins/RankOne',
                src: ['**/*.html', '!editors/**.*.html', '!dialogs/**/*.html'],
                dest: '<%= basePath %>/js/templates.js',
                options: {
                    prefix: '/App_Plugins/RankOne/',
                    module: 'umbraco',
                    htmlmin: {
                        collapseBooleanAttributes: true,
                        collapseWhitespace: true,
                        removeAttributeQuotes: true,
                        removeComments: true,
                        removeEmptyAttributes: true,
                        removeRedundantAttributes: true,
                        removeScriptTypeAttributes: true,
                        removeStyleLinkTypeAttributes: true
                    }
                }
            }
        },

        copy: {
            dll: {
                cwd: 'src/RankOne.SEO.Tool/bin/Release/',
                src: ['*.dll', '!HtmlAgilityPack.dll'],
                dest: '<%= dest %>/bin/',
                expand: true
            },
            debug: {
                cwd: 'src/RankOne.SEO.Tool/bin/Debug/',
                src: ['*.dll', '*.pdb'],
                dest: '<%= dest %>/bin/',
                expand: true
            },
            plugin: {
                cwd: 'src/RankOne.SEO.Tool/Web/UI/App_Plugins/RankOne/',
                src: ['package.manifest', 'lang/*.*', 'editors/**/*.html', 'dialogs/**/*.html', 'images/*.*'],
                dest: '<%= basePath %>',
                expand: true
            },
            nuget: {
                files: [{
                    cwd: '<%= dest %>',
                    src: ['**/*', '!bin', '!bin/*'],
                    dest: 'tmp/nuget/content',
                    expand: true
                }, {
                    cwd: '<%= dest %>/bin',
                    src: ['*.dll', '!HtmlAgilityPack.dll'],
                    dest: 'tmp/nuget/lib/net40',
                    expand: true
                }]
            },
            umbraco: {
                cwd: '<%= dest %>',
                src: '**/*',
                dest: 'tmp/umbraco',
                expand: true
            }
        },

        touch: {
            webconfig: {
                src: ['<%= grunt.option("target") %>\\Web.config']
            }
        },

        msbuild: {
            options: {
                stdout: true,
                verbosity: 'quiet',
                maxCpuCount: 4,
                version: 4.0,
                buildParameters: {
                    WarningLevel: 2,
                    NoWarn: 1607
                }
            },
            dist: {
                src: ['src/RankOne.SEO.Tool/RankOne.csproj'],
                options: {
                    projectConfiguration: 'Release',
                    targets: ['Clean', 'Rebuild'],
                }
            },
            dev: {
                src: ['src/RankOne.SEO.Tool/RankOne.csproj'],
                options: {
                    projectConfiguration: 'Debug',
                    targets: ['Clean', 'Rebuild'],
                }
            }
        },

        nugetpack: {
            dist: {
                src: 'tmp/nuget/package.nuspec',
                dest: 'pkg'
            }
        },

        template: {
            'nuspec': {
                'options': {
                    'data': {
                        name: '<%= pkgMeta.name %>',
                        version: '<%= pkgMeta.version %>',
                        url: '<%= pkgMeta.url %>',
                        license: '<%= pkgMeta.license %>',
                        licenseUrl: '<%= pkgMeta.licenseUrl %>',
                        author: '<%= pkgMeta.author %>',
                        authorUrl: '<%= pkgMeta.authorUrl %>'
                    }
                },
                'files': {
                    'tmp/nuget/package.nuspec': ['config/package.nuspec']
                }
            }
        },

        umbracoPackage: {
            dist: {
                src: 'tmp/umbraco',
                dest: 'pkg',
                options: {
                    name: "<%= pkgMeta.name %>",
                    version: '<%= pkgMeta.version %>',
                    url: '<%= pkgMeta.url %>',
                    license: '<%= pkgMeta.license %>',
                    licenseUrl: '<%= pkgMeta.licenseUrl %>',
                    author: '<%= pkgMeta.author %>',
                    authorUrl: '<%= pkgMeta.authorUrl %>',
                    readme: '<%= grunt.file.read("config/readme.txt") %>',
                    manifest: 'config/package.template.xml'
                }
            }
        }
    });

    grunt.registerTask('default', ['clean', 'msbuild:dist', 'less', 'ngtemplates', 'concat', 'copy:dll', 'copy:plugin']);
    grunt.registerTask('develop', ['clean', 'msbuild:dev', 'less', 'ngtemplates', 'concat', 'copy:debug', 'copy:plugin', 'touch']);
    grunt.registerTask('nuget', ['copy:nuget', 'template:nuspec', 'nugetpack']);
    grunt.registerTask('package', ['clean:tmp', 'default', 'nuget', 'copy:umbraco', 'umbracoPackage', 'clean:tmp']);
};
