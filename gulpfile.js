var gulp = require('gulp');
var clean = require('gulp-clean');
var msbuild = require("gulp-msbuild");
var less = require('gulp-less');

const DEBUG_MODE = 'Debug'
const RELEASE_MODE = 'Release';

var config = {
    temp_directory: './tmp/',
    destination_directory: './dist/',
    app_plugins_directory: './dist/App_Plugins/',
    project_directory: 'src/RankOne.SEO.Tool/',
    project_file: 'src/RankOne.SEO.Tool/RankOne.csproj'

};


function build(configuration) {
    return gulp.src(config.project_file)
        .pipe(msbuild({
            targets: ['Clean', 'Rebuild'],
            toolsVersion: 15,
            errorOnFail: true,
            properties: { Configuration: configuration, WarningLevel: 2 }
        }));
}

gulp.task('less', function(){
    return gulp.src(config.project_directory + 'less/styles.less')
    .pipe(less())
    .pipe(gulp.dest(config.app_plugins_directory + "RankOne"));

});

gulp.task('clean_temp', function(){
    return gulp.src(config.temp_directory, {read: false})
        .pipe(clean());
});

gulp.task('clean_dist', function(){
    return gulp.src(config.destination_directory, {read: false})
        .pipe(clean());
});

gulp.task('clean', ['clean_temp', 'clean_dist']);

gulp.task('copy_nuget', function(){
    gulp.src(['**/*', '!bin', '!bin/*'], { cwd : config.destination_directory })
        .pipe(gulp.dest(config.temp_directory + 'nuget/content'));
    gulp.src(['*.dll', '!HtmlAgilityPack.dll'], { cwd : config.destination_directory + 'bin' })
        .pipe(gulp.dest(config.temp_directory + 'nuget/lib/net40'));
});


//gulp.task('default', ['clean', 'msbuild:dist', 'less', 'ngtemplates', 'concat', 'copy:dll', 'copy:views', 'copy:config', 'copy:plugin']);
//gulp.task('develop', ['clean', 'msbuild:dev', 'less', 'ngtemplates', 'concat', 'copy:debug', 'copy:views', 'copy:config', 'copy:plugin', 'touch']);
//gulp.task('nuget', ['copy:nuget', 'template:nuspec', 'nugetpack']);
//gulp.task('package', ['clean:tmp', 'default', 'nuget', 'copy:umbraco', 'umbracoPackage', 'clean:tmp']);

gulp.task('build_debug', () => { build(DEBUG_MODE) });
gulp.task('build_release', () => { build(RELEASE_MODE) });


gulp.task('default', ['clean', 'build_release', 'less']);
gulp.task('develop', ['clean', 'build_debug', 'less']);
gulp.task('nuget', ['copy_nuget']);
gulp.task('package', ['default', 'nuget']);