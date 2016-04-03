module.exports = function(grunt) {
  require('load-grunt-tasks')(grunt);
  var path = require('path');
  
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    pkgMeta: grunt.file.readJSON('config/meta.json'),
    dest: grunt.option('target') || 'dist',
    basePath: path.join('<%= dest %>', 'App_Plugins', '<%= pkgMeta.name %>'),
    
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

    copy: {
      dll: {
        cwd: 'src/RankOne.SEO.Tool/bin/Release/',
        src: '*.dll',
        dest: '<%= dest %>/bin/',
        expand: true
      },
	  plugin:{
		cwd: 'src/RankOne.SEO.Tool/Web/UI/App_Plugins/RankOne/',
        src: ["*.*", "**/*.*"],
        dest: '<%= basePath %>',
        expand: true	
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
      }
    }
  });

  grunt.registerTask('default', ['msbuild:dist', 'copy:dll', 'copy:plugin']);
};