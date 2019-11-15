/// <binding BeforeBuild='scripts' Clean='clean, minify, scripts' />
/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var uglify = require('gulp-uglify');
var concat = require('gulp-concat');
var rimraf = require('rimraf');
var merge = require('merge-stream');

gulp.task('minify', function () {

    var streams = [
        gulp.src(['wwwroot/js/*.js', '!wwwroot/js/tour*.js', '!wwwroot/js/contact.js'])
            .pipe(uglify())
            .pipe(concat('wilderblog.min.js'))
            .pipe(gulp.dest('wwwroot/lib/site')),
        gulp.src(['wwwroot/js/contact.js'])
            .pipe(uglify())
            .pipe(concat('contact.min.js'))
            .pipe(gulp.dest('wwwroot/lib/site'))
    ];

    return merge(streams);
});

// Dependency Dirs
var deps = {
    'jquery': {
        'dist/jquery.slim.min.js': ''
    },
    'jquery-validation-unobtrusive': {
        'dist/jquery.validate.unobtrusive.min.js': ''
    },
    'bootstrap': {
        'dist/**/*.min.css': '',
        'dist/**/bootstrap.bundle.min.js': ''
    },
    'jquery-ui-dist': {
        'jquery-ui.min.css': '',
        'jquery-ui.min.js': '',
        'images/**': 'images'
    },
    '@fortawesome/fontawesome-free': {
        '**/all.min.css': '',
        '**/all.min.js': '',
        'webfonts/*': 'webfonts'
    },
    'cleave.js': {
        'dist/cleave.min.js': ''
    },
    'flagpack': {
        'dist/flagpack.css': 'css',
        'flags/**/*': 'flags'
    }
};

gulp.task('clean', function (cb) {
    return rimraf('wwwroot/lib/', cb);
});

gulp.task('scripts', function () {

    var streams = [];

    for (var prop in deps) {
        console.log('Prepping Scripts for: ' + prop);

        var folder = prop;
        if (folder.indexOf('/') > 0) {
            folder = prop.substring(prop.indexOf('/') + 1, prop.length);
        }

        for (var itemProp in deps[prop]) {
            streams.push(gulp.src('node_modules/' + prop + '/' + itemProp)
                .pipe(gulp.dest('wwwroot/lib/' + folder + '/' + deps[prop][itemProp])));
        }
    }

    return merge(streams);

});

gulp.task('default', gulp.series('clean', 'scripts', 'minify'));