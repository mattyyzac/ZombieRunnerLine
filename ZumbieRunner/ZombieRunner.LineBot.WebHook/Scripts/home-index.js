$(function () {
    getStories();
});

function getStories() {
    setTimeout(function (){
        async_get_stories();
    }, 150);
}

function async_get_stories() {
    $.ajax({
        type: 'GET',
        //url: 'http://zzombierunner.azurewebsites.net/api/story/demo',
        url: location.origin + '/api/story/demo',
        cache: false
    }).done(function (data) {
        //console.log(data);
        renderStories(data);
    }).fail(function (err) {
        console.log(err);
    });
}

function renderStories(data) {
    if (data === undefined || data.length === 0 || data[0].length === 0) {
        $('section#story-container').html('還沒有人開團，沒有故事劇情。');
        return;
    }

    let ret = '';
    for (var i = 0; i < data.length; i++) {
        ret += '<p style="text-align: left;">' + data[i].replace('\n', '<br />') + '</p><hr />';
    }
    $('section#story-container').html(ret);
}